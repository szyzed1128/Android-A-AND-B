using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace DllInjector
{
    public static class UiHookInjector
    {
        public static void InjectUiHooks(string carDemoDllPath, string websocketLayerDllPath, string outputPath)
        {
            var module = ModuleDefMD.Load(carDemoDllPath);

            var hookModule = ModuleDefMD.Load(websocketLayerDllPath);
            var hookType = hookModule.Find("OBDCloud.WebSocket.WebSocketUIBridge", false);
            if (hookType == null)
                throw new InvalidOperationException("WebSocketUIBridge type not found in hook assembly.");

            var hookMethod = hookType.Methods.First(m =>
                m.Name == "HandleOutgoingJS" && m.IsStatic && m.Parameters.Count == 1);
            var hookMethodRef = module.Import(hookMethod);

            foreach (var type in module.GetTypes().Where(t =>
                         t.FullName.StartsWith("CarDemo.Controls.MainWebView", StringComparison.Ordinal)))
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    var instructions = method.Body.Instructions;
                    for (var i = 0; i < instructions.Count; i++)
                    {
                        var instr = instructions[i];
                        if (!IsEvaluateJavaScriptAsyncCall(instr))
                            continue;

                        // Stack: [webView, scriptString]
                        instructions.Insert(i, OpCodes.Dup.ToInstruction());
                        instructions.Insert(i + 1, OpCodes.Call.ToInstruction(hookMethodRef));
                        i += 2;
                    }
                }
            }

            var options = new ModuleWriterOptions(module)
            {
                WritePdb = false
            };
            module.Write(outputPath, options);
        }

        private static bool IsEvaluateJavaScriptAsyncCall(Instruction instr)
        {
            if (instr.OpCode != OpCodes.Call && instr.OpCode != OpCodes.Callvirt)
                return false;

            if (instr.Operand is not IMethod method)
                return false;

            return method.Name == "EvaluateJavaScriptAsync" &&
                   method.DeclaringType?.FullName == "Xamarin.Forms.WebView";
        }
    }
}
