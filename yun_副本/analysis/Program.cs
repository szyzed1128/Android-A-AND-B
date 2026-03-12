using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Newtonsoft.Json;
using System.Text;

namespace DllAnalyzer;

/// <summary>
/// DLL分析器 - 用于分析CarDemo应用的蓝牙实现
/// </summary>
class Program
{
    // DLL路径配置
    static readonly string BasePath = @"C:\workspace\OBD_Learning\examples\cs2\NewUI\debug_decompiled\unknown\assemblies";
    static readonly string OutputPath = @"C:\workspace\OBD_Learning\examples\cs2\NewUI\yun\analysis";

    // 目标DLL
    static readonly string[] TargetDlls = {
        "CarDemo.dll",
        "CarDemo.Android.dll",
        "Plugin.BLE.dll",
        "Plugin.BLE.Abstractions.dll"
    };

    // 蓝牙相关关键词
    static readonly string[] BluetoothKeywords = {
        "Bluetooth", "BLE", "Scan", "Connect", "Device", "Characteristic",
        "Service", "Adapter", "Discovery", "Pair", "GATT", "UUID",
        "Write", "Read", "Notify", "OBD", "ELM", "Serial"
    };

    static StringBuilder report = new();
    static Dictionary<string, List<MethodInfo>> foundMethods = new();
    static Dictionary<string, List<ClassInfo>> foundClasses = new();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== OBD应用DLL分析器 ===\n");

        report.AppendLine("# OBD应用蓝牙实现分析报告");
        report.AppendLine($"\n生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n");

        foreach (var dllName in TargetDlls)
        {
            var dllPath = Path.Combine(BasePath, dllName);
            if (File.Exists(dllPath))
            {
                Console.WriteLine($"分析: {dllName}");
                AnalyzeDll(dllPath, dllName);
            }
            else
            {
                Console.WriteLine($"未找到: {dllName}");
            }
        }

        // 特别分析JSBridge
        AnalyzeJSBridge();

        // 分析Plugin.BLE使用模式
        AnalyzePluginBLEUsage();

        // 生成报告
        GenerateReport();

