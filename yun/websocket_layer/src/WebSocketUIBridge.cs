using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OBDCloud.WebSocket
{
    public sealed class WebSocketUIBridge
    {
        private readonly object _primaryTarget;
        private readonly Type _primaryType;
        private readonly object _fallbackTarget;
        private readonly Type _fallbackType;
        private readonly object _invokeLock = new object();
        private readonly ConcurrentDictionary<string, MethodInfo[]> _methodCache =
            new ConcurrentDictionary<string, MethodInfo[]>(StringComparer.OrdinalIgnoreCase);

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static event Action<string> OnEvaluateJavascript;
        public static event Action<string> OutgoingMessage;

        public WebSocketUIBridge(object jsBridgeInstance, object fallbackTarget = null)
        {
            _primaryTarget = jsBridgeInstance ?? throw new ArgumentNullException(nameof(jsBridgeInstance));
            _primaryType = jsBridgeInstance.GetType();
            _fallbackTarget = fallbackTarget;
            _fallbackType = fallbackTarget?.GetType();
        }

        public bool HandleIncomingMessage(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            UiCommand command;
            try
            {
                command = JsonConvert.DeserializeObject<UiCommand>(json, JsonSettings);
            }
            catch (JsonException)
            {
                return false;
            }

            if (command == null || !string.Equals(command.Type, UiCommand.TypeName, StringComparison.OrdinalIgnoreCase))
                return false;
            if (string.IsNullOrWhiteSpace(command.Method))
                return false;

            var args = NormalizeArgs(command.Args);

            lock (_invokeLock)
            {
                if (TryInvoke(_primaryTarget, _primaryType, command.Method, args, out _))
                    return true;

                if (_fallbackTarget != null && TryInvoke(_fallbackTarget, _fallbackType, command.Method, args, out _))
                    return true;
            }

            return false;
        }

        public object InvokeMethod(string methodName, params string[] args)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("Method name is required.", nameof(methodName));

            var tokens = args == null ? Array.Empty<JToken>() : args.Select(JToken.FromObject).ToArray();

            lock (_invokeLock)
            {
                if (TryInvoke(_primaryTarget, _primaryType, methodName, tokens, out var result))
                    return result;

                if (_fallbackTarget != null && TryInvoke(_fallbackTarget, _fallbackType, methodName, tokens, out result))
                    return result;
            }

            throw new MissingMethodException($"Method '{methodName}' not found on JSBridge target.");
        }

        public static void HandleOutgoingJS(string jsScript)
        {
            if (string.IsNullOrWhiteSpace(jsScript))
                return;

            // 诊断：记录所有 EvaluateJavascriptAsync hook 调用
            var preview = jsScript.Length > 200 ? jsScript.Substring(0, 200) : jsScript;
            Console.WriteLine($"[HandleOutgoingJS] jsScript={preview}");

            var rawHandler = OnEvaluateJavascript;
            rawHandler?.Invoke(jsScript);

            if (!TryBuildUiEvent(jsScript, out var json))
            {
                Console.WriteLine($"[HandleOutgoingJS] TryBuildUiEvent 失败: {preview.Substring(0, Math.Min(60, preview.Length))}");
                return;
            }

            var hasSubscribers = OutgoingMessage != null;
            Console.WriteLine($"[HandleOutgoingJS] TryBuildUiEvent 成功, hasSubscribers={hasSubscribers}");
            var handler = OutgoingMessage;
            handler?.Invoke(json);
        }

        private static bool TryBuildUiEvent(string jsScript, out string json)
        {
            json = null;

            if (!TryParseJsCall(jsScript, out var eventName, out var argsText))
                return false;

            var dataToken = ParseArgs(argsText);
            var message = new UiEventMessage
            {
                Event = eventName,
                Data = dataToken
            };

            json = JsonConvert.SerializeObject(message, JsonSettings);
            return true;
        }

        private static JToken ParseArgs(string argsText)
        {
            if (string.IsNullOrWhiteSpace(argsText))
                return null;

            var trimmed = argsText.Trim();
            if (IsQuoted(trimmed))
                return new JValue(Unquote(trimmed));

            if (TryParseJson(trimmed, out var token))
                return NormalizeArgumentToken(token);

            if (TryParseJson("[" + trimmed + "]", out token) && token is JArray arrayToken)
            {
                if (arrayToken.Count == 0)
                    return null;
                if (arrayToken.Count == 1)
                    return arrayToken[0];
                return arrayToken;
            }

            return new JValue(trimmed);
        }

        private static JToken NormalizeArgumentToken(JToken token)
        {
            if (token is JArray arrayToken)
            {
                if (arrayToken.Count == 0)
                    return null;
                if (arrayToken.Count == 1)
                    return arrayToken[0];
            }

            return token;
        }

        private static bool TryParseJson(string text, out JToken token)
        {
            try
            {
                token = JToken.Parse(text);
                return true;
            }
            catch (JsonException)
            {
                token = null;
                return false;
            }
        }

        private static bool TryParseJsCall(string jsScript, out string eventName, out string argsText)
        {
            eventName = null;
            argsText = null;

            if (string.IsNullOrWhiteSpace(jsScript))
                return false;

            var script = jsScript.Trim();
            if (script.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
                script = script.Substring("javascript:".Length);

            script = script.Trim();
            if (script.EndsWith(";", StringComparison.Ordinal))
                script = script.Substring(0, script.Length - 1).Trim();

            var openIndex = script.IndexOf('(');
            if (openIndex < 0)
                return false;

            var closeIndex = FindMatchingParen(script, openIndex);
            if (closeIndex < 0)
                return false;

            var funcExpr = script.Substring(0, openIndex).Trim();
            if (string.IsNullOrWhiteSpace(funcExpr))
                return false;

            if (funcExpr.StartsWith("window.", StringComparison.OrdinalIgnoreCase))
                funcExpr = funcExpr.Substring("window.".Length);
            if (funcExpr.StartsWith("this.", StringComparison.OrdinalIgnoreCase))
                funcExpr = funcExpr.Substring("this.".Length);

            var lastDot = funcExpr.LastIndexOf('.');
            eventName = lastDot >= 0 ? funcExpr.Substring(lastDot + 1) : funcExpr;
            argsText = script.Substring(openIndex + 1, closeIndex - openIndex - 1).Trim();

            return !string.IsNullOrWhiteSpace(eventName);
        }

        private static int FindMatchingParen(string text, int openIndex)
        {
            var depth = 0;
            var inSingle = false;
            var inDouble = false;
            var escape = false;

            for (var i = openIndex; i < text.Length; i++)
            {
                var c = text[i];

                if (escape)
                {
                    escape = false;
                    continue;
                }

                if ((inSingle || inDouble) && c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (!inDouble && c == '\'')
                {
                    inSingle = !inSingle;
                    continue;
                }

                if (!inSingle && c == '"')
                {
                    inDouble = !inDouble;
                    continue;
                }

                if (inSingle || inDouble)
                    continue;

                if (c == '(')
                {
                    depth++;
                }
                else if (c == ')')
                {
                    depth--;
                    if (depth == 0)
                        return i;
                }
            }

            return -1;
        }

        private static bool IsQuoted(string text)
        {
            return text.Length >= 2 &&
                   ((text[0] == '"' && text[text.Length - 1] == '"') ||
                    (text[0] == '\'' && text[text.Length - 1] == '\''));
        }

        private static string Unquote(string text)
        {
            if (!IsQuoted(text))
                return text;

            return text.Substring(1, text.Length - 2);
        }

        private bool TryInvoke(object target, Type targetType, string methodName, JToken[] args, out object result)
        {
            result = null;
            if (target == null || targetType == null)
                return false;

            var methods = GetMethods(targetType, methodName);
            foreach (var method in methods)
            {
                if (!TryBuildArguments(method.GetParameters(), args, out var prepared))
                    continue;

                result = method.Invoke(target, prepared);
                return true;
            }

            return false;
        }

        private MethodInfo[] GetMethods(Type targetType, string methodName)
        {
            var key = targetType.FullName + "::" + methodName;
            return _methodCache.GetOrAdd(key, _ =>
                targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => string.Equals(m.Name, methodName, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(m => m.GetParameters().Length)
                    .ToArray());
        }

        private static bool TryBuildArguments(ParameterInfo[] parameters, JToken[] args, out object[] prepared)
        {
            prepared = null;
            args ??= Array.Empty<JToken>();

            if (parameters.Length == 0)
            {
                if (args.Length == 0)
                {
                    prepared = Array.Empty<object>();
                    return true;
                }

                return false;
            }

            if (args.Length > parameters.Length)
                return false;

            if (args.Length < parameters.Length && !parameters.Skip(args.Length).All(p => p.IsOptional))
                return false;

            prepared = new object[parameters.Length];

            for (var i = 0; i < args.Length; i++)
            {
                if (!TryConvertArg(args[i], parameters[i].ParameterType, out var value))
                    return false;

                prepared[i] = value;
            }

            for (var i = args.Length; i < parameters.Length; i++)
            {
                prepared[i] = Type.Missing;
            }

            return true;
        }

        private static bool TryConvertArg(JToken token, Type targetType, out object value)
        {
            value = null;

            if (token == null || token.Type == JTokenType.Null)
                return true;

            if (targetType == typeof(JToken))
            {
                value = token;
                return true;
            }

            if (targetType == typeof(string))
            {
                value = token.Type == JTokenType.String
                    ? token.Value<string>()
                    : token.ToString(Formatting.None);
                return true;
            }

            if (targetType == typeof(string[]))
            {
                if (token is JArray arrayToken)
                {
                    value = arrayToken.Select(item => item.Type == JTokenType.String
                        ? item.Value<string>()
                        : item.ToString(Formatting.None)).ToArray();
                    return true;
                }

                value = new[] { token.ToString(Formatting.None) };
                return true;
            }

            if (targetType.IsEnum)
            {
                try
                {
                    var enumText = token.Type == JTokenType.String
                        ? token.Value<string>()
                        : token.ToString(Formatting.None);
                    value = Enum.Parse(targetType, enumText, true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                value = token.ToObject(targetType, JsonSerializer.CreateDefault());
                return true;
            }
            catch
            {
                try
                {
                    value = Convert.ChangeType(token.ToString(Formatting.None), targetType, CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public sealed class UiCommand
        {
            public const string TypeName = "ui_command";

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("args")]
            public JToken Args { get; set; }
        }

        public sealed class UiEventMessage
        {
            [JsonProperty("type")]
            public string Type { get; set; } = "ui_event";

            [JsonProperty("event")]
            public string Event { get; set; }

            [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
            public JToken Data { get; set; }
        }

        private static JToken[] NormalizeArgs(JToken argsToken)
        {
            if (argsToken == null || argsToken.Type == JTokenType.Null)
                return Array.Empty<JToken>();

            if (argsToken is JArray arrayToken)
                return arrayToken.ToArray();

            return new[] { argsToken };
        }
    }
}
