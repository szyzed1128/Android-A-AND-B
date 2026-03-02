#r "nuget: dnlib, 4.4.0"
using dnlib.DotNet;

var module = ModuleDefMD.Load("/Users/zed/Documents/Programe/CarUI/NewUI/yun/modified_dlls/CarScannerXamarinForms.patched.dll");

Console.WriteLine("=== 验证修补后的 DLL ===\n");

// 查找 OBDDataReader 类
TypeDef odr = null;
foreach (var type in module.GetTypes())
{
    if (type.Name == "OBDDataReader")
    {
        odr = type;
        break;
    }
}

if (odr != null)
{
    Console.WriteLine($"找到类: {odr.FullName}");
    
    // 检查新增的字段
    Console.WriteLine("\n新增字段:");
    foreach (var field in odr.Fields)
    {
        if (field.Name == "UseWebSocketConnection" || field.Name == "WebSocketConnection")
        {
            Console.WriteLine($"  ✓ {field.Name} ({field.FieldType})");
        }
    }
    
    // 检查新增的方法
    Console.WriteLine("\n新增方法:");
    foreach (var method in odr.Methods)
    {
        if (method.Name == "InitializeWebSocket" || 
            method.Name == "DisableWebSocket" || 
            method.Name == "IsWebSocketEnabled")
        {
            Console.WriteLine($"  ✓ {method.Name}()");
        }
    }
    
    // 检查 Connect 状态机
    Console.WriteLine("\n状态机 MoveNext 指令数:");
    foreach (var nested in odr.NestedTypes)
    {
        if (nested.Name.Contains("Connect") && nested.Name.Contains("d__"))
        {
            foreach (var m in nested.Methods)
            {
                if (m.Name == "MoveNext" && m.HasBody)
                {
                    Console.WriteLine($"  {nested.Name}.MoveNext: {m.Body.Instructions.Count} 条指令");
                    
                    // 统计 ldsfld UseWebSocketConnection 的数量
                    int wsChecks = 0;
                    foreach (var instr in m.Body.Instructions)
                    {
                        if (instr.OpCode == dnlib.DotNet.Emit.OpCodes.Ldsfld)
                        {
                            var field = instr.Operand as dnlib.DotNet.IField;
                            if (field?.Name == "UseWebSocketConnection")
                                wsChecks++;
                        }
                    }
                    Console.WriteLine($"  WebSocket 条件检查次数: {wsChecks}");
                }
            }
        }
    }
}

Console.WriteLine("\n验证完成!");
