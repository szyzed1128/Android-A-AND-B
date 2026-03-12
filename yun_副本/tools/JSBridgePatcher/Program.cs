using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace JSBridgePatcher
{
    /// <summary>
    /// JSBridge 补丁工具 - 在 JSBridge 构造函数中注入 RegisterJSBridge 调用
    ///
    /// 目的：让 OBDCloudManager 能够调用 JSBridge 的方法（如 getBrands、getProfiles 等）
    ///
    /// 注入的代码（在构造函数末尾）：
    ///   try {
    ///       OBDCloud.WebSocket.OBDCloudManager.Instance.RegisterJSBridge(this);
    ///   } catch { }
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== JSBridge 补丁工具 ===\n");

            if (args.Length < 2)
            {
                Console.WriteLine("用法: JSBridgePatcher <CarDemo.Android.dll路径> <输出DLL路径> [OBDCloud.WebSocket.dll路径]");
                Console.WriteLine();
                Console.WriteLine("示例:");
                Console.WriteLine("  JSBridgePatcher CarDemo.Android.dll CarDemo.Android.patched.dll ../websocket_layer/bin/Release/netstandard2.0/OBDCloud.WebSocket.dll");
                return;
            }

            var inputDll = args[0];
            var outputDll = args[1];
            var wsLayerDll = args.Length > 2 ? args[2] : null;

            if (!File.Exists(inputDll))
            {
                Console.WriteLine($"错误: 找不到输入文件 {inputDll}");
                return;
            }

            try
            {
                ApplyPatch(inputDll, outputDll, wsLayerDll);
                Console.WriteLine("\n修补完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n错误: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void ApplyPatch(string inputDll, string outputDll, string wsLayerDll)
        {
            Console.WriteLine($"加载模块: {inputDll}");
            var module = ModuleDefMD.Load(inputDll);

            // 加载 WebSocket 层模块以获取类型信息
            ModuleDefMD wsModule = null;
            if (!string.IsNullOrEmpty(wsLayerDll) && File.Exists(wsLayerDll))
            {
                wsModule = ModuleDefMD.Load(wsLayerDll);
                Console.WriteLine($"加载 WebSocket 模块: {wsLayerDll}");
            }

            // 查找 JSBridge 类
            Console.WriteLine("\n步骤1: 查找 JSBridge 类...");
            TypeDef jsBridge = null;
            foreach (var type in module.GetTypes())
            {
                if (type.Name == "JSBridge" && type.Namespace.Contains("Renderers"))
                {
                    jsBridge = type;
                    break;
                }
            }

            if (jsBridge == null)
            {
                throw new Exception("未找到 JSBridge 类");
            }
            Console.WriteLine($"  找到: {jsBridge.FullName}");

            // 查找构造函数
            Console.WriteLine("\n步骤2: 查找构造函数...");
            MethodDef ctor = null;
            foreach (var method in jsBridge.Methods)
            {
                if (method.IsConstructor && !method.IsStatic && method.HasBody)
                {
                    ctor = method;
                    break;
                }
            }

            if (ctor == null)
            {
                throw new Exception("未找到 JSBridge 构造函数");
            }
            Console.WriteLine($"  找到: {ctor.Name} (参数: {ctor.Parameters.Count}, 指令数: {ctor.Body.Instructions.Count})");

            // 注入代码
            Console.WriteLine("\n步骤3: 注入 RegisterJSBridge 调用...");
            InjectRegisterJSBridge(module, jsBridge, ctor, wsModule);

            // 保存
            Console.WriteLine($"\n步骤4: 保存到 {outputDll}");

            var outputDir = Path.GetDirectoryName(outputDll);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var options = new ModuleWriterOptions(module)
            {
                WritePdb = false
            };
            // 避免混淆/特殊 IL 导致的 max stack 计算失败
            options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
            module.Write(outputDll, options);
            Console.WriteLine("  保存成功");
        }

        /// <summary>
        /// 在构造函数末尾（ret 之前）注入 RegisterJSBridge 调用
        /// 使用直接类型引用而不是反射
        /// </summary>
        static void InjectRegisterJSBridge(ModuleDefMD module, TypeDef jsBridge, MethodDef ctor, ModuleDefMD wsModule)
        {
            var body = ctor.Body;
            body.InitLocals = true;

            // 查找或创建对 OBDCloud.WebSocket.dll 的引用
            Console.WriteLine("  创建程序集引用...");

            // 创建程序集引用
            var wsAssemblyRef = new AssemblyRefUser(
                new UTF8String("OBDCloud.WebSocket"),
                new Version(1, 0, 0, 0));

            // 创建 OBDCloudManager 类型引用
            var obdCloudManagerRef = new TypeRefUser(
                module,
                "OBDCloud.WebSocket",
                "OBDCloudManager",
                wsAssemblyRef);

            // 查找 mscorlib 引用（用于 Exception 类型）
            var mscorlibRef = FindMscorlibReference(module);
            if (mscorlibRef == null)
            {
                Console.WriteLine("  警告: 未找到 mscorlib 引用，使用简化注入");
            }

            // 创建 Instance 属性的 getter 方法引用
            // public static OBDCloudManager Instance { get; }
            var instanceGetterSig = MethodSig.CreateStatic(obdCloudManagerRef.ToTypeSig());
            var instanceGetter = new MemberRefUser(
                module,
                "get_Instance",
                instanceGetterSig,
                obdCloudManagerRef);

            // 创建 RegisterJSBridge 方法引用
            // public void RegisterJSBridge(object jsBridge)
            var registerMethodSig = MethodSig.CreateInstance(
                module.CorLibTypes.Void,
                module.CorLibTypes.Object);
            var registerMethod = new MemberRefUser(
                module,
                "RegisterJSBridge",
                registerMethodSig,
                obdCloudManagerRef);

            // 找到 ret 指令的位置
            var retIndices = new System.Collections.Generic.List<int>();
            for (int i = 0; i < body.Instructions.Count; i++)
            {
                if (body.Instructions[i].OpCode == OpCodes.Ret)
                {
                    retIndices.Add(i);
                }
            }

            if (retIndices.Count == 0)
            {
                Console.WriteLine("  警告: 未找到 ret 指令");
                return;
            }

            Console.WriteLine($"  找到 {retIndices.Count} 个 ret 指令");

            // 在最后一个 ret 之前插入代码
            int insertIndex = retIndices[retIndices.Count - 1];
            var originalRet = body.Instructions[insertIndex];

            // 生成注入的指令
            var newInstructions = new System.Collections.Generic.List<Instruction>();

            // try 块开始
            // var manager = OBDCloudManager.Instance;
            var tryStart = OpCodes.Call.ToInstruction(instanceGetter);
            newInstructions.Add(tryStart);

            // manager.RegisterJSBridge(this);
            newInstructions.Add(OpCodes.Ldarg_0.ToInstruction()); // this
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(registerMethod));

            // leave -> ret
            var leaveInTry = OpCodes.Leave.ToInstruction(originalRet);
            newInstructions.Add(leaveInTry);

            // === catch 块 ===
            var catchStart = OpCodes.Pop.ToInstruction();
            newInstructions.Add(catchStart);
            // 异常已被 pop，什么都不做
            var leaveInCatch = OpCodes.Leave.ToInstruction(originalRet);
            newInstructions.Add(leaveInCatch);

            // 插入指令
            for (int i = newInstructions.Count - 1; i >= 0; i--)
            {
                body.Instructions.Insert(insertIndex, newInstructions[i]);
            }

            // 添加异常处理器
            TypeRef exceptionTypeRef;
            if (mscorlibRef != null)
            {
                exceptionTypeRef = new TypeRefUser(module, "System", "Exception", mscorlibRef);
            }
            else
            {
                // 使用 CorLibTypes 的 Object 作为后备
                exceptionTypeRef = new TypeRefUser(module, "System", "Exception",
                    new AssemblyRefUser(new UTF8String("mscorlib")));
            }

            var exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = tryStart,
                TryEnd = catchStart,
                HandlerStart = catchStart,
                HandlerEnd = originalRet,
                CatchType = exceptionTypeRef
            };
            body.ExceptionHandlers.Add(exceptionHandler);

            // 更新分支
            body.SimplifyBranches();
            body.OptimizeBranches();

            Console.WriteLine("  注入完成：在 JSBridge 构造函数中添加了 RegisterJSBridge 调用");
            Console.WriteLine("  注入代码: OBDCloudManager.Instance.RegisterJSBridge(this)");
        }

        static AssemblyRef FindMscorlibReference(ModuleDefMD module)
        {
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "mscorlib")
                    return asmRef;
            }
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "netstandard")
                    return asmRef;
            }
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "System.Runtime")
                    return asmRef;
            }
            return null;
        }
    }
}
