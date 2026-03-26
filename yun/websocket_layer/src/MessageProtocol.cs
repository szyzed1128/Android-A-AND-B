using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OBDCloud.WebSocket
{
    /// <summary>
    /// WebSocket消息类型
    /// </summary>
    public static class MessageType
    {
        public const string Request = "request";
        public const string Response = "response";
        public const string Event = "event";
    }

    /// <summary>
    /// WebSocket操作类型
    /// </summary>
    public static class MessageAction
    {
        public const string Ping = "ping";
        public const string Pong = "pong";
        public const string StartScan = "startScan";
        public const string StopScan = "stopScan";
        public const string DeviceDiscovered = "deviceDiscovered";
        public const string ScanFinished = "scanFinished";
        public const string Connect = "connect";
        public const string Disconnect = "disconnect";
        public const string Send = "send";
        public const string OBDData = "obdData";
        public const string ConnectionLost = "connectionLost";
        public const string Error = "error";

        // 诊断命令（B -> A）
        public const string OBDConnect = "obdConnect";
        public const string OBDDisconnect = "obdDisconnect";
        public const string GetBrands = "getBrands";
        public const string GetProfiles = "getProfiles";
        public const string ApplyProfile = "applyProfile";
        public const string GetECUList = "getECUList";
        public const string ReadECUInfo = "readECUInfo";
        public const string ReadDTC = "readDTC";
        public const string ClearDTC = "clearDTC";
        public const string ReadFreezeFrame = "readFreezeFrame";
        public const string GetPIDList = "getPIDList";
        public const string StartReadPIDs = "startReadPIDs";
        public const string StopReadPIDs = "stopReadPIDs";

        // 云端事件（A -> B）
        public const string OBDStatusChanged = "obdStatusChanged";
        public const string DTCResult = "dtcResult";
        public const string DTCCleared = "dtcCleared";
        public const string ECUInfoResult = "ecuInfoResult";
        public const string FreezeFrameResult = "freezeFrameResult";
        public const string PIDValueChanged = "pidValueChanged";
        public const string BrandsResult = "brandsResult";
        public const string ProfilesResult = "profilesResult";
        public const string ECUListResult = "ecuListResult";
        public const string PIDListResult = "pidListResult";

        // Burst Snapshot Mode（B -> A request-response）
        public const string PrepareBurstSession = "prepareBurstSession";
        public const string CommitBurstSamples = "commitBurstSamples";
        public const string AbortBurstSession = "abortBurstSession";

        // Burst Replay Mode（A -> B 异步事件，replay 完成后推送）
        public const string BurstReplayCompleted = "burstReplayCompleted";
        public const string BurstReplayFailed = "burstReplayFailed";
    }

    /// <summary>
    /// Burst 阶段的结构化错误码
    /// </summary>
    public static class BurstErrorCode
    {
        /// <summary>所选 PID 包含会在 replay 期间动态插队的 lowPriorityRequiredPIDs，MVP 不支持</summary>
        public const string LowPriorityConflict = "BURST_LOW_PRIORITY_CONFLICT";

        /// <summary>某个 descriptor.command 与 bypassSet（ping / beforeCommands / afterCommands）冲突</summary>
        public const string BypassConflict = "BURST_BYPASS_CONFLICT";

        /// <summary>
        /// SharedSettings.Current 的 UseDefaultInit / Mode01Prefix / TesterPresentCommand
        /// 其中某个属性不可读，无法安全生成 pingCommandText，Prepare fail-closed
        /// </summary>
        public const string PingGenerationFailed = "BURST_PING_GENERATION_FAILED";

        /// <summary>
        /// descriptor 中存在 skipATSH=true（VwTp20 + Header=="000"）的请求。
        /// MVP 阶段 ReplayConnection 不支持此类协议的 header 状态同步，Prepare 前置拒绝。
        /// </summary>
        public const string SkipATSHUnsupported = "BURST_SKIP_ATSH_UNSUPPORTED";

        /// <summary>
        /// descriptor 中存在 isMultiRequest=true 的请求。
        /// MVP 阶段 BuildReplayQueue 不支持 OBDMultiRequest 重建，Prepare 前置拒绝。
        /// </summary>
        public const string MultiRequestUnsupported = "BURST_MULTI_REQUEST_UNSUPPORTED";
    }

    /// <summary>
    /// WebSocket消息基类
    /// </summary>
    public class WSMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("requestId", NullValueHandling = NullValueHandling.Ignore)]
        public string RequestId { get; set; }

        [JsonProperty("sessionId", NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }

        [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Success { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public long? Timestamp { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        public WSMessage()
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static WSMessage FromJson(string json)
        {
            return JsonConvert.DeserializeObject<WSMessage>(json);
        }

        /// <summary>
        /// 获取Data字段作为指定类型
        /// </summary>
        public T GetData<T>()
        {
            if (Data == null) return default;
            if (Data is T typed) return typed;
            var json = JsonConvert.SerializeObject(Data);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 获取Data字段作为Base64解码后的字节数组
        /// </summary>
        public byte[] GetDataAsBytes()
        {
            if (Data == null) return null;
            return Convert.FromBase64String(Data.ToString());
        }
    }

    /// <summary>
    /// 蓝牙设备信息
    /// </summary>
    public class BTDeviceInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("valid")]
        public bool Valid { get; set; }

        [JsonProperty("paired")]
        public bool Paired { get; set; }
    }

    /// <summary>
    /// 连接请求数据
    /// </summary>
    public class ConnectRequestData
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
    }

    /// <summary>
    /// OBD连接请求数据（B -> A）
    /// </summary>
    public class OBDConnectRequestData
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }

    /// <summary>
    /// 连接响应数据（A -> B 请求，B -> A 响应）
    /// </summary>
    public class ConnectResult
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }

    /// <summary>
    /// 错误数据
    /// </summary>
    public class ErrorData
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// 消息构建工厂
    /// </summary>
    public static class WSMessageFactory
    {
        private static string NewRequestId() => Guid.NewGuid().ToString("N").Substring(0, 8);

        // --- 请求消息构建 ---

        public static WSMessage CreatePingRequest()
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.Ping,
                RequestId = NewRequestId()
            };
        }

        public static WSMessage CreateStartScanRequest()
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.StartScan,
                RequestId = NewRequestId()
            };
        }

        public static WSMessage CreateStopScanRequest()
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.StopScan,
                RequestId = NewRequestId()
            };
        }

        public static WSMessage CreateConnectRequest(string protocol, string address)
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.Connect,
                RequestId = NewRequestId(),
                Data = new ConnectRequestData
                {
                    Protocol = protocol,
                    Address = address
                }
            };
        }

        public static WSMessage CreateDisconnectRequest(string sessionId)
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.Disconnect,
                RequestId = NewRequestId(),
                SessionId = sessionId
            };
        }

        public static WSMessage CreateSendRequest(string sessionId, byte[] data)
        {
            return new WSMessage
            {
                Type = MessageType.Request,
                Action = MessageAction.Send,
                RequestId = NewRequestId(),
                SessionId = sessionId,
                Data = Convert.ToBase64String(data)
            };
        }

        // --- 响应消息构建 (B端使用) ---

        public static WSMessage CreateResponse(string requestId, string action, bool success, string error = null)
        {
            return new WSMessage
            {
                Type = MessageType.Response,
                Action = action,
                RequestId = requestId,
                Success = success,
                Error = error
            };
        }

        public static WSMessage CreateConnectResponse(string requestId, bool success, string sessionId = null, string error = null)
        {
            return new WSMessage
            {
                Type = MessageType.Response,
                Action = MessageAction.Connect,
                RequestId = requestId,
                Success = success,
                SessionId = sessionId,
                Error = error
            };
        }

        // --- 事件消息构建 (B端使用) ---

        public static WSMessage CreateDeviceDiscoveredEvent(BTDeviceInfo device)
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.DeviceDiscovered,
                Data = device
            };
        }

        public static WSMessage CreateScanFinishedEvent()
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.ScanFinished
            };
        }

        public static WSMessage CreateOBDDataEvent(string sessionId, byte[] data)
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.OBDData,
                SessionId = sessionId,
                Data = Convert.ToBase64String(data)
            };
        }

        public static WSMessage CreateConnectionLostEvent(string sessionId, string reason)
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.ConnectionLost,
                SessionId = sessionId,
                Data = new { reason = reason }
            };
        }

        public static WSMessage CreateErrorEvent(string code, string message)
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.Error,
                Data = new ErrorData { Code = code, Message = message }
            };
        }

        public static WSMessage CreateOBDStatusChangedEvent(string status)
        {
            return new WSMessage
            {
                Type = MessageType.Event,
                Action = MessageAction.OBDStatusChanged,
                Data = new { status = status }
            };
        }
    }
}
