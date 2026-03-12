using System;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace ProxyGenerator
{
    /// <summary>
    /// 代理生成器 - 在 CarScannerXamarinForms.dll 中生成 IOBDConnectionProxy 类
    ///
    /// 这个代理类实现 IOBDConnection 接口，并通过反射调用 WebSocketBluetoothConnection 的方法。
    /// 关键是处理 ReadBytesAsync 的返回类型差异：
    /// - IOBDConnection.ReadBytesAsync() 返回 ValueTask<byte[]>
    /// - WebSocketBluetoothConnection.ReadBytesAsync() 返回 Task<byte[]>
    ///
    /// 解决方案：返回 new ValueTask<byte[]>(task)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== IOBDConnectionProxy 生成器 ===\n");

            if (args.Length < 2)
            {
                Console.WriteLine("用法: ProxyGenerator <原始DLL路径> <输出DLL路径>");
                return;
            }

            var inputDll = args[0];
            var outputDll = args[1];

            if (!File.Exists(inputDll))
            {
                Console.WriteLine($"错误: 找不到输入文件 {inputDll}");
                return;
            }

            try
            {
                GenerateProxy(inputDll, outputDll);
                Console.WriteLine("\n生成完成!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n错误: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void GenerateProxy(string inputDll, string outputDll)
        {
            Console.WriteLine($"加载模块: {inputDll}");
            var module = ModuleDefMD.Load(inputDll);

            // 查找 IOBDConnection 接口
            Console.WriteLine("\n步骤1: 查找 IOBDConnection 接口...");
            TypeDef iobdInterface = null;
            foreach (var type in module.GetTypes())
            {
                if (type.Name == "IOBDConnection" && type.IsInterface)
                {
                    iobdInterface = type;
                    break;
                }
            }

            if (iobdInterface == null)
            {
                throw new Exception("未找到 IOBDConnection 接口");
            }
            Console.WriteLine($"  找到: {iobdInterface.FullName}");

            // 打印接口方法（调试用）
            Console.WriteLine("\n接口方法:");
            foreach (var m in iobdInterface.Methods)
            {
                Console.WriteLine($"  {m.ReturnType} {m.Name}({string.Join(", ", m.Parameters.Skip(1).Select(p => $"{p.Type}"))})");
            }

            // 检查是否已存在代理类
            Console.WriteLine("\n步骤2: 检查/创建代理类...");
            TypeDef proxyClass = null;
            foreach (var type in module.GetTypes())
            {
                if (type.Name == "WebSocketIOBDConnectionProxy")
                {
                    proxyClass = type;
                    Console.WriteLine("  代理类已存在，将更新");
                    break;
                }
            }

            if (proxyClass == null)
            {
                // 创建新的代理类
                proxyClass = CreateProxyClass(module, iobdInterface);
                module.Types.Add(proxyClass);
                Console.WriteLine("  创建了新的代理类");
            }

            // 保存修改后的 DLL
            Console.WriteLine($"\n步骤3: 保存到 {outputDll}");
            var options = new ModuleWriterOptions(module)
            {
                WritePdb = false
            };
            module.Write(outputDll, options);
            Console.WriteLine("  保存成功");
        }

        static TypeDef CreateProxyClass(ModuleDefMD module, TypeDef iobdInterface)
        {
            // 创建 WebSocketIOBDConnectionProxy 类
            var proxyClass = new TypeDefUser(
                "CarScannerXamarinForms.OBD2",
                "WebSocketIOBDConnectionProxy",
                module.CorLibTypes.Object.TypeDefOrRef);

            proxyClass.Attributes = TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed;

            // 实现 IOBDConnection 接口
            proxyClass.Interfaces.Add(new InterfaceImplUser(iobdInterface));

            // 添加 _target 字段（object 类型，存储 WebSocketBluetoothConnection）
            var targetField = new FieldDefUser(
                "_target",
                new FieldSig(module.CorLibTypes.Object),
                FieldAttributes.Private);
            proxyClass.Fields.Add(targetField);

            // 添加构造函数
            AddConstructor(module, proxyClass, targetField);

            // 实现接口属性和方法
            foreach (var method in iobdInterface.Methods)
            {
                if (method.IsGetter)
                {
                    // 属性 getter
                    var propName = method.Name.Substring(4); // 去掉 "get_" 前缀
                    AddPropertyGetter(module, proxyClass, targetField, method, propName);
                }
                else if (method.IsSetter)
                {
                    // 属性 setter
                    var propName = method.Name.Substring(4); // 去掉 "set_" 前缀
                    AddPropertySetter(module, proxyClass, targetField, method, propName);
                }
                else
                {
                    // 普通方法
                    AddMethod(module, proxyClass, targetField, method);
                }
            }

            return proxyClass;
        }

        static void AddConstructor(ModuleDefMD module, TypeDef proxyClass, FieldDef targetField)
        {
            // public WebSocketIOBDConnectionProxy(object target)
            var ctor = new MethodDefUser(
                ".ctor",
                MethodSig.CreateInstance(module.CorLibTypes.Void, module.CorLibTypes.Object),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            ctor.Body = new CilBody();

            // 调用基类构造函数
            var objectCtor = new MemberRefUser(module, ".ctor",
                MethodSig.CreateInstance(module.CorLibTypes.Void),
                module.CorLibTypes.Object.TypeDefOrRef);
            ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(objectCtor));

            // this._target = target;
            ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
            ctor.Body.Instructions.Add(OpCodes.Stfld.ToInstruction(targetField));

            ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            proxyClass.Methods.Add(ctor);
        }

        static void AddPropertyGetter(ModuleDefMD module, TypeDef proxyClass, FieldDef targetField,
            MethodDef interfaceMethod, string propName)
        {
            // 创建 getter 方法
            var getter = new MethodDefUser(
                "get_" + propName,
                MethodSig.CreateInstance(interfaceMethod.ReturnType),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual |
                MethodAttributes.NewSlot | MethodAttributes.SpecialName);

            getter.Body = new CilBody();
            getter.Body.InitLocals = true;

            // 使用反射获取属性值
            // return (bool)_target.GetType().GetProperty("KeepAlive").GetValue(_target);

            var getTypeMethod = FindMethod(module, "System.Object", "GetType");
            var getPropertyMethod = FindMethod(module, "System.Type", "GetProperty", "System.String");
            var getValueMethod = FindMethod(module, "System.Reflection.PropertyInfo", "GetValue", "System.Object");

            // 添加局部变量
            var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
            var propertyInfoTypeRef = new TypeRefUser(module, "System.Reflection", "PropertyInfo", module.CorLibTypes.AssemblyRef);
            getter.Body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            getter.Body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));
            getter.Body.Variables.Add(new Local(module.CorLibTypes.Object));

            // var type = _target.GetType();
            getter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            getter.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            getter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            getter.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var prop = type.GetProperty("PropName");
            getter.Body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            getter.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(propName));
            getter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            getter.Body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // var value = prop.GetValue(_target);
            getter.Body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            getter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            getter.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            getter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getValueMethod));
            getter.Body.Instructions.Add(OpCodes.Stloc_2.ToInstruction());

            // return (ReturnType)value;
            getter.Body.Instructions.Add(OpCodes.Ldloc_2.ToInstruction());
            if (interfaceMethod.ReturnType.IsPrimitive)
            {
                getter.Body.Instructions.Add(OpCodes.Unbox_Any.ToInstruction(interfaceMethod.ReturnType.ToTypeDefOrRef()));
            }
            getter.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            proxyClass.Methods.Add(getter);
        }

        static void AddPropertySetter(ModuleDefMD module, TypeDef proxyClass, FieldDef targetField,
            MethodDef interfaceMethod, string propName)
        {
            // 创建 setter 方法
            var paramType = interfaceMethod.Parameters[1].Type; // 第一个参数是 this
            var setter = new MethodDefUser(
                "set_" + propName,
                MethodSig.CreateInstance(module.CorLibTypes.Void, paramType),
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual |
                MethodAttributes.NewSlot | MethodAttributes.SpecialName);

            setter.Body = new CilBody();
            setter.Body.InitLocals = true;

            // 使用反射设置属性值
            var getTypeMethod = FindMethod(module, "System.Object", "GetType");
            var getPropertyMethod = FindMethod(module, "System.Type", "GetProperty", "System.String");
            var setValueMethod = FindMethod(module, "System.Reflection.PropertyInfo", "SetValue", "System.Object", "System.Object");

            var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
            var propertyInfoTypeRef = new TypeRefUser(module, "System.Reflection", "PropertyInfo", module.CorLibTypes.AssemblyRef);
            setter.Body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            setter.Body.Variables.Add(new Local(propertyInfoTypeRef.ToTypeSig()));

            // var type = _target.GetType();
            setter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            setter.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            setter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            setter.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var prop = type.GetProperty("PropName");
            setter.Body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            setter.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(propName));
            setter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getPropertyMethod));
            setter.Body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // prop.SetValue(_target, value);
            setter.Body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            setter.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            setter.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            setter.Body.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
            if (paramType.IsPrimitive)
            {
                setter.Body.Instructions.Add(OpCodes.Box.ToInstruction(paramType.ToTypeDefOrRef()));
            }
            setter.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(setValueMethod));

            setter.Body.Instructions.Add(OpCodes.Ret.ToInstruction());

            proxyClass.Methods.Add(setter);
        }

        static void AddMethod(ModuleDefMD module, TypeDef proxyClass, FieldDef targetField,
            MethodDef interfaceMethod)
        {
            // 创建方法
            var methodSig = new MethodSig(CallingConvention.HasThis, 0, interfaceMethod.ReturnType,
                interfaceMethod.Parameters.Skip(1).Select(p => p.Type).ToArray());

            var method = new MethodDefUser(
                interfaceMethod.Name,
                methodSig,
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot);

            method.Body = new CilBody();
            method.Body.InitLocals = true;

            // 根据方法名生成不同的实现
            if (interfaceMethod.Name == "ReadBytesAsync")
            {
                GenerateReadBytesAsyncMethod(module, method, targetField, interfaceMethod);
            }
            else if (interfaceMethod.Name == "WriteBytesAsync" ||
                     interfaceMethod.Name == "FlushAsync" ||
                     interfaceMethod.Name == "ConnectAsync")
            {
                GenerateAsyncMethod(module, method, targetField, interfaceMethod);
            }
            else if (interfaceMethod.Name == "Disconect")
            {
                GenerateVoidMethod(module, method, targetField, interfaceMethod);
            }

            proxyClass.Methods.Add(method);
        }

        static void GenerateReadBytesAsyncMethod(ModuleDefMD module, MethodDef method,
            FieldDef targetField, MethodDef interfaceMethod)
        {
            // 特殊处理：将 Task<byte[]> 转换为 ValueTask<byte[]>
            // return new ValueTask<byte[]>((Task<byte[]>)InvokeMethod("ReadBytesAsync"));

            var getTypeMethod = FindMethod(module, "System.Object", "GetType");
            var getMethodMethod = FindMethod(module, "System.Type", "GetMethod", "System.String");
            var invokeMethod = FindMethod(module, "System.Reflection.MethodInfo", "Invoke", "System.Object", "System.Object[]");

            // 添加局部变量
            var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
            var methodInfoTypeRef = new TypeRefUser(module, "System.Reflection", "MethodInfo", module.CorLibTypes.AssemblyRef);
            method.Body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            method.Body.Variables.Add(new Local(methodInfoTypeRef.ToTypeSig()));
            method.Body.Variables.Add(new Local(module.CorLibTypes.Object));

            // var type = _target.GetType();
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var m = type.GetMethod("ReadBytesAsync");
            method.Body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction("ReadBytesAsync"));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getMethodMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // var result = m.Invoke(_target, null);
            method.Body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(invokeMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_2.ToInstruction());

            // 关键：将 Task<byte[]> 转换为 ValueTask<byte[]>
            // 需要找到 ValueTask<byte[]> 的构造函数并调用
            // return new ValueTask<byte[]>((Task<byte[]>)result);

            // 由于 ValueTask<T> 在 mscorlib 中定义，我们需要找到正确的类型引用
            // 暂时直接返回 object，让调用者处理转换
            // TODO: 完善 ValueTask 的创建

            method.Body.Instructions.Add(OpCodes.Ldloc_2.ToInstruction());
            // 这里需要创建 ValueTask<byte[]>，但由于类型复杂性，先简化处理
            method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
        }

        static void GenerateAsyncMethod(ModuleDefMD module, MethodDef method,
            FieldDef targetField, MethodDef interfaceMethod)
        {
            // 通过反射调用目标方法
            var getTypeMethod = FindMethod(module, "System.Object", "GetType");
            var getMethodMethod = FindMethod(module, "System.Type", "GetMethod", "System.String");
            var invokeMethod = FindMethod(module, "System.Reflection.MethodInfo", "Invoke", "System.Object", "System.Object[]");

            var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
            var methodInfoTypeRef = new TypeRefUser(module, "System.Reflection", "MethodInfo", module.CorLibTypes.AssemblyRef);
            method.Body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            method.Body.Variables.Add(new Local(methodInfoTypeRef.ToTypeSig()));
            method.Body.Variables.Add(new Local(new SZArraySig(module.CorLibTypes.Object)));

            // var type = _target.GetType();
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var m = type.GetMethod("MethodName");
            method.Body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(interfaceMethod.Name));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getMethodMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // 创建参数数组
            var paramCount = interfaceMethod.Parameters.Count - 1; // 减去 this
            method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(paramCount));
            method.Body.Instructions.Add(OpCodes.Newarr.ToInstruction(module.CorLibTypes.Object.TypeDefOrRef));
            method.Body.Instructions.Add(OpCodes.Stloc_2.ToInstruction());

            // 填充参数数组
            for (int i = 0; i < paramCount; i++)
            {
                method.Body.Instructions.Add(OpCodes.Ldloc_2.ToInstruction());
                method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(i));
                method.Body.Instructions.Add(OpCodes.Ldarg.ToInstruction(method.Parameters[i + 1]));
                var paramType = method.Parameters[i + 1].Type;
                if (paramType.IsPrimitive || paramType.IsValueType)
                {
                    method.Body.Instructions.Add(OpCodes.Box.ToInstruction(paramType.ToTypeDefOrRef()));
                }
                method.Body.Instructions.Add(OpCodes.Stelem_Ref.ToInstruction());
            }

            // var result = m.Invoke(_target, args);
            method.Body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Ldloc_2.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(invokeMethod));

            // 返回结果（如果有）
            if (!interfaceMethod.ReturnType.FullName.Contains("Void"))
            {
                // 结果已在栈上
            }
            else
            {
                method.Body.Instructions.Add(OpCodes.Pop.ToInstruction());
            }

            method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
        }

        static void GenerateVoidMethod(ModuleDefMD module, MethodDef method,
            FieldDef targetField, MethodDef interfaceMethod)
        {
            // 通过反射调用目标方法
            var getTypeMethod = FindMethod(module, "System.Object", "GetType");
            var getMethodMethod = FindMethod(module, "System.Type", "GetMethod", "System.String");
            var invokeMethod = FindMethod(module, "System.Reflection.MethodInfo", "Invoke", "System.Object", "System.Object[]");

            var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
            var methodInfoTypeRef = new TypeRefUser(module, "System.Reflection", "MethodInfo", module.CorLibTypes.AssemblyRef);
            method.Body.Variables.Add(new Local(typeTypeRef.ToTypeSig()));
            method.Body.Variables.Add(new Local(methodInfoTypeRef.ToTypeSig()));

            // var type = _target.GetType();
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getTypeMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_0.ToInstruction());

            // var m = type.GetMethod("Disconect");
            method.Body.Instructions.Add(OpCodes.Ldloc_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(interfaceMethod.Name));
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(getMethodMethod));
            method.Body.Instructions.Add(OpCodes.Stloc_1.ToInstruction());

            // m.Invoke(_target, null);
            method.Body.Instructions.Add(OpCodes.Ldloc_1.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Ldfld.ToInstruction(targetField));
            method.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
            method.Body.Instructions.Add(OpCodes.Callvirt.ToInstruction(invokeMethod));
            method.Body.Instructions.Add(OpCodes.Pop.ToInstruction());

            method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
        }

        static MemberRefUser FindMethod(ModuleDefMD module, string typeName, string methodName, params string[] paramTypeNames)
        {
            // 查找指定类型的方法引用
            var parts = typeName.Split('.');
            var ns = string.Join(".", parts.Take(parts.Length - 1));
            var name = parts.Last();

            var typeRef = new TypeRefUser(module, ns, name, module.CorLibTypes.AssemblyRef);

            // 创建方法签名
            var paramTypes = paramTypeNames.Select(p => {
                var typeParts = p.Split('.');
                var typeNs = string.Join(".", typeParts.Take(typeParts.Length - 1));
                var typN = typeParts.Last();
                if (typeNs == "System" && typN == "String")
                    return module.CorLibTypes.String;
                if (typeNs == "System" && typN == "Object")
                    return module.CorLibTypes.Object;
                if (typeNs == "System" && typN == "Object[]")
                    return new SZArraySig(module.CorLibTypes.Object) as TypeSig;
                var tRef = new TypeRefUser(module, typeNs, typN, module.CorLibTypes.AssemblyRef);
                return tRef.ToTypeSig();
            }).ToArray();

            TypeSig returnType = module.CorLibTypes.Object;
            if (methodName == "GetType")
            {
                var typeTypeRef = new TypeRefUser(module, "System", "Type", module.CorLibTypes.AssemblyRef);
                returnType = typeTypeRef.ToTypeSig();
            }
            else if (methodName == "GetProperty")
            {
                var propInfoTypeRef = new TypeRefUser(module, "System.Reflection", "PropertyInfo", module.CorLibTypes.AssemblyRef);
                returnType = propInfoTypeRef.ToTypeSig();
            }
            else if (methodName == "GetMethod")
            {
                var methodInfoTypeRef = new TypeRefUser(module, "System.Reflection", "MethodInfo", module.CorLibTypes.AssemblyRef);
                returnType = methodInfoTypeRef.ToTypeSig();
            }
            else if (methodName == "SetValue")
            {
                returnType = module.CorLibTypes.Void;
            }
            else if (methodName == "Invoke")
            {
                returnType = module.CorLibTypes.Object;
            }

            MethodSig sig;
            if (paramTypes.Length == 0)
            {
                sig = MethodSig.CreateInstance(returnType);
            }
            else
            {
                sig = MethodSig.CreateInstance(returnType, paramTypes);
            }

            return new MemberRefUser(module, methodName, sig, typeRef);
        }
    }
}