        Console.WriteLine("\n分析完成！报告已保存。");
    }

    static void AnalyzeDll(string dllPath, string dllName)
    {
        report.AppendLine($"\n## {dllName}\n");

        try
        {
            var module = ModuleDefMD.Load(dllPath);
            var classCount = 0;
            var methodCount = 0;

            foreach (var type in module.GetTypes())
            {
                // 检查类名是否包含蓝牙关键词
                bool isRelevant = BluetoothKeywords.Any(k =>
                    type.Name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    type.FullName.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);

                // 特别关注JSBridge
                if (type.Name.Contains("JSBridge"))
                    isRelevant = true;

                if (isRelevant)
                {
                    classCount++;
                    var classInfo = new ClassInfo
                    {
                        Name = type.Name,
                        FullName = type.FullName,
                        Namespace = type.Namespace,
                        BaseType = type.BaseType?.FullName,
                        Interfaces = type.Interfaces.Select(i => i.Interface.FullName).ToList(),
                        Methods = new List<MethodInfo>()
                    };

                    report.AppendLine($"### 类: {type.FullName}\n");

                    if (type.BaseType != null)
                        report.AppendLine($"- 基类: `{type.BaseType.FullName}`");

                    if (type.Interfaces.Count > 0)
                    {
                        report.AppendLine("- 接口:");
                        foreach (var iface in type.Interfaces)
                            report.AppendLine($"  - `{iface.Interface.FullName}`");
                    }

                    report.AppendLine("\n**方法:**\n");
                    report.AppendLine("| 方法名 | 返回类型 | 参数 | 访问修饰符 |");
                    report.AppendLine("|--------|----------|------|------------|");

                    foreach (var method in type.Methods)
                    {
                        if (method.IsConstructor && method.Name == ".cctor")
                            continue;

                        methodCount++;
                        var methodInfo = new MethodInfo
                        {
                            Name = method.Name,
                            ReturnType = method.ReturnType?.FullName ?? "void",
                            Parameters = method.Parameters
                                .Where(p => !p.IsHiddenThisParameter)
                                .Select(p => $"{p.Type.FullName} {p.Name}")
                                .ToList(),
                            IsAsync = method.CustomAttributes.Any(a =>
                                a.AttributeType.Name.Contains("AsyncStateMachine")),
                            AccessModifier = GetAccessModifier(method)
                        };

                        classInfo.Methods.Add(methodInfo);

                        var paramsStr = string.Join(", ", methodInfo.Parameters);
                        report.AppendLine($"| `{method.Name}` | `{methodInfo.ReturnType}` | `{paramsStr}` | {methodInfo.AccessModifier} |");

                        // 如果是关键方法，提取IL代码
                        if (IsKeyMethod(method.Name))
                        {
                            ExtractMethodIL(method, dllName);
                        }
                    }

                    // 分析字段
                    if (type.Fields.Any())
                    {
                        report.AppendLine("\n**字段:**\n");
                        foreach (var field in type.Fields)
                        {
                            report.AppendLine($"- `{field.FieldType.FullName} {field.Name}`");
                        }
                    }

                    if (!foundClasses.ContainsKey(dllName))
                        foundClasses[dllName] = new List<ClassInfo>();
                    foundClasses[dllName].Add(classInfo);

                    report.AppendLine();
                }
            }

            Console.WriteLine($"  找到 {classCount} 个相关类, {methodCount} 个方法");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  错误: {ex.Message}");
            report.AppendLine($"分析错误: {ex.Message}");
        }
    }

    static void AnalyzeJSBridge()
    {
        report.AppendLine("\n## JSBridge 详细分析\n");

        var dllPath = Path.Combine(BasePath, "CarDemo.Android.dll");
        if (!File.Exists(dllPath)) return;

        var module = ModuleDefMD.Load(dllPath);

        foreach (var type in module.GetTypes())
        {
            if (type.Name == "JSBridge")
            {
                report.AppendLine("### callCSharpMethod 实现分析\n");

                foreach (var method in type.Methods)
                {
                    if (method.Name == "callCSharpMethod" && method.Body != null)
                    {
                        report.AppendLine("```csharp");
                        report.AppendLine("// callCSharpMethod IL代码");
                        foreach (var instr in method.Body.Instructions)
                        {
                            var operandStr = FormatOperand(instr.Operand);
                            report.AppendLine($"IL_{instr.Offset:X4}: {instr.OpCode} {operandStr}");
                        }
                        report.AppendLine("```\n");
                    }
                }

                // 查找状态机类（异步方法的实现）
                break;
            }

            // 查找callCSharpMethod的状态机
            if (type.FullName.Contains("callCSharpMethod") && type.FullName.Contains("<"))
            {
                report.AppendLine($"### 异步状态机: {type.Name}\n");

                foreach (var method in type.Methods)
                {
                    if (method.Name == "MoveNext" && method.Body != null)
                    {
                        report.AppendLine("```csharp");
                        report.AppendLine("// MoveNext 方法 - 异步逻辑实现");

                        // 只输出关键指令
                        var keyInstructions = method.Body.Instructions
                            .Where(i => i.OpCode.Code == Code.Call ||
                                       i.OpCode.Code == Code.Callvirt ||
                                       i.OpCode.Code == Code.Ldstr ||
                                       i.OpCode.Code == Code.Newobj)
                            .Take(50);

                        foreach (var instr in keyInstructions)
                        {
                            var operandStr = FormatOperand(instr.Operand);
                            report.AppendLine($"IL_{instr.Offset:X4}: {instr.OpCode} {operandStr}");
                        }
                        report.AppendLine("```\n");
                    }
                }
            }
        }

        // 查找蓝牙扫描相关方法
        report.AppendLine("### 蓝牙扫描方法查找\n");

        foreach (var type in module.GetTypes())
        {
            foreach (var method in type.Methods)
            {
                var methodNameLower = method.Name.ToLower();
                if (methodNameLower.Contains("scan") ||
                    methodNameLower.Contains("bluetooth") ||
                    methodNameLower.Contains("device"))
                {
                    report.AppendLine($"- `{type.FullName}.{method.Name}`");
                }
            }
        }
    }

    static void AnalyzePluginBLEUsage()
    {
        report.AppendLine("\n## Plugin.BLE 使用模式分析\n");

        // 分析Plugin.BLE.dll本身的接口
        var bleDllPath = Path.Combine(BasePath, "Plugin.BLE.dll");
        var bleAbstractionsDllPath = Path.Combine(BasePath, "Plugin.BLE.Abstractions.dll");

        if (File.Exists(bleAbstractionsDllPath))
        {
            report.AppendLine("### Plugin.BLE.Abstractions 接口\n");

            var module = ModuleDefMD.Load(bleAbstractionsDllPath);

            // 查找关键接口
            var keyInterfaces = new[] { "IAdapter", "IDevice", "ICharacteristic", "IService", "IBluetoothLE" };

            foreach (var type in module.GetTypes())
            {
                if (type.IsInterface && keyInterfaces.Any(k => type.Name.Contains(k)))
                {
                    report.AppendLine($"#### 接口: {type.Name}\n");
                    report.AppendLine("```csharp");
                    report.AppendLine($"public interface {type.Name}");
                    report.AppendLine("{");

                    foreach (var method in type.Methods)
                    {
                        var paramsStr = string.Join(", ", method.Parameters
                            .Where(p => !p.IsHiddenThisParameter)
                            .Select(p => $"{p.Type.FullName} {p.Name}"));
                        report.AppendLine($"    {method.ReturnType.FullName} {method.Name}({paramsStr});");
                    }

                    foreach (var prop in type.Properties)
                    {
                        report.AppendLine($"    {prop.PropertySig.RetType.FullName} {prop.Name} {{ get; }}");
                    }

                    foreach (var evt in type.Events)
                    {
                        report.AppendLine($"    event {evt.EventType.Name} {evt.Name};");
                    }

                    report.AppendLine("}");
                    report.AppendLine("```\n");
                }
            }
        }

        // 在CarDemo.dll中查找Plugin.BLE的使用
        var carDemoDllPath = Path.Combine(BasePath, "CarDemo.dll");
        if (File.Exists(carDemoDllPath))
        {
            report.AppendLine("### CarDemo.dll 中的 Plugin.BLE 调用\n");

            var module = ModuleDefMD.Load(carDemoDllPath);

            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (method.Body == null) continue;

                    var bleReferences = method.Body.Instructions
                        .Where(i => i.Operand is IMethod m &&
                                   (m.DeclaringType?.FullName?.Contains("Plugin.BLE") == true ||
                                    m.DeclaringType?.FullName?.Contains("Bluetooth") == true))
                        .ToList();

                    if (bleReferences.Any())
                    {
                        report.AppendLine($"#### {type.Name}.{method.Name}\n");
                        report.AppendLine("```");
                        foreach (var instr in bleReferences)
                        {
                            var m = instr.Operand as IMethod;
                            report.AppendLine($"  调用: {m?.DeclaringType?.Name}::{m?.Name}");
                        }
                        report.AppendLine("```\n");
                    }
                }
            }
        }
    }

    static void ExtractMethodIL(MethodDef method, string dllName)
    {
        if (method.Body == null) return;

        var methodKey = $"{dllName}_{method.DeclaringType?.Name}_{method.Name}";

        report.AppendLine($"\n#### {method.Name} IL代码\n");
        report.AppendLine("```");

        foreach (var instr in method.Body.Instructions.Take(100))
        {
            var operandStr = FormatOperand(instr.Operand);
            report.AppendLine($"IL_{instr.Offset:X4}: {instr.OpCode} {operandStr}");
        }

        if (method.Body.Instructions.Count > 100)
            report.AppendLine("... (截断)");

        report.AppendLine("```");
    }

    static bool IsKeyMethod(string methodName)
    {
        var keyMethods = new[] {
            "StartScan", "StopScan", "Connect", "Disconnect",
            "Send", "Write", "Read", "OnData", "OnCharacteristic",
            "callCSharpMethod", "startBTScan", "stopBTScan"
        };
        return keyMethods.Any(k =>
            methodName.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    static string GetAccessModifier(MethodDef method)
    {
        if (method.IsPublic) return "public";
        if (method.IsPrivate) return "private";
        if (method.IsFamily) return "protected";
        if (method.IsAssembly) return "internal";
        return "unknown";
    }

    static string FormatOperand(object? operand)
    {
        if (operand == null) return "";

        return operand switch
        {
            IMethod m => $"{m.DeclaringType?.Name}::{m.Name}",
            IField f => f.Name,
            string s => $"\"{s}\"",
            ITypeDefOrRef t => t.FullName,
            _ => operand.ToString() ?? ""
        };
    }

    static void GenerateReport()
    {
        // 保存Markdown报告
        var reportPath = Path.Combine(OutputPath, "bluetooth_analysis_report.md");
        File.WriteAllText(reportPath, report.ToString(), Encoding.UTF8);
        Console.WriteLine($"\n报告已保存到: {reportPath}");

        // 保存JSON格式的类信息
        var jsonPath = Path.Combine(OutputPath, "found_classes.json");
        var json = JsonConvert.SerializeObject(new {
            Classes = foundClasses,
            GeneratedAt = DateTime.Now
        }, Formatting.Indented);
        File.WriteAllText(jsonPath, json, Encoding.UTF8);
        Console.WriteLine($"JSON数据已保存到: {jsonPath}");

        // 生成改造点摘要
        GenerateModificationSummary();
    }

    static void GenerateModificationSummary()
    {
        var summary = new StringBuilder();
        summary.AppendLine("# WebSocket改造点摘要\n");
        summary.AppendLine("## 需要修改的关键位置\n");

        summary.AppendLine("### 1. 蓝牙扫描改造点");
        summary.AppendLine("- 原始: Plugin.BLE.Adapter.StartScanningForDevicesAsync()");
        summary.AppendLine("- 改为: WebSocketBridge.SendAsync({action: 'startScan'})");
        summary.AppendLine();

        summary.AppendLine("### 2. 蓝牙连接改造点");
        summary.AppendLine("- 原始: Plugin.BLE.Adapter.ConnectToKnownDeviceAsync()");
        summary.AppendLine("- 改为: WebSocketBridge.SendAsync({action: 'connect', address: xxx})");
        summary.AppendLine();

        summary.AppendLine("### 3. 数据发送改造点");
        summary.AppendLine("- 原始: ICharacteristic.WriteAsync(bytes)");
        summary.AppendLine("- 改为: WebSocketBridge.SendAsync({action: 'send', data: base64})");
        summary.AppendLine();

        summary.AppendLine("### 4. 数据接收改造点");
        summary.AppendLine("- 原始: ICharacteristic.ValueUpdated += handler");
        summary.AppendLine("- 改为: WebSocketBridge.MessageReceived += handler");
        summary.AppendLine();

        var summaryPath = Path.Combine(OutputPath, "modification_summary.md");
        File.WriteAllText(summaryPath, summary.ToString(), Encoding.UTF8);
        Console.WriteLine($"改造摘要已保存到: {summaryPath}");
    }
}

// 数据类定义
public class ClassInfo
{
    public string Name { get; set; } = "";
    public string FullName { get; set; } = "";
    public string? Namespace { get; set; }
    public string? BaseType { get; set; }
    public List<string> Interfaces { get; set; } = new();
    public List<MethodInfo> Methods { get; set; } = new();
}

public class MethodInfo
{
    public string Name { get; set; } = "";
    public string ReturnType { get; set; } = "";
    public List<string> Parameters { get; set; } = new();
    public bool IsAsync { get; set; }
    public string AccessModifier { get; set; } = "";
}
