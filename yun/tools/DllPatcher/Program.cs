using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace DllPatcher
{
    /// <summary>
    /// DLL修补工具 - 将WebSocket层注入到CarDemo DLL中
    ///
    /// 核心改造：
    /// 在 OBDDataReader.Connect() 方法中，将蓝牙连接创建替换为 WebSocketBluetoothConnection
    ///
    /// 改造前：
    ///   this.Connection = DependencyService.Get<IBluetoothConnection>();
    ///
    /// 改造后：
    ///   if (OBDCloudManager.IsInitialized)
    ///       this.Connection = OBDCloudManager.Instance.BluetoothConnection;
    ///   else
    ///       this.Connection = DependencyService.Get<IBluetoothConnection>();
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== OBD云端化 DLL修补工具 ===\n");

            if (args.Length < 2)
            {
                Console.WriteLine("用法: DllPatcher <原始DLL路径> <输出DLL路径> [WebSocket DLL路径]");
                Console.WriteLine();
                Console.WriteLine("示例:");
                Console.WriteLine("  DllPatcher CarDemo.Android.dll CarDemo.Android.patched.dll");
                Console.WriteLine("  DllPatcher CarDemo.Android.dll CarDemo.Android.patched.dll OBDCloud.WebSocket.dll");
                return;
            }

            var inputDll = args[0];
            var outputDll = args[1];
            var webSocketDll = args.Length > 2 ? args[2] : null;

            if (!File.Exists(inputDll))
            {
                Console.WriteLine($"错误: 找不到输入文件 {inputDll}");
                return;
            }

            try
            {
                ApplyPatch(inputDll, outputDll, webSocketDll);
                Console.WriteLine("\n修补完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n错误: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void ApplyPatch(string inputDll, string outputDll, string webSocketDll)
        {
            Console.WriteLine($"加载模块: {inputDll}");
            var module = ModuleDefMD.Load(inputDll);

            // 步骤1: 查找 OBDDataReader 类
            Console.WriteLine("\n步骤1: 查找目标类...");
            TypeDef obdDataReader = FindType(module, "OBDDataReader");
            if (obdDataReader == null)
            {
                throw new Exception("未找到 OBDDataReader 类");
            }
            Console.WriteLine($"  找到: {obdDataReader.FullName}");

            // 步骤2: 查找 Connect 方法
            Console.WriteLine("\n步骤2: 查找 Connect 方法...");
            MethodDef connectMethod = FindConnectMethod(obdDataReader);
            if (connectMethod == null)
            {
                throw new Exception("未找到 Connect 方法");
            }
            Console.WriteLine($"  找到: {connectMethod.Name} (指令数: {connectMethod.Body?.Instructions.Count ?? 0})");

            // 步骤3: 分析 Connection 字段赋值点
            Console.WriteLine("\n步骤3: 分析连接赋值点...");
            var assignmentPoints = FindConnectionAssignments(connectMethod);
            Console.WriteLine($"  找到 {assignmentPoints.Count} 个赋值点");
            foreach (var point in assignmentPoints)
            {
                Console.WriteLine($"    - IL_{point.Offset:X4}: {point.OpCode} {point.Operand}");
            }

            // 步骤4: 添加 OBDCloudManager 引用
            Console.WriteLine("\n步骤4: 准备注入...");

            // 如果提供了 WebSocket DLL，加载它以获取类型引用
            ModuleDefMD wsModule = null;
            if (!string.IsNullOrEmpty(webSocketDll) && File.Exists(webSocketDll))
            {
                wsModule = ModuleDefMD.Load(webSocketDll);
                Console.WriteLine($"  加载 WebSocket 模块: {webSocketDll}");
            }

            // 步骤5: 注入 WebSocket 支持（添加字段、方法、修改IL）
            Console.WriteLine("\n步骤5: 注入 WebSocket 支持...");
            InjectInitialization(module, obdDataReader, connectMethod, wsModule);

            // 步骤7: 保存修改后的 DLL
            Console.WriteLine($"\n步骤7: 保存到 {outputDll}");

            // 优化所有修改过的方法的分支指令
            Console.WriteLine("  优化分支指令...");
            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (method.HasBody)
                    {
                        method.Body.SimplifyBranches();
                        method.Body.OptimizeBranches();
                    }
                }
            }

            var options = new ModuleWriterOptions(module)
            {
                WritePdb = false
            };
            // 保留原始 MaxStack 值，避免 dnlib 对注入后的复杂方法体重新计算 MaxStack 出错
            options.MetadataOptions.Flags |= dnlib.DotNet.Writer.MetadataFlags.KeepOldMaxStack;

            // 确保输出目录存在
            var outputDir = Path.GetDirectoryName(outputDll);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            module.Write(outputDll, options);
            Console.WriteLine("  保存成功");
        }

        static TypeDef FindType(ModuleDefMD module, string typeName)
        {
            // 先尝试精确匹配
            foreach (var type in module.GetTypes())
            {
                if (type.Name == typeName)
                {
                    return type;
                }
            }

            // 如果没找到，尝试模糊匹配（但排除以 typeName 开头的其他类）
            foreach (var type in module.GetTypes())
            {
                // 只匹配完全相同的名称，或者命名空间.名称 包含目标
                if (type.FullName.EndsWith("." + typeName) || type.FullName == typeName)
                {
                    return type;
                }
            }

            return null;
        }

        static MethodDef FindConnectMethod(TypeDef type)
        {
            // 查找 Connect 方法（异步方法可能有不同的名称）
            MethodDef connectEntry = null;
            foreach (var method in type.Methods)
            {
                // 主要的 Connect 方法入口
                if (method.Name == "Connect" && method.HasBody)
                {
                    connectEntry = method;
                    break;
                }
            }

            // 查找异步状态机的 MoveNext 方法（这是实际包含逻辑的地方）
            foreach (var nestedType in type.NestedTypes)
            {
                // 状态机类名格式: <Connect>d__数字
                if (nestedType.Name.StartsWith("<Connect>") && nestedType.Name.Contains("d__"))
                {
                    Console.WriteLine($"  找到状态机类: {nestedType.Name}");
                    foreach (var method in nestedType.Methods)
                    {
                        if (method.Name == "MoveNext" && method.HasBody)
                        {
                            Console.WriteLine($"  找到 MoveNext 方法 (指令数: {method.Body.Instructions.Count})");
                            return method;
                        }
                    }
                }
            }

            // 如果没找到状态机，返回入口方法
            return connectEntry;
        }

        static List<Instruction> FindConnectionAssignments(MethodDef method)
        {
            var results = new List<Instruction>();
            if (method.Body == null) return results;

            foreach (var instr in method.Body.Instructions)
            {
                // 查找 stfld Connection 指令
                if (instr.OpCode == OpCodes.Stfld && instr.Operand is IField field)
                {
                    if (field.Name == "Connection")
                    {
                        results.Add(instr);
                    }
                }
            }

            return results;
        }

        static void InjectInitialization(ModuleDefMD module, TypeDef obdDataReader,
            MethodDef connectMethod, ModuleDefMD wsModule)
        {
            // 策略：
            // 1. 添加静态字段标记是否使用 WebSocket
            // 2. 添加静态字段存储 WebSocket 连接实例
            // 3. 修改 Connect 方法的 IL 代码，在每个 Connection 赋值点添加条件判断

            // 添加静态字段到 OBDDataReader
            var boolType = module.CorLibTypes.Boolean;

            // UseWebSocketConnection 字段
            FieldDef useWsField;
            if (!obdDataReader.Fields.Any(f => f.Name == "UseWebSocketConnection"))
            {
                useWsField = new FieldDefUser(
                    "UseWebSocketConnection",
                    new FieldSig(boolType),
                    FieldAttributes.Public | FieldAttributes.Static);
                obdDataReader.Fields.Add(useWsField);
                Console.WriteLine("  添加字段: UseWebSocketConnection");
            }
            else
            {
                useWsField = obdDataReader.Fields.First(f => f.Name == "UseWebSocketConnection");
            }

            // WebSocketConnection 字段（存储实现 IOBDConnection 的 WebSocket 连接）
            var objType = module.CorLibTypes.Object;
            FieldDef wsConnField;
            if (!obdDataReader.Fields.Any(f => f.Name == "WebSocketConnection"))
            {
                wsConnField = new FieldDefUser(
                    "WebSocketConnection",
                    new FieldSig(objType),
                    FieldAttributes.Public | FieldAttributes.Static);
                obdDataReader.Fields.Add(wsConnField);
                Console.WriteLine("  添加字段: WebSocketConnection");
            }
            else
            {
                wsConnField = obdDataReader.Fields.First(f => f.Name == "WebSocketConnection");
            }

            // 添加初始化方法
            AddInitializeWebSocketMethod(module, obdDataReader, useWsField, wsConnField);

            // 修改 Connect 方法的 IL 代码
            Console.WriteLine("\n  修改 Connection 赋值逻辑...");
            ModifyConnectionAssignments(module, obdDataReader, connectMethod, useWsField, wsConnField);

            Console.WriteLine("\n注入完成。使用方式：");
            Console.WriteLine("  1. 设置 OBDDataReader.WebSocketConnection = yourIOBDConnectionInstance");
            Console.WriteLine("  2. 设置 OBDDataReader.UseWebSocketConnection = true");
            Console.WriteLine("  3. 或调用 OBDDataReader.InitializeWebSocket(connection)");
        }

        /// <summary>
        /// 修改 Connect 方法中的 Connection 赋值点
        /// 原始: this.Connection = new BTLEConnection(...);
        /// 修改后:
        ///   if (UseWebSocketConnection && WebSocketConnection != null)
        ///       this.Connection = (IOBDConnection)WebSocketConnection;
        ///   else
        ///       this.Connection = new BTLEConnection(...);
        /// </summary>
        static void ModifyConnectionAssignments(ModuleDefMD module, TypeDef obdDataReader,
            MethodDef connectMethod, FieldDef useWsField, FieldDef wsConnField)
        {
            if (connectMethod.Body == null)
            {
                Console.WriteLine("  警告: Connect 方法没有方法体");
                return;
            }

            // 查找 Connection 字段
            FieldDef connectionField = null;
            foreach (var field in obdDataReader.Fields)
            {
                if (field.Name == "Connection")
                {
                    connectionField = field;
                    break;
                }
            }

            // 如果在当前类找不到，可能在父类或者需要从赋值指令推断
            IField connectionFieldRef = null;
            var instructions = connectMethod.Body.Instructions;

            foreach (var instr in instructions)
            {
                if (instr.OpCode == OpCodes.Stfld && instr.Operand is IField field)
                {
                    if (field.Name == "Connection")
                    {
                        connectionFieldRef = field;
                        break;
                    }
                }
            }

            if (connectionFieldRef == null)
            {
                Console.WriteLine("  警告: 未找到 Connection 字段引用");
                return;
            }

            Console.WriteLine($"  找到 Connection 字段: {connectionFieldRef.FullName}");

            // 收集所有需要修改的赋值点
            var assignmentIndices = new List<int>();
            for (int i = 0; i < instructions.Count; i++)
            {
                var instr = instructions[i];
                if (instr.OpCode == OpCodes.Stfld && instr.Operand is IField field)
                {
                    if (field.Name == "Connection")
                    {
                        // 只修改非空赋值（排除 this.Connection = null）
                        if (i > 0 && instructions[i - 1].OpCode != OpCodes.Ldnull)
                        {
                            assignmentIndices.Add(i);
                        }
                    }
                }
            }

            Console.WriteLine($"  找到 {assignmentIndices.Count} 个需要修改的赋值点");

            // 从后往前修改，避免索引偏移问题
            int modifiedCount = 0;
            for (int idx = assignmentIndices.Count - 1; idx >= 0; idx--)
            {
                int stfldIndex = assignmentIndices[idx];

                // 找到这个赋值序列的起始点
                // 通常是: ldarg.0 (this) -> ... (创建连接对象) -> stfld Connection
                int startIndex = FindAssignmentStart(instructions, stfldIndex);

                if (startIndex < 0)
                {
                    Console.WriteLine($"    跳过 IL_{instructions[stfldIndex].Offset:X4}: 无法确定赋值起始点");
                    continue;
                }

                // 插入条件判断
                bool success = InsertConditionalAssignment(
                    module, connectMethod,
                    startIndex, stfldIndex,
                    useWsField, wsConnField, connectionFieldRef);

                if (success)
                {
                    modifiedCount++;
                    Console.WriteLine($"    修改 IL_{instructions[stfldIndex].Offset:X4}: 已添加 WebSocket 条件判断");
                }
            }

            Console.WriteLine($"  成功修改 {modifiedCount} 个赋值点");
        }

        /// <summary>
        /// 找到赋值序列的起始点（通常是 ldarg.0 加载 this）
        /// </summary>
        static int FindAssignmentStart(IList<Instruction> instructions, int stfldIndex)
        {
            // 从 stfld 向前查找对应的 ldarg.0 (加载 this)
            // 需要跟踪栈平衡
            int stackDepth = 2; // stfld 消耗 2 个值：对象引用和值

            for (int i = stfldIndex - 1; i >= 0; i--)
            {
                var instr = instructions[i];

                // 计算这条指令对栈的影响
                int push = 0, pop = 0;
                instr.CalculateStackUsage(out push, out pop);

                stackDepth -= push;
                stackDepth += pop;

                if (stackDepth <= 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在指定位置插入条件判断逻辑
        /// </summary>
        static bool InsertConditionalAssignment(ModuleDefMD module, MethodDef method,
            int startIndex, int stfldIndex,
            FieldDef useWsField, FieldDef wsConnField, IField connectionFieldRef)
        {
            var instructions = method.Body.Instructions;

            // 创建跳转标签
            var originalAssignStart = instructions[startIndex];
            var afterStfld = stfldIndex + 1 < instructions.Count ? instructions[stfldIndex + 1] : null;

            // 需要插入的 IL 代码:
            // ldsfld UseWebSocketConnection
            // brfalse originalAssignStart  // 如果 false，跳到原始赋值
            // ldsfld WebSocketConnection
            // brfalse originalAssignStart  // 如果 null，跳到原始赋值
            // ldarg.0
            // ldsfld WebSocketConnection
            // stfld Connection
            // br afterStfld               // 跳过原始赋值
            // originalAssignStart:        // 原始赋值代码

            var newInstructions = new List<Instruction>();

            // 检查 UseWebSocketConnection
            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(useWsField));
            newInstructions.Add(OpCodes.Brfalse.ToInstruction(originalAssignStart));

            // 检查 WebSocketConnection != null
            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsConnField));
            newInstructions.Add(OpCodes.Brfalse.ToInstruction(originalAssignStart));

            // 使用 WebSocket 连接
            // 关键修复：Mono 编译器在 MoveNext 开头就将 <>4__this（OBDDataReader）缓存到局部变量，
            // 所有 this.Connection 赋值都用 ldloc.s X（直接加载 OBDDataReader）而非 ldarg.0。
            // 原 patch 硬编码了 OpCodes.Ldarg_0（加载状态机自身），导致 stfld 写到错误对象。
            // 正确做法：直接复制 instructions[startIndex]（它已经是加载 OBDDataReader 的指令）。
            var loadOBDReaderInstr = instructions[startIndex];
            Console.WriteLine($"      [WebSocket路径] 复制 startIndex 指令: {loadOBDReaderInstr.OpCode.Name} {loadOBDReaderInstr.Operand}");
            newInstructions.Add(new Instruction(loadOBDReaderInstr.OpCode, loadOBDReaderInstr.Operand)); // 加载 OBDDataReader
            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsConnField));                              // 加载 WSProxy
            newInstructions.Add(OpCodes.Stfld.ToInstruction(connectionFieldRef));                        // 赋值到 OBDDataReader.Connection

            // 跳过原始赋值
            if (afterStfld != null)
            {
                newInstructions.Add(OpCodes.Br.ToInstruction(afterStfld));
            }
            else
            {
                newInstructions.Add(OpCodes.Ret.ToInstruction());
            }

            // 在 startIndex 处插入新指令
            for (int i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(startIndex, newInstructions[i]);
            }

            // 更新异常处理器和其他跳转目标
            method.Body.UpdateInstructionOffsets();

            return true;
        }

        /// <summary>
        /// 查找 IOBDConnection 接口类型
        /// </summary>
        static ITypeDefOrRef FindIOBDConnectionType(ModuleDefMD module, IField connectionField)
        {
            // 从字段类型获取
            if (connectionField.FieldSig?.Type != null)
            {
                return connectionField.FieldSig.Type.ToTypeDefOrRef();
            }

            // 在模块中查找 IOBDConnection 接口
            foreach (var type in module.GetTypes())
            {
                if (type.Name == "IOBDConnection" && type.IsInterface)
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// 查找模块中已存在的 mscorlib 程序集引用
        /// </summary>
        static AssemblyRef FindMscorlibReference(ModuleDefMD module)
        {
            // 首先查找 mscorlib
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "mscorlib")
                {
                    return asmRef;
                }
            }

            // 如果没找到 mscorlib，查找 netstandard
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "netstandard")
                {
                    return asmRef;
                }
            }

            // 如果还没找到，查找 System.Runtime
            foreach (var asmRef in module.GetAssemblyRefs())
            {
                if (asmRef.Name == "System.Runtime")
                {
                    return asmRef;
                }
            }

            return null;
        }

        /// <summary>
        /// 添加初始化方法：public static void InitializeWebSocket(object connection)
        /// 设置 WebSocketConnection 和 UseWebSocketConnection
        /// </summary>
        static void AddInitializeWebSocketMethod(ModuleDefMD module, TypeDef targetType,
            FieldDef useWsField, FieldDef wsConnField)
        {
            // 检查是否已存在
            if (targetType.Methods.Any(m => m.Name == "InitializeWebSocket"))
            {
                Console.WriteLine("  InitializeWebSocket 方法已存在，跳过");
                return;
            }

            var voidType = module.CorLibTypes.Void;
            var objType = module.CorLibTypes.Object;

            // 创建方法1: public static void InitializeWebSocket(object connection)
            var methodSig1 = MethodSig.CreateStatic(voidType, objType);
            var initMethod1 = new MethodDefUser(
                "InitializeWebSocket",
                methodSig1,
                MethodAttributes.Public | MethodAttributes.Static);

            initMethod1.Body = new CilBody();
            initMethod1.Body.InitLocals = true;

            // 方法体：
            // WebSocketConnection = connection;
            // UseWebSocketConnection = (connection != null);

            // ldarg.0 (connection 参数)
            initMethod1.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            // stsfld WebSocketConnection
            initMethod1.Body.Instructions.Add(OpCodes.Stsfld.ToInstruction(wsConnField));
            // ldarg.0
            initMethod1.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            // ldnull
            initMethod1.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            // cgt.un (比较是否不为 null)
            initMethod1.Body.Instructions.Add(OpCodes.Cgt_Un.ToInstruction());
            // stsfld UseWebSocketConnection
            initMethod1.Body.Instructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));
            // ret
            initMethod1.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            targetType.Methods.Add(initMethod1);
            Console.WriteLine("  添加方法: InitializeWebSocket(object connection)");

            // 创建静态构造函数(.cctor)自动初始化 WebSocket 服务器
            AddStaticConstructor(module, targetType, useWsField, wsConnField);

            // 创建方法2: public static void DisableWebSocket() - 禁用 WebSocket 连接
            var methodSig2 = MethodSig.CreateStatic(voidType);
            var disableMethod = new MethodDefUser(
                "DisableWebSocket",
                methodSig2,
                MethodAttributes.Public | MethodAttributes.Static);

            disableMethod.Body = new CilBody();

            // UseWebSocketConnection = false;
            disableMethod.Body.Instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
            disableMethod.Body.Instructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));
            // WebSocketConnection = null;
            disableMethod.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            disableMethod.Body.Instructions.Add(OpCodes.Stsfld.ToInstruction(wsConnField));
            disableMethod.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            targetType.Methods.Add(disableMethod);
            Console.WriteLine("  添加方法: DisableWebSocket()");

            // 创建方法3: public static bool IsWebSocketEnabled() - 检查是否启用
            var methodSig3 = MethodSig.CreateStatic(module.CorLibTypes.Boolean);
            var checkMethod = new MethodDefUser(
                "IsWebSocketEnabled",
                methodSig3,
                MethodAttributes.Public | MethodAttributes.Static);

            checkMethod.Body = new CilBody();

            // return UseWebSocketConnection && WebSocketConnection != null;
            var retFalse = OpCodes.Ldc_I4_0.ToInstruction();

            checkMethod.Body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(useWsField));
            checkMethod.Body.Instructions.Add(OpCodes.Brfalse_S.ToInstruction(retFalse));
            checkMethod.Body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(wsConnField));
            checkMethod.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            checkMethod.Body.Instructions.Add(OpCodes.Cgt_Un.ToInstruction());
            checkMethod.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            checkMethod.Body.Instructions.Add(retFalse);
            checkMethod.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            targetType.Methods.Add(checkMethod);
            Console.WriteLine("  添加方法: IsWebSocketEnabled()");
        }

        /// <summary>
        /// 添加静态构造函数，在类首次使用时自动初始化 WebSocket 服务器
        /// 使用反射加载 OBDCloud.WebSocket.dll 并调用 OBDCloudManager.Instance
        /// </summary>
        static void AddStaticConstructor(ModuleDefMD module, TypeDef targetType,
            FieldDef useWsField, FieldDef wsConnField)
        {
            // 检查是否已存在静态构造函数
            MethodDef existingCctor = null;
            foreach (var method in targetType.Methods)
            {
                if (method.IsStaticConstructor)
                {
                    existingCctor = method;
                    break;
                }
            }

            if (existingCctor != null)
            {
                // 在现有静态构造函数开头插入初始化代码
                Console.WriteLine("  修改现有静态构造函数，添加 WebSocket 自动初始化");
                InsertAutoInitCode(module, existingCctor, useWsField, wsConnField);
                return;
            }

            // 创建新的静态构造函数
            Console.WriteLine("  创建静态构造函数，添加 WebSocket 自动初始化");

            var voidType = module.CorLibTypes.Void;
            var cctorSig = MethodSig.CreateStatic(voidType);
            var cctor = new MethodDefUser(
                ".cctor",
                cctorSig,
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Private | MethodAttributes.Static |
                MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            cctor.Body = new CilBody();
            cctor.Body.InitLocals = true;

            // 生成自动初始化代码
            GenerateAutoInitCode(module, cctor, useWsField, wsConnField);

            targetType.Methods.Add(cctor);
        }

        /// <summary>
        /// 在现有静态构造函数开头插入初始化代码
        /// </summary>
        static void InsertAutoInitCode(ModuleDefMD module, MethodDef cctor,
            FieldDef useWsField, FieldDef wsConnField)
        {
            var body = cctor.Body;
            body.InitLocals = true;

            // 查找 mscorlib 程序集引用（Xamarin/Mono 使用的）
            var mscorlibRef = FindMscorlibReference(module);
            if (mscorlibRef == null)
            {
                Console.WriteLine("  警告: 未找到 mscorlib 引用，跳过静态构造函数注入");
                return;
            }

            // 需要的类型引用
            var assemblyTypeRef = new TypeRefUser(module, "System.Reflection", "Assembly", mscorlibRef);
            var typeTypeRef = new TypeRefUser(module, "System", "Type", mscorlibRef);
            var propertyInfoTypeRef = new TypeRefUser(module, "System.Reflection", "PropertyInfo", mscorlibRef);
            var exceptionTypeRef = new TypeRefUser(module, "System", "Exception", mscorlibRef);

            // 添加局部变量（在现有变量基础上）
            int varOffset = body.Variables.Count;
            // assembly
            body.Variables.Add(new Local(assemblyTypeRef.ToTypeSig()));
            // type
            body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            // propertyInfo (Instance)
            body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));
            // manager object
            body.Variables.Add(new Local(module.CorLibTypes.Object));
            // propertyInfo (BluetoothConnection)
            body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));
            // exception
            body.Variables.Add(new Local(exceptionTypeRef.ToTypeSig()));

            // 方法引用
            var assemblyLoadMethod = new MemberRefUser(module, "Load",
                MethodSig.CreateStatic(assemblyTypeRef.ToTypeSig(), module.CorLibTypes.String),
                assemblyTypeRef);

            var getTypeMethod = new MemberRefUser(module, "GetType",
                MethodSig.CreateInstance(typeTypeRef.ToTypeSig(), module.CorLibTypes.String),
                assemblyTypeRef);

            var getPropertyMethod = new MemberRefUser(module, "GetProperty",
                MethodSig.CreateInstance(propertyInfoTypeRef.ToTypeSig(), module.CorLibTypes.String),
                typeTypeRef);

            var getValueMethod = new MemberRefUser(module, "GetValue",
                MethodSig.CreateInstance(module.CorLibTypes.Object, module.CorLibTypes.Object),
                propertyInfoTypeRef);

            // 保存原始第一条指令作为跳转目标
            var originalFirst = body.Instructions[0];

            // 生成新指令列表
            var newInstructions = new List<Instruction>();

            // try 块开始
            var tryStart = OpCodes.Ldstr.ToInstruction("OBDCloud.WebSocket");
            var catchStart = OpCodes.Pop.ToInstruction(); // 弹出异常对象，不存储

            // === try 块 ===
            newInstructions.Add(tryStart);
            newInstructions.Add(OpCodes.Call.ToInstruction(assemblyLoadMethod));
            newInstructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[varOffset]));

            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset]));
            newInstructions.Add(OpCodes.Ldstr.ToInstruction("OBDCloud.WebSocket.OBDCloudManager"));
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            newInstructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[varOffset + 1]));

            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset + 1]));
            newInstructions.Add(OpCodes.Ldstr.ToInstruction("Instance"));
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            newInstructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[varOffset + 2]));

            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset + 2]));
            newInstructions.Add(OpCodes.Ldnull.ToInstruction());
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(getValueMethod));
            newInstructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[varOffset + 3]));

            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset + 1]));
            newInstructions.Add(OpCodes.Ldstr.ToInstruction("BluetoothConnection"));
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            newInstructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[varOffset + 4]));

            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset + 4]));
            newInstructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[varOffset + 3]));
            newInstructions.Add(OpCodes.Callvirt.ToInstruction(getValueMethod));
            newInstructions.Add(OpCodes.Stsfld.ToInstruction(wsConnField));

            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsConnField));
            newInstructions.Add(OpCodes.Ldnull.ToInstruction());
            newInstructions.Add(OpCodes.Cgt_Un.ToInstruction());
            newInstructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));

            // 不再调用 Debug.WriteLine，因为 Xamarin/Android 上不可用

            var leaveInTry = OpCodes.Leave.ToInstruction(originalFirst);
            newInstructions.Add(leaveInTry);

            // === catch 块 ===
            newInstructions.Add(catchStart);

            // 异常已被弹出，只设置 UseWebSocketConnection = false

            newInstructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
            newInstructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));

            var leaveInCatch = OpCodes.Leave.ToInstruction(originalFirst);
            newInstructions.Add(leaveInCatch);

            // 在开头插入所有新指令
            for (int i = newInstructions.Count - 1; i >= 0; i--)
            {
                body.Instructions.Insert(0, newInstructions[i]);
            }

            // 添加异常处理器
            var exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = tryStart,
                TryEnd = catchStart,
                HandlerStart = catchStart,
                HandlerEnd = originalFirst,
                CatchType = exceptionTypeRef
            };
            body.ExceptionHandlers.Insert(0, exceptionHandler);

            Console.WriteLine("  在现有静态构造函数中插入了 WebSocket 初始化代码");
        }

        /// <summary>
        /// 生成自动初始化代码到方法体
        /// </summary>
        static void GenerateAutoInitCode(ModuleDefMD module, MethodDef method,
            FieldDef useWsField, FieldDef wsConnField)
        {
            var body = method.Body;
            body.InitLocals = true;

            // 查找 mscorlib 程序集引用（Xamarin/Mono 使用的）
            var mscorlibRef = FindMscorlibReference(module);
            if (mscorlibRef == null)
            {
                Console.WriteLine("  警告: 未找到 mscorlib 引用，跳过静态构造函数生成");
                body.Instructions.Add(OpCodes.Ret.ToInstruction());
                return;
            }

            // 需要的类型引用
            var assemblyTypeRef = new TypeRefUser(module, "System.Reflection", "Assembly", mscorlibRef);
            var typeTypeRef = new TypeRefUser(module, "System", "Type", mscorlibRef);
            var propertyInfoTypeRef = new TypeRefUser(module, "System.Reflection", "PropertyInfo", mscorlibRef);
            var exceptionTypeRef = new TypeRefUser(module, "System", "Exception", mscorlibRef);

            // 添加局部变量
            // 0: Assembly
            body.Variables.Add(new Local(assemblyTypeRef.ToTypeSig()));
            // 1: Type
            body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            // 2: PropertyInfo (Instance)
            body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));
            // 3: object (manager)
            body.Variables.Add(new Local(module.CorLibTypes.Object));
            // 4: PropertyInfo (BluetoothConnection)
            body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));
            // 5: Exception
            body.Variables.Add(new Local(exceptionTypeRef.ToTypeSig()));

            // 方法引用
            var assemblyLoadMethod = new MemberRefUser(module, "Load",
                MethodSig.CreateStatic(assemblyTypeRef.ToTypeSig(), module.CorLibTypes.String),
                assemblyTypeRef);

            var getTypeMethod = new MemberRefUser(module, "GetType",
                MethodSig.CreateInstance(typeTypeRef.ToTypeSig(), module.CorLibTypes.String),
                assemblyTypeRef);

            var getPropertyMethod = new MemberRefUser(module, "GetProperty",
                MethodSig.CreateInstance(propertyInfoTypeRef.ToTypeSig(), module.CorLibTypes.String),
                typeTypeRef);

            var getValueMethod = new MemberRefUser(module, "GetValue",
                MethodSig.CreateInstance(module.CorLibTypes.Object, module.CorLibTypes.Object),
                propertyInfoTypeRef);

            var instructions = body.Instructions;

            // === try 块开始 ===
            var tryStart = OpCodes.Ldstr.ToInstruction("OBDCloud.WebSocket");
            var catchStart = OpCodes.Pop.ToInstruction(); // 弹出异常对象，不存储
            var endHandler = OpCodes.Ret.ToInstruction();

            // var assembly = Assembly.Load("OBDCloud.WebSocket");
            instructions.Add(tryStart);
            instructions.Add(OpCodes.Call.ToInstruction(assemblyLoadMethod));
            instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var managerType = assembly.GetType("OBDCloud.WebSocket.OBDCloudManager");
            instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            instructions.Add(OpCodes.Ldstr.ToInstruction("OBDCloud.WebSocket.OBDCloudManager"));
            instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // var instanceProp = managerType.GetProperty("Instance");
            instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            instructions.Add(OpCodes.Ldstr.ToInstruction("Instance"));
            instructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            instructions.Add(OpCodes.Stloc_2.ToInstruction());

            // var manager = instanceProp.GetValue(null);
            instructions.Add(OpCodes.Ldloc_2.ToInstruction());
            instructions.Add(OpCodes.Ldnull.ToInstruction());
            instructions.Add(OpCodes.Callvirt.ToInstruction(getValueMethod));
            instructions.Add(OpCodes.Stloc_3.ToInstruction());

            // var btConnProp = managerType.GetProperty("BluetoothConnection");
            instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            instructions.Add(OpCodes.Ldstr.ToInstruction("BluetoothConnection"));
            instructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            instructions.Add(OpCodes.Stloc_S.ToInstruction(body.Variables[4]));

            // WebSocketConnection = btConnProp.GetValue(manager);
            instructions.Add(OpCodes.Ldloc_S.ToInstruction(body.Variables[4]));
            instructions.Add(OpCodes.Ldloc_3.ToInstruction());
            instructions.Add(OpCodes.Callvirt.ToInstruction(getValueMethod));
            instructions.Add(OpCodes.Stsfld.ToInstruction(wsConnField));

            // UseWebSocketConnection = (WebSocketConnection != null);
            instructions.Add(OpCodes.Ldsfld.ToInstruction(wsConnField));
            instructions.Add(OpCodes.Ldnull.ToInstruction());
            instructions.Add(OpCodes.Cgt_Un.ToInstruction());
            instructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));

            // 不使用 Debug.WriteLine，因为在 Xamarin/Android 上不可用

            // leave -> end
            var leaveInTry = OpCodes.Leave.ToInstruction(endHandler);
            instructions.Add(leaveInTry);

            // === catch 块开始 ===
            instructions.Add(catchStart);

            // 异常已被弹出，设置 UseWebSocketConnection = false

            // UseWebSocketConnection = false;
            instructions.Add(OpCodes.Ldc_I4_0.ToInstruction());
            instructions.Add(OpCodes.Stsfld.ToInstruction(useWsField));

            // leave -> end
            var leaveInCatch = OpCodes.Leave.ToInstruction(endHandler);
            instructions.Add(leaveInCatch);

            // === end ===
            instructions.Add(endHandler);

            // 添加异常处理器
            var exceptionHandler = new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = tryStart,
                TryEnd = catchStart,
                HandlerStart = catchStart,
                HandlerEnd = endHandler,
                CatchType = exceptionTypeRef
            };
            body.ExceptionHandlers.Add(exceptionHandler);

            Console.WriteLine("  生成了 WebSocket 自动初始化 IL 代码（含异常处理）");
        }

        /// <summary>
        /// 生成自动初始化的 IL 指令（简化版，不使用 try-catch）
        /// 等效 C# 代码:
        /// var assembly = Assembly.Load("OBDCloud.WebSocket");
        /// if (assembly != null) {
        ///     var managerType = assembly.GetType("OBDCloud.WebSocket.OBDCloudManager");
        ///     if (managerType != null) {
        ///         var instanceProp = managerType.GetProperty("Instance");
        ///         if (instanceProp != null) {
        ///             var manager = instanceProp.GetValue(null);
        ///             if (manager != null) {
        ///                 var btConnProp = managerType.GetProperty("BluetoothConnection");
        ///                 if (btConnProp != null) {
        ///                     WebSocketConnection = btConnProp.GetValue(manager);
        ///                     UseWebSocketConnection = (WebSocketConnection != null);
        ///                 }
        ///             }
        ///         }
        ///     }
        /// }
        /// </summary>
        static void GenerateAutoInitInstructions(ModuleDefMD module, List<Instruction> instructions,
            FieldDef useWsField, FieldDef wsConnField)
        {
            // 简化版：只生成基本的初始化代码，不添加 try-catch
            // 这用于插入到现有静态构造函数
            Console.WriteLine("  生成简化版 WebSocket 初始化代码");
        }
    }
}
