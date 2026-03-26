using System;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using Newtonsoft.Json.Linq;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// Burst Replay Mode 专用 IOBDConnection 实现。
    ///
    /// 消费模型：顺序指针唯一消耗路径。
    ///   _entries[]  按 sampleOrdinal 升序排列的 transcript 快照
    ///   _pointer    下一条待消费的索引（只增不减）
    ///   _currentHeader  当前 ATSH 设置的 header（随 ATSH 命令更新）
    ///
    /// WriteBytesAsync 固定顺序：
    ///   1. 解码命令文本
    ///   2. 若 ATSH → 更新 _currentHeader，缓存 OK\r\r>，返回
    ///   3. 若命中 bypassSet → 缓存 OK\r\r>，返回
    ///   4. 若 _pointer >= _entries.Length → 缓存 NO DATA\r\r>，通知 exhausted，返回
    ///   5. 计算 actualKey = normalizedCommand + "|" + _currentHeader
    ///   6. 与 _entries[_pointer].exchangeKey 比较
    ///   7. 匹配 → 消费并 pointer++；若消费后 pointer == length → 通知 exhausted
    ///   8. 不匹配 → 缓存 NO DATA\r\r>，通知 mismatch，不推进 pointer
    ///
    /// ReadBytesAsync：返回 _cachedResponse（WriteBytesAsync 已填充，含 > 终结符）
    ///   ReadData 循环依赖 > 终结符退出，所以 _cachedResponse 必须始终含 >。
    ///   无缓存时返回 Array.Empty<byte>()（ReadData 会 Delay(10) 重试，不会崩溃）。
    ///
    /// 红线：
    ///   - 不修改 OBDDataReader 任何字段
    ///   - 不影响非 Burst 路径
    ///   - 终止后所有方法均安全（返回 NO DATA 或空数组）
    /// </summary>
    public class ReplayConnection : IOBDConnection
    {
        // ── transcript ────────────────────────────────────────────────────
        private readonly JObject[] _entries;   // 按 sampleOrdinal 升序
        private int _pointer;                  // 下一条待消费索引

        // ── 状态 ──────────────────────────────────────────────────────────
        private string _currentHeader;         // 当前 ATSH 设置的 header（大写，无空格）
        private readonly System.Collections.Generic.HashSet<string> _bypassSet;
        private volatile bool _active;
        private volatile bool _terminated;
        private volatile bool _hasSeenFirstWrite;
        private volatile bool _hasPrimedInitialPrompt;
        private volatile bool _hasLoggedInitialPromptRead;
        private volatile bool _hasLoggedPreWriteEmptyRead;
        private readonly DateTime _createdUtc;

        // ── 通信 ──────────────────────────────────────────────────────────
        private byte[] _cachedResponse;        // WriteBytesAsync 填充，ReadBytesAsync 消费
        private readonly System.Threading.Tasks.TaskCompletionSource<string> _terminationTcs;
        private readonly Action<string> _logger;
        private readonly Action<JObject, int> _onEntryConsumed;
        private readonly string _sessionId;

        // ── IOBDConnection 属性 ───────────────────────────────────────────
        public bool KeepAlive { get; set; }
        public bool NoDelay { get; set; }
        public bool Connected => _active;
        public bool NeedFlush => false;

        // ── 构造函数 ──────────────────────────────────────────────────────
        /// <param name="sortedEntries">按 sampleOrdinal 升序排列的 JObject[]</param>
        /// <param name="bypassSet">ping + beforeCommands + afterCommands（小写）</param>
        /// <param name="terminationTcs">replay 终止信号</param>
        /// <param name="logger">日志回调</param>
        public ReplayConnection(
            JObject[] sortedEntries,
            System.Collections.Generic.HashSet<string> bypassSet,
            System.Threading.Tasks.TaskCompletionSource<string> terminationTcs,
            Action<string> logger,
            Action<JObject, int> onEntryConsumed,
            string sessionId)
        {
            _entries = sortedEntries ?? Array.Empty<JObject>();
            _bypassSet = bypassSet ?? new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _terminationTcs = terminationTcs ?? throw new ArgumentNullException(nameof(terminationTcs));
            _logger = logger ?? (_ => { });
            _onEntryConsumed = onEntryConsumed;
            _sessionId = sessionId ?? "";
            _pointer = 0;
            _currentHeader = "";
            _active = true;
            _terminated = false;
            _hasSeenFirstWrite = false;
            _hasPrimedInitialPrompt = false;
            _hasLoggedInitialPromptRead = false;
            _hasLoggedPreWriteEmptyRead = false;
            _createdUtc = DateTime.UtcNow;
            _cachedResponse = null;

            _logger($"[ReplayConnection] 构造完成: entries={_entries.Length} bypass={_bypassSet.Count}");
        }

        // ── WriteBytesAsync ───────────────────────────────────────────────
        public Task WriteBytesAsync(byte[] data)
        {
            if (_terminated)
            {
                _cachedResponse = Encoding.ASCII.GetBytes("NO DATA\r\r>");
                return Task.CompletedTask;
            }

            try
            {
                _hasSeenFirstWrite = true;

                // 1. 解码命令文本（去掉末尾 \r，转大写，去空格）
                var raw = Encoding.ASCII.GetString(data).TrimEnd('\r', '\n', ' ');
                var cmd = raw.Replace(" ", "").ToUpperInvariant();

                _logger($"[ReplayConnection] Write: [{cmd}]");

                // 2. ATSH → 更新 _currentHeader，缓存 OK\r\r>，返回
                if (cmd.StartsWith("ATSH", StringComparison.Ordinal))
                {
                    _currentHeader = cmd.Substring(4); // "ATSH7E0" → "7E0"
                    _logger($"[ReplayConnection] ATSH → _currentHeader=[{_currentHeader}]");
                    _cachedResponse = Encoding.ASCII.GetBytes("OK\r\r>");
                    return Task.CompletedTask;
                }

                // 3. bypassSet → 缓存 OK\r\r>，返回
                if (_bypassSet.Contains(cmd))
                {
                    _logger($"[ReplayConnection] bypass: [{cmd}] → OK");
                    _cachedResponse = Encoding.ASCII.GetBytes("OK\r\r>");
                    return Task.CompletedTask;
                }

                // 4. pointer 越界 → exhausted
                if (_pointer >= _entries.Length)
                {
                    _logger($"[ReplayConnection] pointer={_pointer} >= length={_entries.Length} → exhausted");
                    _cachedResponse = Encoding.ASCII.GetBytes("NO DATA\r\r>");
                    Terminate("exhausted");
                    return Task.CompletedTask;
                }

                // 5. 计算 actualKey = normalizedCommand + "|" + _currentHeader
                var actualKey = cmd + "|" + _currentHeader;

                // 6. 与 _entries[_pointer].exchangeKey 比较
                var expected = _entries[_pointer].Value<string>("exchangeKey") ?? "";

                if (!string.Equals(actualKey, expected, StringComparison.OrdinalIgnoreCase))
                {
                    var actualParts = actualKey.Split(new[] { '|' }, 2);
                    var expectedParts = expected.Split(new[] { '|' }, 2);
                    var actualCommand = actualParts.Length > 0 ? actualParts[0] : "";
                    var actualHeader = actualParts.Length > 1 ? actualParts[1] : "";
                    var expectedCommand = expectedParts.Length > 0 ? expectedParts[0] : "";
                    var expectedHeader = expectedParts.Length > 1 ? expectedParts[1] : "";
                    // 8. 不匹配 → mismatch，不推进 pointer
                    _logger($"[ReplayConnection] ✗ mismatch: sessionId={_sessionId} pointer={_pointer} expectedCommand=[{expectedCommand}] expectedHeader=[{expectedHeader}] actualCommand=[{actualCommand}] actualHeader=[{actualHeader}]");
                    _cachedResponse = Encoding.ASCII.GetBytes("NO DATA\r\r>");
                    Terminate("mismatch");
                    return Task.CompletedTask;
                }

                // 7. 匹配 → 消费
                var rawElmText = _entries[_pointer].Value<string>("rawElmText") ?? "NO DATA\r\r>";
                // 确保含 > 终结符（ReadData 依赖 > 退出循环）
                if (!rawElmText.Contains(">"))
                    rawElmText = rawElmText.TrimEnd('\r', '\n') + "\r\r>";
                _cachedResponse = Encoding.ASCII.GetBytes(rawElmText);

                _logger($"[ReplayConnection] ✓ match pointer={_pointer} key=[{actualKey}]");
                try
                {
                    _onEntryConsumed?.Invoke(_entries[_pointer], _pointer);
                }
                catch (Exception callbackEx)
                {
                    _logger($"[ReplayConnection] onEntryConsumed 异常: {callbackEx.Message}");
                }
                _pointer++;

                // 消费后检查是否已耗尽
                if (_pointer >= _entries.Length)
                {
                    _logger("[ReplayConnection] ✓ transcript 全部消耗完毕 → exhausted");
                    Terminate("exhausted");
                }
            }
            catch (Exception ex)
            {
                _logger($"[ReplayConnection] WriteBytesAsync 异常: {ex.Message}");
                _cachedResponse = Encoding.ASCII.GetBytes("NO DATA\r\r>");
                Terminate("exception");
            }

            return Task.CompletedTask;
        }

        // ── ReadBytesAsync ────────────────────────────────────────────────
        /// <summary>
        /// 返回 WriteBytesAsync 填充的缓存响应。
        /// ReadData 循环：bytes.Length == 0 时 Delay(10) 重试；含 > 时退出。
        /// 无缓存时返回 Array.Empty（ReadData 会重试，不会崩溃）。
        /// </summary>
        public ValueTask<byte[]> ReadBytesAsync()
        {
            var resp = _cachedResponse;
            if (resp == null || resp.Length == 0)
            {
                if (!_hasSeenFirstWrite && !_terminated && !_hasLoggedPreWriteEmptyRead)
                {
                    _hasLoggedPreWriteEmptyRead = true;
                    _logger("[ReplayConnection] 首写前空读 → Array.Empty");
                }

                return new ValueTask<byte[]>(Array.Empty<byte>());
            }

            _cachedResponse = null; // 消费一次后清空，防止重复读

            if (!_hasSeenFirstWrite && _hasPrimedInitialPrompt && !_hasLoggedInitialPromptRead && IsPromptOnly(resp))
            {
                _hasLoggedInitialPromptRead = true;
                _logger("[ReplayConnection] 初始 prompt 已被消费 [\r\r>]");
            }

            return new ValueTask<byte[]>(resp);
        }

        // ── ConnectAsync：synthetic 成功，不触发任何真实连接副作用 ──────────
        public Task<bool> ConnectAsync(string host_port, PCLDebugStream debugStream)
        {
            var ok = _active && !_terminated;
            if (ok && !_hasSeenFirstWrite && (_cachedResponse == null || _cachedResponse.Length == 0))
            {
                _cachedResponse = Encoding.ASCII.GetBytes("\r\r>");
                _hasPrimedInitialPrompt = true;
                _hasLoggedPreWriteEmptyRead = false;
                _logger("[ReplayConnection] primed initial prompt [\r\r>]");
            }

            _logger($"[ReplayConnection] ConnectAsync synthetic result={ok} host_port={host_port ?? "(null)"}");
            return Task.FromResult(ok);
        }

        // ── FlushAsync：空操作 ────────────────────────────────────────────
        public Task FlushAsync()
        {
            return Task.CompletedTask;
        }

        // ── Disconect：标记非活跃（原始拼写保留）────────────────────────
        public void Disconect()
        {
            _logger("[ReplayConnection] Disconect 被调用");

            var ageMs = (DateTime.UtcNow - _createdUtc).TotalMilliseconds;
            if (!_hasSeenFirstWrite && ageMs < 2500)
            {
                _logger($"[ReplayConnection] 忽略启动窗口内的 Disconect ageMs={ageMs:0}");
                return;
            }

            if (!_terminated)
                Terminate("disconnect");
        }

        // ── 公共方法：主动 abort ──────────────────────────────────────────
        public void Abort()
        {
            _logger("[ReplayConnection] Abort 被调用");
            if (!_terminated)
                Terminate("abort");
        }

        private static bool IsPromptOnly(byte[] data)
        {
            return data != null
                && data.Length == 3
                && data[0] == (byte)'\r'
                && data[1] == (byte)'\r'
                && data[2] == (byte)'>';
        }

        // ── 内部：触发终止信号（幂等）────────────────────────────────────
        private void Terminate(string reason)
        {
            if (_terminated) return;
            _terminated = true;
            _active = false;
            _logger($"[ReplayConnection] Terminate: reason={reason}");
            _terminationTcs.TrySetResult(reason);
        }
    }
}
