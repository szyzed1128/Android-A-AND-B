using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

/// <summary>
/// 示例: 修改AndroidBluetooth2Manager添加WebSocket开关
/// </summary>
public class SamplePatch
{
    public static void ApplyPatch(string dllPath, string outputPath)
    {
        // 加载模块
        var module = ModuleDefMD.Load(dllPath);

        // 查找AndroidBluetooth2Manager类
        TypeDef targetType = null;
        foreach (var type in module.GetTypes())
        {
            if (type.Name == "AndroidBluetooth2Manager")
            {
                targetType = type;
                break;
            }
        }

        if (targetType == null)
        {
            Console.WriteLine("未找到AndroidBluetooth2Manager类");
            return;
        }

        // 添加静态字段: UseWebSocket
        var boolType = module.CorLibTypes.Boolean;
        var useWsField = new FieldDefUser(
            "UseWebSocket",
            new FieldSig(boolType),
            FieldAttributes.Public | FieldAttributes.Static);
        targetType.Fields.Add(useWsField);

        // 添加静态字段: WebSocketServerUrl
        var stringType = module.CorLibTypes.String;
        var wsUrlField = new FieldDefUser(
            "WebSocketServerUrl",
            new FieldSig(stringType),
            FieldAttributes.Public | FieldAttributes.Static);
        targetType.Fields.Add(wsUrlField);

        Console.WriteLine("已添加WebSocket配置字段");

        // 保存修改后的DLL
        var options = new ModuleWriterOptions(module)
        {
            WritePdb = false
        };
        module.Write(outputPath, options);

        Console.WriteLine($"已保存到: {outputPath}");
    }
}