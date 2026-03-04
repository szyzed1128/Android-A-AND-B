using System;
using dnlib.DotNet;

class Program {
    static void Main(string[] args) {
        var module = ModuleDefMD.Load(args[0]);
        foreach (var type in module.GetTypes()) {
            if (type.Name == "JSBridge" && type.Namespace.Contains("Renderers")) {
                Console.WriteLine($"找到 JSBridge: {type.FullName}");
                foreach (var method in type.Methods) {
                    if (method.IsConstructor && !method.IsStatic && method.HasBody) {
                        Console.WriteLine($"\n构造函数指令数: {method.Body.Instructions.Count}");
                        Console.WriteLine("异常处理器数: " + method.Body.ExceptionHandlers.Count);
                        foreach (var inst in method.Body.Instructions) {
                            Console.WriteLine($"  {inst}");
                        }
                    }
                }
            }
        }
    }
}
