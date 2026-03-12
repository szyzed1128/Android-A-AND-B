using System;
using System.Linq;
using dnlib.DotNet;

namespace InterfacePatcher
{
    /// <summary>
    /// 在编译后给 WebSocketBluetoothConnection 添加 IBluetoothConnection 接口实现
    ///
    /// 这是必要的，因为：
    /// 1. 我们无法在编译时直接引用 CarScannerXamarinForms.dll（mscorlib 版本冲突）
    /// 2. DllPatcher 修补的 IL 代码会尝试将 WebSocketConnection 转换为 IOBDConnection
    /// 3. 如果 WebSocketBluetoothConnection 没有实现该接口，转换会失败
    ///
    /// 此工具在运行时添加接口实现，绕过编译时的类型冲突。
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== WebSocket 接口修补工具 ===\n");

            if (args.Length < 3)
            {
                Console.WriteLine("用法: InterfacePatcher <OBDCloud.WebSocket.dll> <CarScannerXamarinForms.dll> <输出路径>");
                Console.WriteLine();
                Console.WriteLine("示例:");
                Console.WriteLine("  InterfacePatcher OBDCloud.WebSocket.dll CarScannerXamarinForms.dll OBDCloud.WebSocket.patched.dll");
                return;
            }

            var webSocketDll = args[0];
            var carScannerDll = args[1];
            var outputDll = args[2];

            try
            {
                ApplyPatch(webSocketDll, carScannerDll, outputDll);
                Console.WriteLine("\n修补完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n错误: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void ApplyPatch(string webSocketDll, string carScannerDll, string outputDll)
        {
            Console.WriteLine($"加载 WebSocket 模块: {webSocketDll}");
            var wsModule = ModuleDefMD.Load(webSocketDll);

            Console.WriteLine($"加载 CarScanner 模块: {carScannerDll}");
            var csModule = ModuleDefMD.Load(carScannerDll);

            // 查找接口
            Console.WriteLine("\n步骤1: 查找 IBluetoothConnection 接口...");
            TypeDef iBluetoothConnection = null;
            TypeDef iOBDConnection = null;

            foreach (var type in csModule.GetTypes())
            {
                if (type.Name == "IBluetoothConnection" && type.IsInterface)
                {
                    iBluetoothConnection = type;
                    Console.WriteLine($"  找到 IBluetoothConnection: {type.FullName}");
                }
                if (type.Name == "IOBDConnection" && type.IsInterface)
                {
                    iOBDConnection = type;
                    Console.WriteLine($"  找到 IOBDConnection: {type.FullName}");
                }
            }

            if (iBluetoothConnection == null || iOBDConnection == null)
            {
                throw new Exception("未找到必要的接口定义");
            }

            // 查找 WebSocketBluetoothConnection 类
            Console.WriteLine("\n步骤2: 查找 WebSocketBluetoothConnection 类...");
            TypeDef wsBluetoothConn = null;
            foreach (var type in wsModule.GetTypes())
            {
                if (type.Name == "WebSocketBluetoothConnection")
                {
                    wsBluetoothConn = type;
                    Console.WriteLine($"  找到: {type.FullName}");
                    break;
                }
            }

            if (wsBluetoothConn == null)
            {
                throw new Exception("未找到 WebSocketBluetoothConnection 类");
            }

            // 创建程序集引用
            Console.WriteLine("\n步骤3: 创建程序集引用...");
            var csAsmRef = new AssemblyRefUser(csModule.Assembly);
            Console.WriteLine($"  引用: {csAsmRef.FullName}");

            // 创建类型引用
            var iBluetoothConnRef = new TypeRefUser(wsModule,
                iBluetoothConnection.Namespace,
                iBluetoothConnection.Name,
                csAsmRef);

            var iOBDConnectionRef = new TypeRefUser(wsModule,
                iOBDConnection.Namespace,
                iOBDConnection.Name,
                csAsmRef);

            // 添加接口实现
            Console.WriteLine("\n步骤4: 添加接口实现...");

            // 检查是否已经实现
            bool hasIBluetoothConnection = wsBluetoothConn.Interfaces.Any(i => i.Interface.Name == "IBluetoothConnection");
            if (!hasIBluetoothConnection)
            {
                wsBluetoothConn.Interfaces.Add(new InterfaceImplUser(iBluetoothConnRef));
                Console.WriteLine("  添加 IBluetoothConnection 接口");
            }
            else
            {
                Console.WriteLine("  IBluetoothConnection 接口已存在，跳过");
            }

            // IOBDConnection 是 IBluetoothConnection 的父接口，也需要添加
            bool hasIOBDConnection = wsBluetoothConn.Interfaces.Any(i => i.Interface.Name == "IOBDConnection");
            if (!hasIOBDConnection)
            {
                wsBluetoothConn.Interfaces.Add(new InterfaceImplUser(iOBDConnectionRef));
                Console.WriteLine("  添加 IOBDConnection 接口");
            }
            else
            {
                Console.WriteLine("  IOBDConnection 接口已存在，跳过");
            }

            // 验证方法签名
            Console.WriteLine("\n步骤5: 验证方法签名...");
            VerifyMethodSignatures(wsBluetoothConn, iOBDConnection, iBluetoothConnection);

            // 保存
            Console.WriteLine($"\n步骤6: 保存到 {outputDll}");
            wsModule.Write(outputDll);
            Console.WriteLine("  保存成功");
        }

        static void VerifyMethodSignatures(TypeDef implementation, TypeDef iOBD, TypeDef iBluetooth)
        {
            // 检查必要的方法是否存在
            string[] requiredMethods = { "ReadBytesAsync", "WriteBytesAsync", "FlushAsync", "ConnectAsync", "Disconect" };
            string[] requiredProps = { "KeepAlive", "NoDelay", "Connected", "NeedFlush", "ConnectionTimeoutSeconds" };

            foreach (var methodName in requiredMethods)
            {
                var method = implementation.Methods.FirstOrDefault(m => m.Name == methodName);
                if (method != null)
                {
                    Console.WriteLine($"  ✓ 方法 {methodName} 已实现");
                }
                else
                {
                    Console.WriteLine($"  ✗ 警告: 方法 {methodName} 未找到");
                }
            }

            foreach (var propName in requiredProps)
            {
                var getter = implementation.Methods.FirstOrDefault(m => m.Name == $"get_{propName}");
                if (getter != null)
                {
                    Console.WriteLine($"  ✓ 属性 {propName} getter 已实现");
                }
                else
                {
                    Console.WriteLine($"  ✗ 警告: 属性 {propName} getter 未找到");
                }
            }
        }
    }
}
