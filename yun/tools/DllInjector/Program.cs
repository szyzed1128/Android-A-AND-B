using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;

namespace DllInjector
{
    internal sealed class InjectorPaths
    {
        public string CarDemoAndroidPath { get; init; }
        public string CarDemoPath { get; init; }
        public string WebSocketDllPath { get; init; }
        public string OutputDir { get; init; }
    }

    /// <summary>
    /// 存储从 OBDCloud.WebSocket.dll 导入的方法引用
    /// </summary>
    internal sealed class ImportedRefs
    {
        public IMethod GetInstance { get; set; }           // OBDCloudManager.get_Instance
        public IMethod GetIsInitialized { get; set; }      // OBDCloudManager.get_IsInitialized
        public IMethod InitializeIfNeededAsync { get; set; }
        public IMethod RegisterJSBridge { get; set; }
        public IMethod StartScanAsync { get; set; }
        public IMethod StopScanAsync { get; set; }
        public IMethod ConnectToDeviceAsync { get; set; }
        public IMethod SendOBDCommandAsyncBytes { get; set; }
        public IMethod GetOBDConnection { get; set; }      // OBDCloudManager.get_OBDConnection
        public IMethod ReadNextDataAsync { get; set; }     // WebSocketOBDConnection.ReadNextDataAsync
        public IMethod GetOBDConnectionIsConnected { get; set; }  // WebSocketOBDConnection.get_IsConnected
        public IMethod HandleUiInvoke { get; set; }        // OBDCloudManager.HandleUiInvoke
    }

    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("=== OBD Cloud DLL Injector v4.0 (dnlib merge) ===\n");

            var paths = ResolvePaths(args);
            if (!ValidatePaths(paths))
                return 1;

            Directory.CreateDirectory(paths.OutputDir);

            // 步骤 1: 加载两个模块
            Console.WriteLine("[1/3] Loading modules...");
            var androidModule = ModuleDefMD.Load(paths.CarDemoAndroidPath);
            var wsModule = ModuleDefMD.Load(paths.WebSocketDllPath);

            Console.WriteLine($"  CarDemo.Android.dll: {androidModule.Assembly.FullName}");
            Console.WriteLine($"  OBDCloud.WebSocket.dll: {wsModule.Assembly.FullName}");
            Console.WriteLine($"  MVID: {androidModule.Mvid}");

            // 步骤 2: 使用 dnlib 将 WebSocket 类型复制到 CarDemo.Android.dll
            Console.WriteLine("\n[2/3] Copying WebSocket types into CarDemo.Android.dll...");
            CopyTypesFromModule(androidModule, wsModule);

            // 步骤 2.5: 修复类型引用，将指向 OBDCloud.WebSocket 程序集的引用替换为本地 TypeDef
            Console.WriteLine("\n  Fixing type references...");

            // DEBUG: 检查复制的类型的 DefinitionAssembly
            var obdCloudTypes = androidModule.GetTypes().Where(t => t.FullName.StartsWith("OBDCloud.")).ToList();
            Console.WriteLine($"    OBDCloud types in module: {obdCloudTypes.Count}");
            foreach (var t in obdCloudTypes.Take(5))
            {
                Console.WriteLine($"      - {t.FullName}");
                Console.WriteLine($"        Module: {t.Module?.Name}");
                Console.WriteLine($"        DefinitionAssembly: {t.DefinitionAssembly?.Name}");
            }

            // DEBUG: 在修复前检查 TypeRef
            var preFixTypeRefs = androidModule.GetTypeRefs()
                .Where(tr => IsFromAssembly(tr, "OBDCloud.WebSocket"))
                .ToList();
            Console.WriteLine($"    Before fix: {preFixTypeRefs.Count} TypeRefs referencing OBDCloud.WebSocket");

            FixTypeReferences(androidModule, "OBDCloud.WebSocket");

            // DEBUG: 深度检查所有可能的引用
            Console.WriteLine("    Deep scanning for remaining references...");
            foreach (var type in androidModule.GetTypes().ToList())
            {
                foreach (var method in type.Methods)
                {
                    // 检查方法覆写
                    foreach (var ov in method.Overrides)
                    {
                        if (ov.MethodBody is MemberRef mbr && mbr.DeclaringType is TypeRef tr1 && tr1.ResolutionScope is AssemblyRef ar1 && ar1.Name == "OBDCloud.WebSocket")
                            Console.WriteLine($"    OVERRIDE: {type.Name}.{method.Name} Body -> {tr1.FullName}");
                        if (ov.MethodDeclaration is MemberRef mdr && mdr.DeclaringType is TypeRef tr2 && tr2.ResolutionScope is AssemblyRef ar2 && ar2.Name == "OBDCloud.WebSocket")
                            Console.WriteLine($"    OVERRIDE: {type.Name}.{method.Name} Decl -> {tr2.FullName}");
                    }

                    // 检查自定义属性
                    foreach (var ca in method.CustomAttributes)
                    {
                        if (ca.Constructor?.DeclaringType is TypeRef tr3 && tr3.ResolutionScope is AssemblyRef ar3 && ar3.Name == "OBDCloud.WebSocket")
                            Console.WriteLine($"    CA: {type.Name}.{method.Name} [{tr3.FullName}]");
                        // 检查 CA 参数中的类型引用
                        foreach (var arg in ca.ConstructorArguments)
                        {
                            if (arg.Value is TypeRef tr && tr.ResolutionScope is AssemblyRef ar && ar.Name == "OBDCloud.WebSocket")
                                Console.WriteLine($"    CA ARG: {type.Name}.{method.Name} -> {tr.FullName}");
                            if (arg.Type is ClassSig cs && IsFromAssembly(cs.TypeDefOrRef, "OBDCloud.WebSocket"))
                                Console.WriteLine($"    CA TYPE: {type.Name}.{method.Name} -> {cs.FullName}");
                        }
                    }
                }

                // 检查类型的自定义属性
                foreach (var ca in type.CustomAttributes)
                {
                    if (ca.Constructor?.DeclaringType is TypeRef tr4 && tr4.ResolutionScope is AssemblyRef ar4 && ar4.Name == "OBDCloud.WebSocket")
                        Console.WriteLine($"    TYPE CA: {type.Name} [{tr4.FullName}]");
                    foreach (var arg in ca.ConstructorArguments)
                    {
                        if (arg.Value is TypeDefOrRefSig tdrs && IsFromAssembly(tdrs.TypeDefOrRef, "OBDCloud.WebSocket"))
                            Console.WriteLine($"    TYPE CA ARG: {type.Name} -> {tdrs.FullName}");
                    }
                    foreach (var namedArg in ca.NamedArguments)
                    {
                        if (namedArg.Value is TypeDefOrRefSig tdrs2 && IsFromAssembly(tdrs2.TypeDefOrRef, "OBDCloud.WebSocket"))
                            Console.WriteLine($"    TYPE CA NAMED: {type.Name}.{namedArg.Name} -> {tdrs2.FullName}");
                    }
                }

                // 检查字段的自定义属性
                foreach (var field in type.Fields)
                {
                    foreach (var ca in field.CustomAttributes)
                    {
                        if (ca.Constructor?.DeclaringType is TypeRef tr5 && tr5.ResolutionScope is AssemblyRef ar5 && ar5.Name == "OBDCloud.WebSocket")
                            Console.WriteLine($"    FIELD CA: {type.Name}.{field.Name} [{tr5.FullName}]");
                    }
                }
            }
            Console.WriteLine("    Deep scan complete");

            // 找到合并后的方法引用
            var refs = ImportOBDCloudManagerRefsFromSameModule(androidModule);

            // 注入蓝牙钩子
            InjectBluetoothHooks(androidModule, refs);

            // 步骤 2.55: 最终清理 - 再次修复所有 TypeRef（确保没有遗漏）
            Console.WriteLine("\n  Final TypeRef cleanup...");
            FinalTypeRefCleanup(androidModule, "OBDCloud.WebSocket");

            // 步骤 2.6: 移除对 OBDCloud.WebSocket 的程序集引用
            Console.WriteLine("\n  Removing unused assembly references...");
            RemoveAssemblyReference(androidModule, "OBDCloud.WebSocket");

            var androidOut = Path.Combine(paths.OutputDir, "CarDemo.Android.dll");
            SaveModule(androidModule, androidOut);

            // 创建空的 OBDCloud.WebSocket.dll 占位符
            // 虽然所有类型已合并到 CarDemo.Android.dll，但仍有 AssemblyRef 引用 OBDCloud.WebSocket
            // 需要提供一个空的占位符 DLL 让 Mono 能够加载（但不包含任何类型定义以避免冲突）
            Console.WriteLine("\n  Creating empty OBDCloud.WebSocket.dll placeholder...");
            CreateEmptyPlaceholderDll(paths.OutputDir, "OBDCloud.WebSocket");

            // 验证 - 检查是否还有 OBDCloud.WebSocket 引用
            Console.WriteLine("\n  Running verification...");
            Verify.Run(androidOut);

            // 步骤 3: 处理 CarDemo.dll
            Console.WriteLine("\n[3/3] Processing CarDemo.dll...");
            var carDemoModule = ModuleDefMD.Load(paths.CarDemoPath);
            InjectUiHooksSimplified(carDemoModule);
            var carDemoOut = Path.Combine(paths.OutputDir, "CarDemo.dll");
            SaveModule(carDemoModule, carDemoOut);

            Console.WriteLine("\nDone!");
            Console.WriteLine($"Output directory: {paths.OutputDir}");
            return 0;
        }

        /// <summary>
        /// 使用 dnlib 将源模块的所有类型复制到目标模块
        /// 保持目标模块的标识信息不变（MVID、Assembly Name 等）
        /// </summary>
        private static void CopyTypesFromModule(ModuleDefMD targetModule, ModuleDefMD sourceModule)
        {
            var importer = new Importer(targetModule);
            var typesToCopy = sourceModule.GetTypes()
                .Where(t => !t.IsGlobalModuleType)
                .ToList();

            Console.WriteLine($"  Found {typesToCopy.Count} types to copy");

            var typeDefMap = new Dictionary<string, TypeDef>(StringComparer.Ordinal);
            var newSourceTypes = new HashSet<TypeDef>();

            // Pass 1: create type shells and map
            foreach (var sourceType in typesToCopy)
            {
                if (sourceType.IsGlobalModuleType)
                    continue;

                var existing = targetModule.Find(sourceType.FullName, false);
                if (existing != null)
                {
                    typeDefMap[sourceType.FullName] = existing;
                    Console.WriteLine($"  SKIP (exists): {sourceType.FullName}");
                    continue;
                }

                var newType = new TypeDefUser(sourceType.Namespace, sourceType.Name, null)
                {
                    Attributes = sourceType.Attributes
                };

                typeDefMap[sourceType.FullName] = newType;
                newSourceTypes.Add(sourceType);
            }

            // Pass 2: attach types (top-level or nested)
            foreach (var sourceType in typesToCopy)
            {
                if (!typeDefMap.TryGetValue(sourceType.FullName, out var targetType))
                    continue;

                if (targetType.Module != null)
                    continue;

                if (sourceType.DeclaringType != null &&
                    typeDefMap.TryGetValue(sourceType.DeclaringType.FullName, out var targetDeclaring))
                {
                    targetDeclaring.NestedTypes.Add(targetType);
                }
                else
                {
                    targetModule.Types.Add(targetType);
                }

                if (newSourceTypes.Contains(sourceType))
                    Console.WriteLine($"  + {sourceType.FullName}");
            }

            var fieldMap = new Dictionary<FieldDef, FieldDef>();
            var methodMap = new Dictionary<MethodDef, MethodDef>();

            // Pass 3: copy members (signatures only)
            foreach (var sourceType in typesToCopy)
            {
                if (!newSourceTypes.Contains(sourceType))
                    continue;

                var newType = typeDefMap[sourceType.FullName];

                // 基类
                newType.BaseType = ImportTypeDefOrRef(sourceType.BaseType, importer, typeDefMap);

                // 复制接口实现
                foreach (var iface in sourceType.Interfaces)
                {
                    newType.Interfaces.Add(new InterfaceImplUser(ImportTypeDefOrRef(iface.Interface, importer, typeDefMap)));
                }

                // 复制泛型参数
                foreach (var gp in sourceType.GenericParameters)
                {
                    var newGp = new GenericParamUser(gp.Number, gp.Flags, gp.Name);
                    foreach (var gc in gp.GenericParamConstraints)
                    {
                        var constraint = ImportTypeDefOrRef(gc.Constraint, importer, typeDefMap);
                        newGp.GenericParamConstraints.Add(new GenericParamConstraintUser(constraint));
                    }
                    newType.GenericParameters.Add(newGp);
                }

                // 复制字段
                foreach (var field in sourceType.Fields)
                {
                    var newFieldSig = ImportFieldSig(field.FieldSig, importer, typeDefMap);
                    var newField = new FieldDefUser(field.Name, newFieldSig, field.Attributes);
                    if (field.HasConstant)
                        newField.Constant = field.Constant;
                    if (field.InitialValue != null)
                        newField.InitialValue = field.InitialValue;
                    newType.Fields.Add(newField);
                    fieldMap[field] = newField;
                }

                // 复制方法（先只复制签名，方法体稍后处理）
                foreach (var method in sourceType.Methods)
                {
                    var newMethodSig = ImportMethodSig(method.MethodSig, importer, typeDefMap);
                    var newMethod = new MethodDefUser(
                        method.Name,
                        newMethodSig,
                        method.ImplAttributes,
                        method.Attributes);

                    // 复制泛型参数
                    foreach (var gp in method.GenericParameters)
                    {
                        var newGp = new GenericParamUser(gp.Number, gp.Flags, gp.Name);
                        foreach (var gc in gp.GenericParamConstraints)
                        {
                            var constraint = ImportTypeDefOrRef(gc.Constraint, importer, typeDefMap);
                            newGp.GenericParamConstraints.Add(new GenericParamConstraintUser(constraint));
                        }
                        newMethod.GenericParameters.Add(newGp);
                    }

                    // 复制参数定义
                    foreach (var param in method.ParamDefs)
                    {
                        newMethod.ParamDefs.Add(new ParamDefUser(param.Name, param.Sequence, param.Attributes));
                    }

                    newType.Methods.Add(newMethod);
                    methodMap[method] = newMethod;
                }

                // 复制属性
                foreach (var prop in sourceType.Properties)
                {
                    var newPropSig = ImportPropertySig(prop.PropertySig, importer, typeDefMap);
                    var newProp = new PropertyDefUser(prop.Name, newPropSig, prop.Attributes);

                    if (prop.GetMethod != null && methodMap.TryGetValue(prop.GetMethod, out var getter))
                        newProp.GetMethod = getter;
                    else if (prop.GetMethod != null)
                        newProp.GetMethod = newType.Methods.FirstOrDefault(m => m.Name == prop.GetMethod.Name);

                    if (prop.SetMethod != null && methodMap.TryGetValue(prop.SetMethod, out var setter))
                        newProp.SetMethod = setter;
                    else if (prop.SetMethod != null)
                        newProp.SetMethod = newType.Methods.FirstOrDefault(m => m.Name == prop.SetMethod.Name);

                    newType.Properties.Add(newProp);
                }

                // 复制事件
                foreach (var evt in sourceType.Events)
                {
                    var newEvt = new EventDefUser(evt.Name, ImportTypeDefOrRef(evt.EventType, importer, typeDefMap), evt.Attributes);
                    if (evt.AddMethod != null && methodMap.TryGetValue(evt.AddMethod, out var add))
                        newEvt.AddMethod = add;
                    else if (evt.AddMethod != null)
                        newEvt.AddMethod = newType.Methods.FirstOrDefault(m => m.Name == evt.AddMethod.Name);

                    if (evt.RemoveMethod != null && methodMap.TryGetValue(evt.RemoveMethod, out var remove))
                        newEvt.RemoveMethod = remove;
                    else if (evt.RemoveMethod != null)
                        newEvt.RemoveMethod = newType.Methods.FirstOrDefault(m => m.Name == evt.RemoveMethod.Name);

                    if (evt.InvokeMethod != null && methodMap.TryGetValue(evt.InvokeMethod, out var invoke))
                        newEvt.InvokeMethod = invoke;
                    else if (evt.InvokeMethod != null)
                        newEvt.InvokeMethod = newType.Methods.FirstOrDefault(m => m.Name == evt.InvokeMethod.Name);

                    newType.Events.Add(newEvt);
                }
            }

            // Pass 3.5: copy method overrides (MethodImpl)
            foreach (var sourceType in typesToCopy)
            {
                if (!newSourceTypes.Contains(sourceType))
                    continue;

                foreach (var method in sourceType.Methods)
                {
                    if (!methodMap.TryGetValue(method, out var newMethod))
                        continue;

                    if (!method.HasOverrides)
                        continue;

                    foreach (var ov in method.Overrides)
                    {
                        var bodyRef = ov.MethodBody as IMethodDefOrRef;
                        var declRef = ov.MethodDeclaration as IMethodDefOrRef;
                        if (bodyRef == null || declRef == null)
                            continue;

                        var newBody = ImportMethodDefOrRef(bodyRef, importer, typeDefMap, methodMap) as IMethodDefOrRef;
                        var newDecl = ImportMethodDefOrRef(declRef, importer, typeDefMap, methodMap) as IMethodDefOrRef;
                        if (newBody != null && newDecl != null)
                            newMethod.Overrides.Add(new MethodOverride(newBody, newDecl));
                    }
                }
            }

            // Pass 4: copy method bodies
            foreach (var sourceType in typesToCopy)
            {
                if (!newSourceTypes.Contains(sourceType))
                    continue;

                foreach (var method in sourceType.Methods)
                {
                    if (!method.HasBody)
                        continue;

                    if (!methodMap.TryGetValue(method, out var newMethod))
                        continue;

                    newMethod.Body = new CilBody();
                    CopyMethodBody(method.Body, newMethod.Body, importer, typeDefMap, methodMap, fieldMap);
                    newMethod.Body.KeepOldMaxStack = true;
                }
            }

            // 第二遍：修复嵌套类型和交叉引用
            FixNestedTypeReferences(targetModule, sourceModule, importer);
        }

        /// <summary>
        /// 复制方法体
        /// </summary>
        private static void CopyMethodBody(
            CilBody source,
            CilBody target,
            Importer importer,
            IReadOnlyDictionary<string, TypeDef> typeDefMap,
            IReadOnlyDictionary<MethodDef, MethodDef> methodMap,
            IReadOnlyDictionary<FieldDef, FieldDef> fieldMap)
        {
            target.InitLocals = source.InitLocals;
            target.MaxStack = source.MaxStack;

            // 复制局部变量
            foreach (var local in source.Variables)
            {
                var newLocalType = ImportTypeSig(local.Type, importer, typeDefMap);
                target.Variables.Add(new Local(newLocalType, local.Name));
            }

            // 先创建所有指令（用于分支引用）
            var instrMap = new Dictionary<Instruction, Instruction>();
            foreach (var instr in source.Instructions)
            {
                var newInstr = new Instruction(instr.OpCode);
                instrMap[instr] = newInstr;
                target.Instructions.Add(newInstr);
            }

            // 填充操作数
            for (int i = 0; i < source.Instructions.Count; i++)
            {
                var srcInstr = source.Instructions[i];
                var tgtInstr = target.Instructions[i];

                tgtInstr.Operand = ImportOperand(srcInstr, instrMap, importer, target, typeDefMap, methodMap, fieldMap);
            }

            // 复制异常处理器
            foreach (var eh in source.ExceptionHandlers)
            {
                var newEh = new ExceptionHandler(eh.HandlerType);
                if (eh.TryStart != null) newEh.TryStart = instrMap[eh.TryStart];
                if (eh.TryEnd != null) newEh.TryEnd = instrMap[eh.TryEnd];
                if (eh.HandlerStart != null) newEh.HandlerStart = instrMap[eh.HandlerStart];
                if (eh.HandlerEnd != null) newEh.HandlerEnd = instrMap[eh.HandlerEnd];
                if (eh.FilterStart != null) newEh.FilterStart = instrMap[eh.FilterStart];
                if (eh.CatchType != null) newEh.CatchType = ImportTypeDefOrRef(eh.CatchType, importer, typeDefMap);
                target.ExceptionHandlers.Add(newEh);
            }
        }

        /// <summary>
        /// 导入指令操作数
        /// </summary>
        private static object ImportOperand(
            Instruction srcInstr,
            Dictionary<Instruction, Instruction> instrMap,
            Importer importer,
            CilBody targetBody,
            IReadOnlyDictionary<string, TypeDef> typeDefMap,
            IReadOnlyDictionary<MethodDef, MethodDef> methodMap,
            IReadOnlyDictionary<FieldDef, FieldDef> fieldMap)
        {
            var operand = srcInstr.Operand;
            if (operand == null)
                return null;

            // 分支目标
            if (operand is Instruction targetInstr)
                return instrMap.TryGetValue(targetInstr, out var mapped) ? mapped : null;

            // 分支目标数组（switch）
            if (operand is Instruction[] targets)
                return targets.Select(t => instrMap.TryGetValue(t, out var m) ? m : null).ToArray();

            if (operand is MethodSpec methodSpec)
            {
                try { return ImportMethodSpec(methodSpec, importer, typeDefMap, methodMap); }
                catch { return operand; }
            }

            // 方法引用
            if (operand is IMethodDefOrRef methodRef)
            {
                try { return ImportMethodDefOrRef(methodRef, importer, typeDefMap, methodMap); }
                catch { return operand; }
            }

            // 类型引用
            if (operand is ITypeDefOrRef typeRef)
            {
                try { return ImportTypeDefOrRef(typeRef, importer, typeDefMap); }
                catch { return operand; }
            }

            // 字段引用
            if (operand is IField fieldRef)
            {
                try { return ImportFieldDefOrRef(fieldRef, importer, typeDefMap, fieldMap); }
                catch { return operand; }
            }

            // 局部变量
            if (operand is Local local)
            {
                var idx = local.Index;
                return idx < targetBody.Variables.Count ? targetBody.Variables[idx] : operand;
            }

            // 参数
            if (operand is Parameter param)
                return operand; // 参数直接复用

            // 字符串、int、float 等基本类型直接返回
            return operand;
        }

        private static FieldSig ImportFieldSig(FieldSig fieldSig, Importer importer, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (fieldSig == null) return null;
            var imported = importer.Import(fieldSig);
            var newType = ReplaceTypeSigWithLocal(imported.Type, typeDefMap);
            return newType != imported.Type ? new FieldSig(newType) : imported;
        }

        private static MethodSig ImportMethodSig(MethodSig methodSig, Importer importer, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (methodSig == null) return null;
            var imported = importer.Import(methodSig);

            var newRetType = ReplaceTypeSigWithLocal(imported.RetType, typeDefMap);
            if (newRetType != imported.RetType)
                imported.RetType = newRetType;

            for (int i = 0; i < imported.Params.Count; i++)
            {
                var newParam = ReplaceTypeSigWithLocal(imported.Params[i], typeDefMap);
                if (newParam != imported.Params[i])
                    imported.Params[i] = newParam;
            }

            if (imported.ParamsAfterSentinel != null)
            {
                for (int i = 0; i < imported.ParamsAfterSentinel.Count; i++)
                {
                    var newParam = ReplaceTypeSigWithLocal(imported.ParamsAfterSentinel[i], typeDefMap);
                    if (newParam != imported.ParamsAfterSentinel[i])
                        imported.ParamsAfterSentinel[i] = newParam;
                }
            }

            return imported;
        }

        private static PropertySig ImportPropertySig(PropertySig propertySig, Importer importer, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (propertySig == null) return null;
            var imported = importer.Import(propertySig);

            var newRetType = ReplaceTypeSigWithLocal(imported.RetType, typeDefMap);
            if (newRetType != imported.RetType)
                imported.RetType = newRetType;

            for (int i = 0; i < imported.Params.Count; i++)
            {
                var newParam = ReplaceTypeSigWithLocal(imported.Params[i], typeDefMap);
                if (newParam != imported.Params[i])
                    imported.Params[i] = newParam;
            }

            return imported;
        }

        private static ITypeDefOrRef ImportTypeDefOrRef(ITypeDefOrRef typeRef, Importer importer, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (typeRef == null) return null;

            if (typeRef is TypeSpec typeSpec)
            {
                var importedSig = ImportTypeSig(typeSpec.TypeSig, importer, typeDefMap);
                return new TypeSpecUser(importedSig);
            }

            if (TryMapTypeDefOrRef(typeRef, typeDefMap, out var localType))
                return localType;

            return importer.Import(typeRef);
        }

        private static TypeSig ImportTypeSig(TypeSig typeSig, Importer importer, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (typeSig == null) return null;
            var imported = importer.Import(typeSig);
            return ReplaceTypeSigWithLocal(imported, typeDefMap);
        }

        private static TypeSig ReplaceTypeSigWithLocal(TypeSig typeSig, IReadOnlyDictionary<string, TypeDef> typeDefMap)
        {
            if (typeSig == null) return null;

            switch (typeSig)
            {
                case ClassSig classSig:
                    if (TryMapTypeDefOrRef(classSig.TypeDefOrRef, typeDefMap, out var localClass))
                        return new ClassSig(localClass);
                    break;

                case ValueTypeSig valueTypeSig:
                    if (TryMapTypeDefOrRef(valueTypeSig.TypeDefOrRef, typeDefMap, out var localValue))
                        return new ValueTypeSig(localValue);
                    break;

                case GenericInstSig genericInstSig:
                    bool genericModified = false;
                    var newArgs = new List<TypeSig>(genericInstSig.GenericArguments.Count);
                    foreach (var arg in genericInstSig.GenericArguments)
                    {
                        var newArg = ReplaceTypeSigWithLocal(arg, typeDefMap);
                        if (newArg != arg)
                            genericModified = true;
                        newArgs.Add(newArg);
                    }

                    var genericType = genericInstSig.GenericType;
                    ClassOrValueTypeSig newGenericType = genericType;
                    if (genericType?.TypeDefOrRef != null &&
                        TryMapTypeDefOrRef(genericType.TypeDefOrRef, typeDefMap, out var localGenericType))
                    {
                        newGenericType = localGenericType.IsValueType
                            ? (ClassOrValueTypeSig)new ValueTypeSig(localGenericType)
                            : new ClassSig(localGenericType);
                        genericModified = true;
                    }

                    if (genericModified)
                        return new GenericInstSig(newGenericType, newArgs);
                    break;

                case SZArraySig szArraySig:
                    var newElem = ReplaceTypeSigWithLocal(szArraySig.Next, typeDefMap);
                    if (newElem != szArraySig.Next)
                        return new SZArraySig(newElem);
                    break;

                case ByRefSig byRefSig:
                    var newByRef = ReplaceTypeSigWithLocal(byRefSig.Next, typeDefMap);
                    if (newByRef != byRefSig.Next)
                        return new ByRefSig(newByRef);
                    break;

                case PtrSig ptrSig:
                    var newPtr = ReplaceTypeSigWithLocal(ptrSig.Next, typeDefMap);
                    if (newPtr != ptrSig.Next)
                        return new PtrSig(newPtr);
                    break;
            }

            return typeSig;
        }

        private static IMethod ImportMethodDefOrRef(
            IMethodDefOrRef methodRef,
            Importer importer,
            IReadOnlyDictionary<string, TypeDef> typeDefMap,
            IReadOnlyDictionary<MethodDef, MethodDef> methodMap)
        {
            if (methodRef == null) return null;

            if (methodRef is MethodDef methodDef && methodMap.TryGetValue(methodDef, out var localMethod))
                return localMethod;

            if (methodRef is MemberRef memberRef)
            {
                if (TryMapTypeDefOrRef(memberRef.DeclaringType, typeDefMap, out var localType))
                {
                    var matchedMethod = localType.Methods.FirstOrDefault(m =>
                        m.Name == memberRef.Name && MatchMethodSig(m.MethodSig, memberRef.MethodSig));
                    if (matchedMethod != null)
                        return matchedMethod;
                }
            }

            return importer.Import(methodRef);
        }

        private static object ImportMethodSpec(
            MethodSpec methodSpec,
            Importer importer,
            IReadOnlyDictionary<string, TypeDef> typeDefMap,
            IReadOnlyDictionary<MethodDef, MethodDef> methodMap)
        {
            var newMethod = ImportMethodDefOrRef(methodSpec.Method, importer, typeDefMap, methodMap);
            if (newMethod is not IMethodDefOrRef methodDefOrRef)
                return importer.Import(methodSpec);

            GenericInstMethodSig newGenericSig = null;
            if (methodSpec.GenericInstMethodSig != null)
            {
                var newArgs = new List<TypeSig>(methodSpec.GenericInstMethodSig.GenericArguments.Count);
                foreach (var arg in methodSpec.GenericInstMethodSig.GenericArguments)
                {
                    newArgs.Add(ImportTypeSig(arg, importer, typeDefMap));
                }
                newGenericSig = new GenericInstMethodSig(newArgs.ToArray());
            }

            return new MethodSpecUser(methodDefOrRef, newGenericSig ?? methodSpec.GenericInstMethodSig);
        }

        private static IField ImportFieldDefOrRef(
            IField fieldRef,
            Importer importer,
            IReadOnlyDictionary<string, TypeDef> typeDefMap,
            IReadOnlyDictionary<FieldDef, FieldDef> fieldMap)
        {
            if (fieldRef == null) return null;

            if (fieldRef is FieldDef fieldDef && fieldMap.TryGetValue(fieldDef, out var localField))
                return localField;

            if (fieldRef is MemberRef memberRef && memberRef.IsFieldRef)
            {
                if (TryMapTypeDefOrRef(memberRef.DeclaringType, typeDefMap, out var localType))
                {
                    var matchedField = localType.Fields.FirstOrDefault(f => f.Name == memberRef.Name);
                    if (matchedField != null)
                        return matchedField;
                }
            }

            return importer.Import(fieldRef);
        }

        private static bool TryMapTypeDefOrRef(ITypeDefOrRef typeRef, IReadOnlyDictionary<string, TypeDef> typeDefMap, out TypeDef localType)
        {
            localType = null;
            if (typeRef == null) return false;
            var fullName = typeRef.FullName;
            if (typeDefMap.TryGetValue(fullName, out localType))
                return true;

            // 兼容嵌套类型的不同分隔符表示
            if (fullName.Contains('+'))
            {
                var alt = fullName.Replace('+', '/');
                if (typeDefMap.TryGetValue(alt, out localType))
                    return true;
            }
            else if (fullName.Contains('/'))
            {
                var alt = fullName.Replace('/', '+');
                if (typeDefMap.TryGetValue(alt, out localType))
                    return true;
            }

            // 若为嵌套类型但 FullName 不完整，尝试组合父类型名称
            if (typeRef is TypeRef tr && tr.ResolutionScope is TypeRef parentRef)
            {
                var parentName = parentRef.FullName;
                var nestedFull = parentName + "/" + tr.Name;
                if (typeDefMap.TryGetValue(nestedFull, out localType))
                    return true;

                var nestedAlt = parentName + "+" + tr.Name;
                if (typeDefMap.TryGetValue(nestedAlt, out localType))
                    return true;
            }

            // 最后尝试按 Name 唯一匹配（仅用于无命名空间的嵌套类型）
            if (string.IsNullOrEmpty(typeRef.Namespace))
            {
                var matches = typeDefMap.Values.Where(t => t.Name == typeRef.Name).ToList();
                if (matches.Count == 1)
                {
                    localType = matches[0];
                    return true;
                }
            }

            return false;
        }

        private static void SetTypeRefResolutionScope(
            TypeRef typeRef,
            TypeDef localType,
            ModuleDef module,
            Dictionary<TypeDef, TypeRef> typeRefCache)
        {
            if (typeRef == null || localType == null)
                return;

            if (localType.DeclaringType == null)
            {
                typeRef.ResolutionScope = module;
                return;
            }

            var parentRef = GetOrCreateLocalTypeRef(localType.DeclaringType, module, typeRefCache);
            typeRef.ResolutionScope = parentRef;
        }

        private static TypeRef GetOrCreateLocalTypeRef(
            TypeDef typeDef,
            ModuleDef module,
            Dictionary<TypeDef, TypeRef> typeRefCache)
        {
            if (typeRefCache.TryGetValue(typeDef, out var cached))
                return cached;

            IResolutionScope scope = module;
            if (typeDef.DeclaringType != null)
                scope = GetOrCreateLocalTypeRef(typeDef.DeclaringType, module, typeRefCache);

            var typeRef = new TypeRefUser(module, typeDef.Namespace, typeDef.Name, scope);
            typeRefCache[typeDef] = typeRef;
            return typeRef;
        }

        /// <summary>
        /// 修复嵌套类型引用（第二遍处理）
        /// </summary>
        private static void FixNestedTypeReferences(ModuleDefMD targetModule, ModuleDefMD sourceModule, Importer importer)
        {
            // 处理嵌套类型
            foreach (var sourceType in sourceModule.GetTypes().Where(t => !t.IsGlobalModuleType))
            {
                if (!sourceType.HasNestedTypes) continue;

                var targetType = targetModule.Find(sourceType.FullName, false);
                if (targetType == null) continue;

                foreach (var nestedSource in sourceType.NestedTypes)
                {
                    // 查找已经添加到目标模块的对应类型
                    var nestedTarget = targetModule.Find(nestedSource.FullName, false);
                    if (nestedTarget != null && !targetType.NestedTypes.Contains(nestedTarget))
                    {
                        // 从顶层移到嵌套
                        if (targetModule.Types.Contains(nestedTarget))
                            targetModule.Types.Remove(nestedTarget);
                        targetType.NestedTypes.Add(nestedTarget);
                    }
                }
            }
        }

        /// <summary>
        /// 从同一个模块中导入 OBDCloudManager 方法引用（合并后）
        /// </summary>
        private static ImportedRefs ImportOBDCloudManagerRefsFromSameModule(ModuleDefMD module)
        {
            var refs = new ImportedRefs();

            // 找到 OBDCloudManager 类型（现在在同一个模块中）
            var obdCloudManagerType = module.GetTypes()
                .FirstOrDefault(t => t.FullName == "OBDCloud.WebSocket.OBDCloudManager");

            if (obdCloudManagerType == null)
            {
                Console.WriteLine("  ERROR: OBDCloudManager not found in merged module!");
                return refs;
            }

            Console.WriteLine($"  Found OBDCloudManager: {obdCloudManagerType.FullName}");

            // 导入属性 getter（直接引用，因为在同一模块）
            var instanceProp = obdCloudManagerType.Properties.FirstOrDefault(p => p.Name == "Instance");
            if (instanceProp?.GetMethod != null)
                refs.GetInstance = instanceProp.GetMethod;

            var isInitializedProp = obdCloudManagerType.Properties.FirstOrDefault(p => p.Name == "IsInitialized");
            if (isInitializedProp?.GetMethod != null)
                refs.GetIsInitialized = isInitializedProp.GetMethod;

            var obdConnectionProp = obdCloudManagerType.Properties.FirstOrDefault(p => p.Name == "OBDConnection");
            if (obdConnectionProp?.GetMethod != null)
                refs.GetOBDConnection = obdConnectionProp.GetMethod;

            // 导入方法
            foreach (var method in obdCloudManagerType.Methods)
            {
                switch (method.Name)
                {
                    case "InitializeIfNeededAsync":
                        refs.InitializeIfNeededAsync = method;
                        break;
                    case "RegisterJSBridge":
                        refs.RegisterJSBridge = method;
                        break;
                    case "StartScanAsync":
                        refs.StartScanAsync = method;
                        break;
                    case "StopScanAsync":
                        refs.StopScanAsync = method;
                        break;
                    case "ConnectToDeviceAsync":
                        refs.ConnectToDeviceAsync = method;
                        break;
                    case "SendOBDCommandAsync":
                        // 使用 MethodSig.Params（不含隐式 this 参数）
                        if (method.MethodSig.Params.Count == 1 &&
                            method.MethodSig.Params[0].FullName == "System.Byte[]")
                            refs.SendOBDCommandAsyncBytes = method;
                        break;
                    case "HandleUiInvoke":
                        refs.HandleUiInvoke = method;
                        break;
                }
            }

            // 找到 WebSocketOBDConnection 类型并导入 ReadNextDataAsync 和 IsConnected
            var obdConnType = module.GetTypes()
                .FirstOrDefault(t => t.FullName == "OBDCloud.WebSocket.WebSocketOBDConnection");
            if (obdConnType != null)
            {
                var readNextData = obdConnType.Methods.FirstOrDefault(m => m.Name == "ReadNextDataAsync");
                if (readNextData != null)
                    refs.ReadNextDataAsync = readNextData;

                // 导入 IsConnected 属性 getter
                var isConnectedProp = obdConnType.Properties.FirstOrDefault(p => p.Name == "IsConnected");
                if (isConnectedProp?.GetMethod != null)
                    refs.GetOBDConnectionIsConnected = isConnectedProp.GetMethod;
            }

            // 验证必需的引用
            PrintRefStatus("Instance", refs.GetInstance);
            PrintRefStatus("IsInitialized", refs.GetIsInitialized);
            PrintRefStatus("InitializeIfNeededAsync", refs.InitializeIfNeededAsync);
            PrintRefStatus("StartScanAsync", refs.StartScanAsync);
            PrintRefStatus("StopScanAsync", refs.StopScanAsync);
            PrintRefStatus("ConnectToDeviceAsync", refs.ConnectToDeviceAsync);
            PrintRefStatus("SendOBDCommandAsync", refs.SendOBDCommandAsyncBytes);
            PrintRefStatus("ReadNextDataAsync", refs.ReadNextDataAsync);
            PrintRefStatus("OBDConnection", refs.GetOBDConnection);
            PrintRefStatus("OBDConnection.IsConnected", refs.GetOBDConnectionIsConnected);
            PrintRefStatus("HandleUiInvoke", refs.HandleUiInvoke);

            return refs;
        }

        private static void PrintRefStatus(string name, IMethod method)
        {
            if (method != null)
                Console.WriteLine($"    ✓ {name}");
            else
                Console.WriteLine($"    ✗ {name} - NOT FOUND");
        }

        private static void PatchMainWebViewInvokeAsync(ModuleDefMD module, ImportedRefs refs)
        {
            if (refs.HandleUiInvoke == null)
            {
                Console.WriteLine("  WARNING: HandleUiInvoke not found, skip MainWebView patch");
                return;
            }

            var mainWebViewType = module.GetTypes()
                .FirstOrDefault(t => t.FullName == "CarDemo.Controls.MainWebView");
            if (mainWebViewType == null)
            {
                Console.WriteLine("  WARNING: MainWebView not found");
                return;
            }

            var methods = mainWebViewType.Methods
                .Where(m => m.Name == "InvokeAsync" && m.HasBody)
                .ToList();

            if (methods.Count == 0)
            {
                Console.WriteLine("  WARNING: MainWebView.InvokeAsync not found");
                return;
            }

            foreach (var method in methods)
            {
                if (method.Body.Instructions.Any(i => i.OpCode == OpCodes.Call && i.Operand == refs.HandleUiInvoke))
                {
                    Console.WriteLine($"  InvokeAsync already patched: {method.Name}");
                    continue;
                }

                var methodNameParam = method.Parameters.FirstOrDefault(p => p.Type.FullName == "System.String");
                if (methodNameParam == null)
                {
                    Console.WriteLine($"  WARNING: InvokeAsync missing method name param: {method.Name}");
                    continue;
                }

                var argsParam = method.Parameters.FirstOrDefault(p => p.Type.FullName == "System.Object[]");

                var insert = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldarg, methodNameParam),
                    argsParam != null ? Instruction.Create(OpCodes.Ldarg, argsParam) : Instruction.Create(OpCodes.Ldnull),
                    Instruction.Create(OpCodes.Call, refs.HandleUiInvoke)
                };

                for (var i = insert.Count - 1; i >= 0; i--)
                {
                    method.Body.Instructions.Insert(0, insert[i]);
                }

                method.Body.KeepOldMaxStack = true;
                Console.WriteLine($"  Patched MainWebView.InvokeAsync: {method.Name}");
            }
        }

        /// <summary>
        /// 简化版的 UI hooks 注入（不依赖外部 WebSocket 类型）
        /// </summary>
        private static void InjectUiHooksSimplified(ModuleDefMD module)
        {
            var proxyMethod = EnsureUiBridgeProxy(module);
            PatchEvaluateJavaScriptAsync(module, proxyMethod);
        }

        private static MethodDef EnsureUiBridgeProxy(ModuleDefMD module)
        {
            const string proxyNamespace = "CarDemo.Controls";
            const string proxyName = "WebSocketUiProxy";
            const string proxyFullName = proxyNamespace + "." + proxyName;

            var proxyType = module.Types.FirstOrDefault(t => t.FullName == proxyFullName);
            if (proxyType == null)
            {
                proxyType = new TypeDefUser(proxyNamespace, proxyName, module.CorLibTypes.Object.TypeDefOrRef)
                {
                    Attributes = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract
                };
                module.Types.Add(proxyType);
            }

            var existing = proxyType.Methods.FirstOrDefault(m =>
                m.Name == "HandleOutgoingJS" && m.IsStatic && m.Parameters.Count == 1);
            if (existing != null)
                return existing;

            var sig = MethodSig.CreateStatic(
                module.CorLibTypes.Void,
                module.CorLibTypes.String);
            var method = new MethodDefUser(
                "HandleOutgoingJS",
                sig,
                MethodImplAttributes.IL | MethodImplAttributes.Managed,
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);

            var body = new CilBody { InitLocals = true, MaxStack = 4 };

            // 使用 mscorlib 中的类型引用，避免引入 System.Private.CoreLib
            var mscorlibRef = module.GetAssemblyRefs().FirstOrDefault(r => r.Name == "mscorlib")
                ?? (module.CorLibTypes.Object.TypeDefOrRef.DefinitionAssembly as AssemblyRef);
            if (mscorlibRef == null)
            {
                // 如果没有 mscorlib 引用，创建一个（TypeRefUser 会自动将引用添加到模块）
                mscorlibRef = new AssemblyRefUser("mscorlib", new Version(2, 0, 5, 0), new PublicKeyToken(new byte[] { 0x7c, 0xec, 0x85, 0xd7, 0xbe, 0xa7, 0x79, 0x8e }));
            }

            // 创建 System.Type 类型引用
            var typeTypeRef = new TypeRefUser(module, "System", "Type", mscorlibRef);
            // 创建 System.Reflection.MethodInfo 类型引用
            var methodInfoTypeRef = new TypeRefUser(module, "System.Reflection", "MethodInfo", mscorlibRef);
            // 创建 System.Reflection.BindingFlags 类型引用（值类型）
            var bindingFlagsTypeRef = new TypeRefUser(module, "System.Reflection", "BindingFlags", mscorlibRef);

            var typeLocal = new Local(typeTypeRef.ToTypeSig());
            var methodLocal = new Local(methodInfoTypeRef.ToTypeSig());
            var argsLocal = new Local(new SZArraySig(module.CorLibTypes.Object));
            body.Variables.Add(typeLocal);
            body.Variables.Add(methodLocal);
            body.Variables.Add(argsLocal);

            // String.IsNullOrEmpty(string) - 静态方法
            var isNullOrEmpty = new MemberRefUser(
                module,
                "IsNullOrEmpty",
                MethodSig.CreateStatic(module.CorLibTypes.Boolean, module.CorLibTypes.String),
                module.CorLibTypes.String.TypeDefOrRef);

            // Type.GetType(string, bool) - 静态方法
            var getType = new MemberRefUser(
                module,
                "GetType",
                MethodSig.CreateStatic(typeTypeRef.ToTypeSig(), module.CorLibTypes.String, module.CorLibTypes.Boolean),
                typeTypeRef);

            // Type.GetMethod(string, BindingFlags) - 实例方法
            var getMethod = new MemberRefUser(
                module,
                "GetMethod",
                MethodSig.CreateInstance(methodInfoTypeRef.ToTypeSig(), module.CorLibTypes.String, new ValueTypeSig(bindingFlagsTypeRef)),
                typeTypeRef);

            // MethodInfo.Invoke(object, object[]) - 实例方法
            var invoke = new MemberRefUser(
                module,
                "Invoke",
                MethodSig.CreateInstance(module.CorLibTypes.Object, module.CorLibTypes.Object, new SZArraySig(module.CorLibTypes.Object)),
                methodInfoTypeRef);

            var proceed = Instruction.Create(OpCodes.Nop);
            var hasType = Instruction.Create(OpCodes.Nop);
            var ret = Instruction.Create(OpCodes.Ret);

            var instructions = body.Instructions;
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call, isNullOrEmpty));
            instructions.Add(Instruction.Create(OpCodes.Brfalse_S, proceed));
            instructions.Add(Instruction.Create(OpCodes.Ret));

            instructions.Add(proceed);
            // 类型已合并到 CarDemo.Android.dll，直接从该程序集加载
            instructions.Add(Instruction.Create(OpCodes.Ldstr, "OBDCloud.WebSocket.WebSocketUIBridge, CarDemo.Android"));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            instructions.Add(Instruction.Create(OpCodes.Call, getType));
            instructions.Add(Instruction.Create(OpCodes.Stloc, typeLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, typeLocal));
            instructions.Add(Instruction.Create(OpCodes.Brfalse_S, ret));

            instructions.Add(hasType);
            instructions.Add(Instruction.Create(OpCodes.Ldloc, typeLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldstr, "HandleOutgoingJS"));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            instructions.Add(Instruction.Create(OpCodes.Callvirt, getMethod));
            instructions.Add(Instruction.Create(OpCodes.Stloc, methodLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, methodLocal));
            instructions.Add(Instruction.Create(OpCodes.Brfalse_S, ret));

            instructions.Add(Instruction.Create(OpCodes.Ldc_I4_1));
            instructions.Add(Instruction.Create(OpCodes.Newarr, module.CorLibTypes.Object.TypeDefOrRef));
            instructions.Add(Instruction.Create(OpCodes.Stloc, argsLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, argsLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, methodLocal));
            instructions.Add(Instruction.Create(OpCodes.Ldnull));
            instructions.Add(Instruction.Create(OpCodes.Ldloc, argsLocal));
            instructions.Add(Instruction.Create(OpCodes.Callvirt, invoke));
            instructions.Add(Instruction.Create(OpCodes.Pop));
            instructions.Add(ret);

            method.Body = body;
            proxyType.Methods.Add(method);

            return method;
        }

        private static void PatchEvaluateJavaScriptAsync(ModuleDefMD module, MethodDef proxyMethod)
        {
            if (proxyMethod == null)
            {
                Console.WriteLine("  WARNING: UI proxy method missing, skip JS hook");
                return;
            }

            var patched = 0;
            foreach (var type in module.GetTypes().Where(t =>
                         t.FullName.StartsWith("CarDemo.Controls.MainWebView", StringComparison.Ordinal)))
            {
                foreach (var method in type.Methods.Where(m => m.HasBody))
                {
                    var instructions = method.Body.Instructions;
                    for (var i = 0; i < instructions.Count; i++)
                    {
                        var instr = instructions[i];
                        if (!IsEvaluateJavaScriptAsyncCall(instr))
                            continue;

                        if (i > 0 && instructions[i - 1].OpCode == OpCodes.Call &&
                            instructions[i - 1].Operand == proxyMethod)
                            continue;

                        instructions.Insert(i, Instruction.Create(OpCodes.Dup));
                        instructions.Insert(i + 1, Instruction.Create(OpCodes.Call, proxyMethod));
                        i += 2;
                        patched++;
                    }
                }
            }

            Console.WriteLine($"  Patched EvaluateJavaScriptAsync calls: {patched}");
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

        private static InjectorPaths ResolvePaths(string[] args)
        {
            var root = FindRepoRoot();
            var assembliesDir = Path.Combine(root, "debug_decompiled", "unknown", "assemblies");

            var carDemoAndroidPath = args.Length > 0
                ? args[0]
                : Path.Combine(assembliesDir, "CarDemo.Android.dll");
            var carDemoPath = args.Length > 1
                ? args[1]
                : Path.Combine(assembliesDir, "CarDemo.dll");
            var webSocketDllPath = args.Length > 2
                ? args[2]
                : Path.Combine(root, "yun", "websocket_layer", "bin", "Release", "netstandard2.0", "OBDCloud.WebSocket.dll");
            var outputDir = args.Length > 3
                ? args[3]
                : Path.Combine(root, "yun", "modified_dlls");

            return new InjectorPaths
            {
                CarDemoAndroidPath = Path.GetFullPath(carDemoAndroidPath),
                CarDemoPath = Path.GetFullPath(carDemoPath),
                WebSocketDllPath = Path.GetFullPath(webSocketDllPath),
                OutputDir = Path.GetFullPath(outputDir)
            };
        }

        private static string FindRepoRoot()
        {
            var baseDir = AppContext.BaseDirectory;
            var candidate = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "..", ".."));
            if (Directory.Exists(Path.Combine(candidate, "debug_decompiled")))
                return candidate;

            var cwd = Directory.GetCurrentDirectory();
            if (Directory.Exists(Path.Combine(cwd, "debug_decompiled")))
                return cwd;

            return candidate;
        }

        private static bool ValidatePaths(InjectorPaths paths)
        {
            var ok = true;

            if (!File.Exists(paths.CarDemoAndroidPath))
            {
                Console.Error.WriteLine($"Missing CarDemo.Android.dll: {paths.CarDemoAndroidPath}");
                ok = false;
            }

            if (!File.Exists(paths.CarDemoPath))
            {
                Console.Error.WriteLine($"Missing CarDemo.dll: {paths.CarDemoPath}");
                ok = false;
            }

            if (!File.Exists(paths.WebSocketDllPath))
            {
                Console.Error.WriteLine($"Missing OBDCloud.WebSocket.dll: {paths.WebSocketDllPath}");
                ok = false;
            }

            return ok;
        }

        private static void InjectBluetoothHooks(ModuleDefMD module, ImportedRefs refs)
        {
            var bluetoothManagerType = module.GetTypes().FirstOrDefault(t => t.Name == "AndroidBluetooth2Manager");
            var jsBridgeType = module.GetTypes().FirstOrDefault(t => t.Name == "JSBridge");

            if (bluetoothManagerType == null)
                throw new InvalidOperationException("AndroidBluetooth2Manager not found.");
            if (jsBridgeType == null)
                throw new InvalidOperationException("JSBridge not found.");

            var useWebSocketField = EnsureField(bluetoothManagerType, "UseWebSocket",
                new FieldSig(module.CorLibTypes.Boolean), FieldAttributes.Public | FieldAttributes.Static);
            var wsServerUrlField = EnsureField(bluetoothManagerType, "WebSocketServerUrl",
                new FieldSig(module.CorLibTypes.String), FieldAttributes.Public | FieldAttributes.Static);
            var wsInitializedField = EnsureField(bluetoothManagerType, "_wsInitialized",
                new FieldSig(module.CorLibTypes.Boolean), FieldAttributes.Private | FieldAttributes.Static);

            EnsureStaticConstructor(bluetoothManagerType, useWebSocketField, wsServerUrlField, wsInitializedField);

            // 注入 JSBridge 初始化逻辑
            PatchJSBridge(jsBridgeType, useWebSocketField, wsServerUrlField, refs);
            PatchJSBridgeConstructor(jsBridgeType, useWebSocketField, wsServerUrlField, refs);
            AddUiCallbackMethod(module, jsBridgeType, refs);

            // 注入蓝牙管理器方法
            PatchBluetoothManager(bluetoothManagerType, useWebSocketField, wsServerUrlField, refs);

            // 查找并注入 BluetoothConnectionV3 方法
            var btConnType = module.GetTypes().FirstOrDefault(t => t.Name == "BluetoothConnectionV3");
            if (btConnType != null)
            {
                PatchBluetoothConnection(btConnType, useWebSocketField, refs);
            }
            else
            {
                Console.WriteLine("  WARNING: BluetoothConnectionV3 not found");
            }

        }

        private static FieldDef EnsureField(TypeDef targetType, string name, FieldSig sig, FieldAttributes attrs)
        {
            var existing = targetType.Fields.FirstOrDefault(f => f.Name == name);
            if (existing != null)
                return existing;

            var field = new FieldDefUser(name, sig, attrs);
            targetType.Fields.Add(field);
            return field;
        }

        private static void EnsureStaticConstructor(TypeDef targetType, FieldDef useWebSocketField,
            FieldDef wsServerUrlField, FieldDef wsInitializedField)
        {
            var cctor = targetType.Methods.FirstOrDefault(m => m.Name == ".cctor");

            if (cctor == null)
            {
                cctor = new MethodDefUser(
                    ".cctor",
                    MethodSig.CreateStatic(targetType.Module.CorLibTypes.Void),
                    MethodAttributes.Private | MethodAttributes.Static |
                    MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName
                )
                {
                    Body = new CilBody()
                };
                cctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
                targetType.Methods.Add(cctor);
            }

            var instructions = cctor.Body.Instructions;
            EnsureFieldInit(instructions, useWebSocketField, new List<Instruction>
            {
                OpCodes.Ldc_I4_1.ToInstruction(),  // true - 默认启用 WebSocket 模式
                OpCodes.Stsfld.ToInstruction(useWebSocketField)
            });

            EnsureFieldInit(instructions, wsServerUrlField, new List<Instruction>
            {
                OpCodes.Ldstr.ToInstruction("ws://10.0.2.2:8080/ws"),  // 10.0.2.2 = 模拟器访问主机
                OpCodes.Stsfld.ToInstruction(wsServerUrlField)
            });

            EnsureFieldInit(instructions, wsInitializedField, new List<Instruction>
            {
                OpCodes.Ldc_I4_0.ToInstruction(),
                OpCodes.Stsfld.ToInstruction(wsInitializedField)
            });

            if (instructions.Count == 0 || instructions.Last().OpCode != OpCodes.Ret)
                instructions.Add(OpCodes.Ret.ToInstruction());
        }

        private static void EnsureFieldInit(IList<Instruction> instructions, FieldDef field, IList<Instruction> init)
        {
            if (instructions.Any(i => i.OpCode == OpCodes.Stsfld && i.Operand == field))
                return;

            var insertIndex = Math.Max(0, instructions.Count - 1);
            foreach (var instr in init)
            {
                instructions.Insert(insertIndex++, instr);
            }
        }

        /// <summary>
        /// 注入 JSBridge 初始化逻辑
        /// 在 callCSharpMethod 开头添加 WebSocket 初始化检查
        /// </summary>
        private static void PatchJSBridge(TypeDef jsBridgeType, FieldDef useWebSocketField,
            FieldDef wsServerUrlField, ImportedRefs refs)
        {
            var callMethod = jsBridgeType.Methods.FirstOrDefault(m => m.Name == "callCSharpMethod");
            if (callMethod == null || !callMethod.HasBody)
            {
                Console.WriteLine("  WARNING: JSBridge.callCSharpMethod not found");
                return;
            }

            var instructions = callMethod.Body.Instructions;
            if (instructions.Count == 0)
                return;

            // 检查是否已注入
            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine("  JSBridge already patched");
                return;
            }

            var originalFirst = instructions[0];

            // 构建注入代码:
            // if (!UseWebSocket) goto originalFirst;
            // if (OBDCloudManager.IsInitialized) goto originalFirst;
            // OBDCloudManager.InitializeIfNeededAsync(WebSocketServerUrl).GetAwaiter().GetResult();
            // OBDCloudManager.Instance.RegisterJSBridge(this);

            var newInstructions = new List<Instruction>();

            // if (!UseWebSocket) goto originalFirst
            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(useWebSocketField));
            newInstructions.Add(OpCodes.Brfalse.ToInstruction(originalFirst));

            // 初始化 WebSocket 连接（即使已初始化也安全）
            if (refs.InitializeIfNeededAsync != null)
            {
                // OBDCloudManager.InitializeIfNeededAsync(WebSocketServerUrl)
                newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsServerUrlField));
                newInstructions.Add(OpCodes.Call.ToInstruction(refs.InitializeIfNeededAsync));
                // 丢弃返回的 Task（fire-and-forget）
                newInstructions.Add(OpCodes.Pop.ToInstruction());
            }

            // 注册 JSBridge: OBDCloudManager.Instance.RegisterJSBridge(this)
            if (refs.GetInstance != null && refs.RegisterJSBridge != null)
            {
                newInstructions.Add(OpCodes.Call.ToInstruction(refs.GetInstance));
                newInstructions.Add(OpCodes.Ldarg_0.ToInstruction()); // this
                newInstructions.Add(OpCodes.Callvirt.ToInstruction(refs.RegisterJSBridge));
            }

            // 插入到方法开头
            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            // 需要重新计算 max stack
            callMethod.Body.KeepOldMaxStack = true;

            Console.WriteLine("  Patched JSBridge.callCSharpMethod");
        }

        /// <summary>
        /// 在 JSBridge 构造函数中注册 JSBridge，避免未注册导致 UI 调用失败
        /// </summary>
        private static void PatchJSBridgeConstructor(TypeDef jsBridgeType, FieldDef useWebSocketField,
            FieldDef wsServerUrlField, ImportedRefs refs)
        {
            var ctors = jsBridgeType.Methods.Where(m => m.Name == ".ctor" && m.HasBody).ToList();
            if (ctors.Count == 0)
            {
                Console.WriteLine("  WARNING: JSBridge constructor not found");
                return;
            }

            foreach (var ctor in ctors)
            {
                var instructions = ctor.Body.Instructions;
                if (instructions.Count == 0)
                    continue;

                // 已注入则跳过
                if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
                {
                    Console.WriteLine("  JSBridge ctor already patched");
                    continue;
                }

                // 找到 base .ctor 调用后的位置
                var insertIndex = 0;
                for (var i = 0; i < instructions.Count; i++)
                {
                    if (instructions[i].OpCode == OpCodes.Call && instructions[i].Operand is IMethod method &&
                        method.Name == ".ctor")
                    {
                        insertIndex = i + 1;
                        break;
                    }
                }

                var originalTarget = instructions[Math.Min(insertIndex, instructions.Count - 1)];
                var newInstructions = new List<Instruction>();

                // if (!UseWebSocket) goto originalTarget;
                newInstructions.Add(OpCodes.Ldsfld.ToInstruction(useWebSocketField));
                newInstructions.Add(OpCodes.Brfalse.ToInstruction(originalTarget));

                // 初始化 WebSocket 连接（即使已初始化也安全）
                if (refs.InitializeIfNeededAsync != null)
                {
                    newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsServerUrlField));
                    newInstructions.Add(OpCodes.Call.ToInstruction(refs.InitializeIfNeededAsync));
                    newInstructions.Add(OpCodes.Pop.ToInstruction());
                }

                // 注册 JSBridge: OBDCloudManager.Instance.RegisterJSBridge(this)
                if (refs.GetInstance != null && refs.RegisterJSBridge != null)
                {
                    newInstructions.Add(OpCodes.Call.ToInstruction(refs.GetInstance));
                    newInstructions.Add(OpCodes.Ldarg_0.ToInstruction());
                    newInstructions.Add(OpCodes.Callvirt.ToInstruction(refs.RegisterJSBridge));
                }

                for (var i = newInstructions.Count - 1; i >= 0; i--)
                {
                    instructions.Insert(insertIndex, newInstructions[i]);
                }

                ctor.Body.KeepOldMaxStack = true;
                Console.WriteLine("  Patched JSBridge .ctor");
            }
        }

        /// <summary>
        /// 向 JSBridge C# 类添加 uiCallback(string name, string data) 方法。
        /// JS 层（React UI）调用 window.jsBridge.uiCallback(name, json) 时，
        /// 此方法将数据转发给 OBDCloudManager.HandleUiInvoke → WebSocket → B 端。
        /// </summary>
        private static void AddUiCallbackMethod(ModuleDefMD module, TypeDef jsBridgeType, ImportedRefs refs)
        {
            if (jsBridgeType.Methods.Any(m => m.Name == "uiCallback"))
            {
                Console.WriteLine("  uiCallback already exists in JSBridge, skip");
                return;
            }

            if (refs.HandleUiInvoke == null)
            {
                Console.WriteLine("  WARNING: HandleUiInvoke ref not found, cannot add uiCallback");
                return;
            }

            // 方法签名: public void uiCallback(string name, string data)
            var sig = MethodSig.CreateInstance(
                module.CorLibTypes.Void,
                module.CorLibTypes.String,
                module.CorLibTypes.String);

            var method = new MethodDefUser(
                "uiCallback",
                sig,
                MethodAttributes.Public | MethodAttributes.HideBySig);

            var body = new CilBody();
            body.InitLocals = true;
            body.KeepOldMaxStack = true;

            // IL：
            // ldarg.1          // name (string)
            // ldc.i4.1         // array length = 1
            // newarr object    // new object[1]
            // dup
            // ldc.i4.0         // index 0
            // ldarg.2          // data (string)
            // stelem.ref       // array[0] = data
            // call OBDCloudManager.HandleUiInvoke(string, object[])
            // ret
            body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_1));
            body.Instructions.Add(Instruction.Create(OpCodes.Newarr,
                (ITypeDefOrRef)module.CorLibTypes.Object.ToTypeDefOrRef()));
            body.Instructions.Add(Instruction.Create(OpCodes.Dup));
            body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_2));
            body.Instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            body.Instructions.Add(Instruction.Create(OpCodes.Call, refs.HandleUiInvoke));
            body.Instructions.Add(Instruction.Create(OpCodes.Ret));

            method.Body = body;
            jsBridgeType.Methods.Add(method);
            Console.WriteLine("  Added uiCallback(string, string) to JSBridge → calls OBDCloudManager.HandleUiInvoke");
        }

        /// <summary>
        /// 修改 AndroidBluetooth2Manager 的扫描方法和状态属性
        /// </summary>
        private static void PatchBluetoothManager(TypeDef bluetoothManagerType, FieldDef useWebSocketField,
            FieldDef wsServerUrlField, ImportedRefs refs)
        {
            // 修改 IsOn 属性 - 让蓝牙状态显示为已开启
            var isOnProp = bluetoothManagerType.Properties.FirstOrDefault(p => p.Name == "IsOn");
            if (isOnProp?.GetMethod?.Body != null)
            {
                PatchBoolPropertyToReturnTrue(isOnProp.GetMethod, useWebSocketField, "IsOn");
            }

            // 修改 IsAvailable 属性 - 让蓝牙状态显示为可用
            var isAvailableProp = bluetoothManagerType.Properties.FirstOrDefault(p => p.Name == "IsAvailable");
            if (isAvailableProp?.GetMethod?.Body != null)
            {
                PatchBoolPropertyToReturnTrue(isAvailableProp.GetMethod, useWebSocketField, "IsAvailable");
            }

            // 修改 StartDiscoveringDevices
            var startMethod = bluetoothManagerType.Methods.FirstOrDefault(
                m => m.Name == "StartDiscoveringDevices" && !m.Name.Contains("<"));
            if (startMethod?.Body != null)
            {
                PatchAsyncMethodWithCall(startMethod, useWebSocketField, refs.GetInstance, refs.StartScanAsync,
                    "StartDiscoveringDevices", wsServerUrlField, refs.GetIsInitialized, refs.InitializeIfNeededAsync);
            }

            // 修改 StopDiscoveringDevices
            var stopMethod = bluetoothManagerType.Methods.FirstOrDefault(
                m => m.Name == "StopDiscoveringDevices" && !m.Name.Contains("<"));
            if (stopMethod?.Body != null)
            {
                PatchAsyncMethodWithCall(stopMethod, useWebSocketField, refs.GetInstance, refs.StopScanAsync,
                    "StopDiscoveringDevices", wsServerUrlField, refs.GetIsInitialized, refs.InitializeIfNeededAsync);
            }
        }

        /// <summary>
        /// 修补布尔属性的 getter，让它在 WebSocket 模式下返回 true
        /// </summary>
        private static void PatchBoolPropertyToReturnTrue(MethodDef method, FieldDef useWebSocketField, string propName)
        {
            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine($"  {propName} getter already patched");
                return;
            }

            var originalFirst = instructions[0];

            var newInstructions = new List<Instruction>
            {
                // if (!UseWebSocket) goto originalFirst
                OpCodes.Ldsfld.ToInstruction(useWebSocketField),
                OpCodes.Brfalse.ToInstruction(originalFirst),

                // return true
                OpCodes.Ldc_I4_1.ToInstruction(),
                OpCodes.Ret.ToInstruction()
            };

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            method.Body.KeepOldMaxStack = true;

            Console.WriteLine($"  Patched AndroidBluetooth2Manager.{propName} getter");
        }

        /// <summary>
        /// 修改 BluetoothConnectionV3 的连接和读写方法
        /// </summary>
        private static void PatchBluetoothConnection(TypeDef btConnType, FieldDef useWebSocketField, ImportedRefs refs)
        {
            // 修改 Connected 属性 getter - 让 Cloud A 认为蓝牙已连接
            var connectedProp = btConnType.Properties.FirstOrDefault(p => p.Name == "Connected");
            if (connectedProp?.GetMethod?.Body != null && refs.GetOBDConnectionIsConnected != null)
            {
                PatchConnectedPropertyGetter(connectedProp.GetMethod, useWebSocketField, refs);
            }
            else
            {
                Console.WriteLine("  WARNING: BluetoothConnectionV3.Connected property not found or cannot be patched");
            }

            // 修改 ConnectAsync
            var connectMethod = btConnType.Methods.FirstOrDefault(
                m => m.Name == "ConnectAsync" && !m.Name.Contains("<"));
            if (connectMethod?.Body != null && refs.ConnectToDeviceAsync != null)
            {
                PatchConnectAsync(connectMethod, useWebSocketField, refs);
            }

            // 修改 WriteBytesAsync
            var writeMethod = btConnType.Methods.FirstOrDefault(
                m => m.Name == "WriteBytesAsync" && !m.Name.Contains("<"));
            if (writeMethod?.Body != null && refs.SendOBDCommandAsyncBytes != null)
            {
                PatchWriteBytesAsync(writeMethod, useWebSocketField, refs);
            }

            // 修改 ReadBytesAsync
            var readMethod = btConnType.Methods.FirstOrDefault(
                m => m.Name == "ReadBytesAsync" && !m.Name.Contains("<"));
            if (readMethod?.Body != null && refs.ReadNextDataAsync != null)
            {
                PatchReadBytesAsync(readMethod, useWebSocketField, refs);
            }
        }

        /// <summary>
        /// 修补 Connected 属性的 getter
        /// 原始逻辑: return device != null && device.State == 2
        /// 修改为: if (UseWebSocket) return true; 原始逻辑
        /// 在 WebSocket 模式下，Cloud A 始终认为蓝牙已连接
        /// 实际的数据传输由 WebSocket → Phone B → 蓝牙 完成
        /// </summary>
        private static void PatchConnectedPropertyGetter(MethodDef method, FieldDef useWebSocketField, ImportedRefs refs)
        {
            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine("  Connected getter already patched");
                return;
            }

            var originalFirst = instructions[0];

            var newInstructions = new List<Instruction>
            {
                // if (!UseWebSocket) goto originalFirst
                OpCodes.Ldsfld.ToInstruction(useWebSocketField),
                OpCodes.Brfalse.ToInstruction(originalFirst),

                // return true  (WebSocket 模式下始终返回已连接)
                OpCodes.Ldc_I4_1.ToInstruction(),
                OpCodes.Ret.ToInstruction()
            };

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            method.Body.KeepOldMaxStack = true;

            Console.WriteLine("  Patched BluetoothConnectionV3.Connected getter -> always true");
        }

        /// <summary>
        /// 通用的异步方法修补：在开头插入 WebSocket 分支
        /// 包含初始化检查：如果 OBDCloudManager 未初始化，先进行初始化
        /// </summary>
        private static void PatchAsyncMethodWithCall(MethodDef method, FieldDef useWebSocketField,
            IMethod getInstance, IMethod targetMethod, string methodName,
            FieldDef wsServerUrlField = null, IMethod getIsInitialized = null, IMethod initializeIfNeededAsync = null)
        {
            if (getInstance == null || targetMethod == null)
            {
                Console.WriteLine($"  WARNING: Cannot patch {methodName} - missing method references");
                return;
            }

            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            // 检查是否已注入
            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine($"  {methodName} already patched");
                return;
            }

            var originalFirst = instructions[0];

            var newInstructions = new List<Instruction>();

            // if (!UseWebSocket) goto originalFirst
            newInstructions.Add(OpCodes.Ldsfld.ToInstruction(useWebSocketField));
            newInstructions.Add(OpCodes.Brfalse.ToInstruction(originalFirst));

            // 如果有初始化方法引用，添加初始化检查
            if (getIsInitialized != null && initializeIfNeededAsync != null && wsServerUrlField != null)
            {
                // 创建一个标签用于跳过初始化
                var callMethodLabel = OpCodes.Call.ToInstruction(getInstance);

                // if (OBDCloudManager.IsInitialized) goto callMethod
                newInstructions.Add(OpCodes.Call.ToInstruction(getIsInitialized));
                newInstructions.Add(OpCodes.Brtrue.ToInstruction(callMethodLabel));

                // OBDCloudManager.InitializeIfNeededAsync(WebSocketServerUrl)
                newInstructions.Add(OpCodes.Ldsfld.ToInstruction(wsServerUrlField));
                newInstructions.Add(OpCodes.Call.ToInstruction(initializeIfNeededAsync));
                // 丢弃返回的 Task（fire-and-forget 初始化）
                newInstructions.Add(OpCodes.Pop.ToInstruction());

                // callMethod: return OBDCloudManager.Instance.TargetMethodAsync()
                newInstructions.Add(callMethodLabel);  // 这个指令已经是 Call getInstance
            }
            else
            {
                // 没有初始化检查，直接调用
                newInstructions.Add(OpCodes.Call.ToInstruction(getInstance));
            }

            newInstructions.Add(OpCodes.Callvirt.ToInstruction(targetMethod));
            newInstructions.Add(OpCodes.Ret.ToInstruction());

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            method.Body.KeepOldMaxStack = true;

            Console.WriteLine($"  Patched {methodName}");
        }

        /// <summary>
        /// 修补 ConnectAsync 方法
        /// 原始签名: Task<bool> ConnectAsync(string device_id, Stream debugStream)
        /// </summary>
        private static void PatchConnectAsync(MethodDef method, FieldDef useWebSocketField, ImportedRefs refs)
        {
            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine("  ConnectAsync already patched");
                return;
            }

            var originalFirst = instructions[0];

            var newInstructions = new List<Instruction>
            {
                // if (!UseWebSocket) goto originalFirst
                OpCodes.Ldsfld.ToInstruction(useWebSocketField),
                OpCodes.Brfalse.ToInstruction(originalFirst),

                // return OBDCloudManager.Instance.ConnectToDeviceAsync("bt", device_id)
                OpCodes.Call.ToInstruction(refs.GetInstance),
                OpCodes.Ldstr.ToInstruction("bt"), // protocol = "bt"
                OpCodes.Ldarg_1.ToInstruction(),   // device_id (第一个参数)
                OpCodes.Callvirt.ToInstruction(refs.ConnectToDeviceAsync),
                OpCodes.Ret.ToInstruction()
            };

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            method.Body.KeepOldMaxStack = true;

            Console.WriteLine("  Patched ConnectAsync");
        }

        /// <summary>
        /// 修补 WriteBytesAsync 方法
        /// 原始签名: Task WriteBytesAsync(byte[] data)
        /// </summary>
        private static void PatchWriteBytesAsync(MethodDef method, FieldDef useWebSocketField, ImportedRefs refs)
        {
            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine("  WriteBytesAsync already patched");
                return;
            }

            var originalFirst = instructions[0];

            var newInstructions = new List<Instruction>
            {
                // if (!UseWebSocket) goto originalFirst
                OpCodes.Ldsfld.ToInstruction(useWebSocketField),
                OpCodes.Brfalse.ToInstruction(originalFirst),

                // return OBDCloudManager.Instance.SendOBDCommandAsync(data)
                OpCodes.Call.ToInstruction(refs.GetInstance),
                OpCodes.Ldarg_1.ToInstruction(), // data (第一个参数)
                OpCodes.Callvirt.ToInstruction(refs.SendOBDCommandAsyncBytes),
                OpCodes.Ret.ToInstruction()
            };

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            method.Body.KeepOldMaxStack = true;

            Console.WriteLine("  Patched WriteBytesAsync");
        }

        /// <summary>
        /// 修补 ReadBytesAsync 方法
        /// 原始签名: ValueTask<byte[]> ReadBytesAsync()
        /// 需要将 Task<byte[]> 转换为 ValueTask<byte[]>
        /// </summary>
        private static void PatchReadBytesAsync(MethodDef method, FieldDef useWebSocketField, ImportedRefs refs)
        {
            var instructions = method.Body.Instructions;
            if (instructions.Count == 0)
                return;

            if (instructions.Any(i => i.OpCode == OpCodes.Ldsfld && i.Operand == useWebSocketField))
            {
                Console.WriteLine("  ReadBytesAsync already patched");
                return;
            }

            var originalFirst = instructions[0];
            var module = method.Module;

            // 检查返回类型是否是 ValueTask<byte[]>
            var returnTypeName = method.ReturnType.FullName;
            var isValueTask = returnTypeName.Contains("ValueTask");

            // 如果是 ValueTask，需要找到构造函数
            IMethod valueTaskCtor = null;
            if (isValueTask)
            {
                valueTaskCtor = FindValueTaskCtor(module, method.ReturnType);
                if (valueTaskCtor == null)
                {
                    Console.WriteLine("  WARNING: ValueTask<byte[]> constructor not found, skipping ReadBytesAsync patch");
                    return;
                }
            }

            var newInstructions = new List<Instruction>
            {
                // if (!UseWebSocket) goto originalFirst
                OpCodes.Ldsfld.ToInstruction(useWebSocketField),
                OpCodes.Brfalse.ToInstruction(originalFirst),

                // var conn = OBDCloudManager.Instance.OBDConnection
                OpCodes.Call.ToInstruction(refs.GetInstance),
                OpCodes.Callvirt.ToInstruction(refs.GetOBDConnection),

                // var task = conn.ReadNextDataAsync()
                OpCodes.Callvirt.ToInstruction(refs.ReadNextDataAsync)
            };

            if (isValueTask && valueTaskCtor != null)
            {
                // return new ValueTask<byte[]>(task)
                newInstructions.Add(OpCodes.Newobj.ToInstruction(valueTaskCtor));
            }

            newInstructions.Add(OpCodes.Ret.ToInstruction());

            for (var i = newInstructions.Count - 1; i >= 0; i--)
            {
                instructions.Insert(0, newInstructions[i]);
            }

            // 需要重新计算 max stack
            method.Body.KeepOldMaxStack = true;

            Console.WriteLine("  Patched ReadBytesAsync");
        }

        /// <summary>
        /// 查找 ValueTask<byte[]> 的构造函数
        /// </summary>
        private static IMethod FindValueTaskCtor(ModuleDef module, TypeSig returnType)
        {
            // 从返回类型获取 ValueTask<byte[]> 的信息
            if (returnType is GenericInstSig genericInst)
            {
                var baseType = genericInst.GenericType;
                if (baseType?.TypeDefOrRef != null)
                {
                    try
                    {
                        // 创建 Task<byte[]> 类型签名
                        var byteArrayType = new SZArraySig(module.CorLibTypes.Byte);
                        var taskOfByteArray = new GenericInstSig(
                            new ClassSig(new TypeRefUser(module, "System.Threading.Tasks", "Task`1",
                                module.CorLibTypes.AssemblyRef)),
                            byteArrayType);

                        // 构造函数签名: void .ctor(Task<byte[]>)
                        var ctorSig = MethodSig.CreateInstance(module.CorLibTypes.Void, taskOfByteArray);
                        var ctorRef = new MemberRefUser(module, ".ctor", ctorSig, baseType.TypeDefOrRef);

                        return module.Import(ctorRef);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  Error creating ValueTask ctor ref: {ex.Message}");
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 修复类型引用：将指向外部程序集的 TypeRef 替换为本地 TypeDef
        /// 这是必要的，因为 Importer 在复制字段类型时会创建指向原程序集的 TypeRef
        /// </summary>
        private static void FixTypeReferences(ModuleDefMD module, string sourceAssemblyName)
        {
            // 构建从 FullName 到 TypeDef 的映射
            var typeDefMap = new Dictionary<string, TypeDef>();
            foreach (var type in module.GetTypes())
            {
                if (!type.IsGlobalModuleType)
                    typeDefMap[type.FullName] = type;
            }

            int fixedCount = 0;

            // 遍历所有类型，修复字段、方法参数和返回类型中的引用
            foreach (var type in module.GetTypes().ToList())
            {
                // 修复字段类型
                foreach (var field in type.Fields)
                {
                    var newSig = FixTypeSig(field.FieldSig?.Type, typeDefMap, sourceAssemblyName);
                    if (newSig != null && newSig != field.FieldSig?.Type)
                    {
                        field.FieldSig = new FieldSig(newSig);
                        fixedCount++;
                        Console.WriteLine($"    Fixed field: {type.Name}.{field.Name}");
                    }
                }

                // 修复方法签名
                foreach (var method in type.Methods)
                {
                    if (method.MethodSig == null) continue;

                    bool modified = false;

                    // 修复返回类型
                    var newRetType = FixTypeSig(method.MethodSig.RetType, typeDefMap, sourceAssemblyName);
                    if (newRetType != null && newRetType != method.MethodSig.RetType)
                    {
                        method.MethodSig.RetType = newRetType;
                        modified = true;
                    }

                    // 修复参数类型
                    for (int i = 0; i < method.MethodSig.Params.Count; i++)
                    {
                        var newParamType = FixTypeSig(method.MethodSig.Params[i], typeDefMap, sourceAssemblyName);
                        if (newParamType != null && newParamType != method.MethodSig.Params[i])
                        {
                            method.MethodSig.Params[i] = newParamType;
                            modified = true;
                        }
                    }

                    if (modified)
                    {
                        fixedCount++;
                        Console.WriteLine($"    Fixed method sig: {type.Name}.{method.Name}");
                    }

                    // 修复方法体中的指令操作数
                    if (method.HasBody)
                    {
                        foreach (var instr in method.Body.Instructions)
                        {
                            FixInstructionOperand(instr, typeDefMap, sourceAssemblyName, module);
                        }

                        // 修复局部变量类型
                        foreach (var local in method.Body.Variables)
                        {
                            var newLocalType = FixTypeSig(local.Type, typeDefMap, sourceAssemblyName);
                            if (newLocalType != null && newLocalType != local.Type)
                            {
                                local.Type = newLocalType;
                            }
                        }
                    }
                }

                // 修复属性签名
                foreach (var prop in type.Properties)
                {
                    if (prop.PropertySig == null) continue;

                    var newRetType = FixTypeSig(prop.PropertySig.RetType, typeDefMap, sourceAssemblyName);
                    if (newRetType != null && newRetType != prop.PropertySig.RetType)
                    {
                        prop.PropertySig.RetType = newRetType;
                        fixedCount++;
                    }
                }

                // 修复事件类型
                foreach (var evt in type.Events)
                {
                    if (evt.EventType != null && IsFromAssembly(evt.EventType, sourceAssemblyName))
                    {
                        var fullName = evt.EventType.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            evt.EventType = localType;
                            fixedCount++;
                        }
                    }
                }

                // 修复基类引用
                if (type.BaseType != null && IsFromAssembly(type.BaseType, sourceAssemblyName))
                {
                    var fullName = type.BaseType.FullName;
                    if (typeDefMap.TryGetValue(fullName, out var localType))
                    {
                        type.BaseType = localType;
                        fixedCount++;
                        Console.WriteLine($"    Fixed base type: {type.Name} -> {localType.Name}");
                    }
                }

                // 修复接口实现
                for (int i = 0; i < type.Interfaces.Count; i++)
                {
                    var iface = type.Interfaces[i];
                    if (IsFromAssembly(iface.Interface, sourceAssemblyName))
                    {
                        var fullName = iface.Interface.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            type.Interfaces[i] = new InterfaceImplUser(localType);
                            fixedCount++;
                            Console.WriteLine($"    Fixed interface: {type.Name} implements {localType.Name}");
                        }
                    }
                }
            }

            // 第二遍：修复异常处理子句中的类型引用
            foreach (var type in module.GetTypes().ToList())
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody) continue;

                    foreach (var exHandler in method.Body.ExceptionHandlers)
                    {
                        if (exHandler.CatchType != null && IsFromAssembly(exHandler.CatchType, sourceAssemblyName))
                        {
                            var fullName = exHandler.CatchType.FullName;
                            if (typeDefMap.TryGetValue(fullName, out var localType))
                            {
                                exHandler.CatchType = localType;
                                fixedCount++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"    Fixed {fixedCount} type references");

            // 第三遍：修复所有 TypeRef 的 ResolutionScope
            // 即使这些 TypeRef 没有被直接使用，它们仍然存在于元数据表中
            // 这会导致 dnlib 在写入时包含对 OBDCloud.WebSocket 的程序集引用
            Console.WriteLine("    Fixing orphaned TypeRefs...");
            int orphanFixed = 0;
            var typeRefCache = new Dictionary<TypeDef, TypeRef>();
            foreach (var typeRef in module.GetTypeRefs().ToList())
            {
                if (IsFromAssembly(typeRef, sourceAssemblyName))
                {
                    var fullName = typeRef.FullName;
                    if (typeDefMap.TryGetValue(fullName, out var localType))
                    {
                        // 将 TypeRef 的 ResolutionScope 修改为当前模块或本地父类型
                        // 避免嵌套类型被错误解析为顶层类型
                        SetTypeRefResolutionScope(typeRef, localType, module, typeRefCache);
                        orphanFixed++;
                        Console.WriteLine($"      Fixed orphan TypeRef: {fullName}");
                    }
                }
            }
            Console.WriteLine($"    Fixed {orphanFixed} orphaned TypeRefs");
        }

        /// <summary>
        /// 修复类型签名，将指向外部程序集的引用替换为本地 TypeDef
        /// </summary>
        private static TypeSig FixTypeSig(TypeSig typeSig, Dictionary<string, TypeDef> typeDefMap, string sourceAssemblyName)
        {
            if (typeSig == null) return null;

            // 处理各种类型签名
            switch (typeSig)
            {
                case ClassSig classSig:
                    if (IsFromAssembly(classSig.TypeDefOrRef, sourceAssemblyName))
                    {
                        var fullName = classSig.TypeDefOrRef.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            return new ClassSig(localType);
                        }
                    }
                    break;

                case ValueTypeSig valueTypeSig:
                    if (IsFromAssembly(valueTypeSig.TypeDefOrRef, sourceAssemblyName))
                    {
                        var fullName = valueTypeSig.TypeDefOrRef.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            return new ValueTypeSig(localType);
                        }
                    }
                    break;

                case GenericInstSig genericInstSig:
                    // 修复泛型类型参数
                    bool genericModified = false;
                    var newGenericArgs = new List<TypeSig>();
                    foreach (var arg in genericInstSig.GenericArguments)
                    {
                        var newArg = FixTypeSig(arg, typeDefMap, sourceAssemblyName);
                        if (newArg != null && newArg != arg)
                        {
                            newGenericArgs.Add(newArg);
                            genericModified = true;
                        }
                        else
                        {
                            newGenericArgs.Add(arg);
                        }
                    }

                    // 修复泛型类型本身
                    var genericType = genericInstSig.GenericType;
                    ClassOrValueTypeSig newGenericType = genericType;
                    if (IsFromAssembly(genericType?.TypeDefOrRef, sourceAssemblyName))
                    {
                        var fullName = genericType.TypeDefOrRef.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            newGenericType = localType.IsValueType
                                ? (ClassOrValueTypeSig)new ValueTypeSig(localType)
                                : new ClassSig(localType);
                            genericModified = true;
                        }
                    }

                    if (genericModified)
                    {
                        return new GenericInstSig(newGenericType, newGenericArgs);
                    }
                    break;

                case SZArraySig szArraySig:
                    var newElemType = FixTypeSig(szArraySig.Next, typeDefMap, sourceAssemblyName);
                    if (newElemType != null && newElemType != szArraySig.Next)
                    {
                        return new SZArraySig(newElemType);
                    }
                    break;

                case ByRefSig byRefSig:
                    var newByRefType = FixTypeSig(byRefSig.Next, typeDefMap, sourceAssemblyName);
                    if (newByRefType != null && newByRefType != byRefSig.Next)
                    {
                        return new ByRefSig(newByRefType);
                    }
                    break;

                case PtrSig ptrSig:
                    var newPtrType = FixTypeSig(ptrSig.Next, typeDefMap, sourceAssemblyName);
                    if (newPtrType != null && newPtrType != ptrSig.Next)
                    {
                        return new PtrSig(newPtrType);
                    }
                    break;
            }

            return typeSig;
        }

        /// <summary>
        /// 修复指令操作数中的类型引用
        /// </summary>
        private static void FixInstructionOperand(Instruction instr, Dictionary<string, TypeDef> typeDefMap,
            string sourceAssemblyName, ModuleDef module)
        {
            if (instr.Operand == null) return;

            // 修复类型引用
            if (instr.Operand is ITypeDefOrRef typeRef && IsFromAssembly(typeRef, sourceAssemblyName))
            {
                var fullName = typeRef.FullName;
                if (typeDefMap.TryGetValue(fullName, out var localType))
                {
                    instr.Operand = localType;
                }
            }

            // 修复 TypeSpec（泛型类型实例）
            if (instr.Operand is TypeSpec typeSpec)
            {
                var fixed_sig = FixTypeSig(typeSpec.TypeSig, typeDefMap, sourceAssemblyName);
                if (fixed_sig != null && fixed_sig != typeSpec.TypeSig)
                {
                    instr.Operand = new TypeSpecUser(fixed_sig);
                }
            }

            // 修复方法引用 - MemberRef
            if (instr.Operand is MemberRef memberRef)
            {
                var declType = memberRef.DeclaringType;

                // 处理 TypeSpec（泛型类型）的情况
                if (declType is TypeSpec ts)
                {
                    var fixed_ts = FixTypeSigForTypeSpec(ts, typeDefMap, sourceAssemblyName, module);
                    if (fixed_ts != null)
                    {
                        // 创建新的 MemberRef 指向修复后的类型
                        if (memberRef.IsMethodRef && memberRef.MethodSig != null)
                        {
                            var newMemberRef = new MemberRefUser(module, memberRef.Name, memberRef.MethodSig, fixed_ts);
                            instr.Operand = newMemberRef;
                        }
                        else if (memberRef.IsFieldRef && memberRef.FieldSig != null)
                        {
                            var newMemberRef = new MemberRefUser(module, memberRef.Name, memberRef.FieldSig, fixed_ts);
                            instr.Operand = newMemberRef;
                        }
                        return;
                    }
                }

                if (IsFromAssembly(declType, sourceAssemblyName))
                {
                    var fullName = declType.FullName;
                    if (typeDefMap.TryGetValue(fullName, out var localType))
                    {
                        // 找到本地类型中对应的方法
                        var localMethod = localType.Methods.FirstOrDefault(m =>
                            m.Name == memberRef.Name &&
                            MatchMethodSig(m.MethodSig, memberRef.MethodSig));

                        if (localMethod != null)
                        {
                            instr.Operand = localMethod;
                        }
                        else
                        {
                            // 找到本地类型中对应的字段
                            var localField = localType.Fields.FirstOrDefault(f => f.Name == memberRef.Name);
                            if (localField != null)
                            {
                                instr.Operand = localField;
                            }
                        }
                    }
                }
            }

            // 修复字段引用 - FieldDef 已经是本地的，不需要修复

            // 修复方法规范（泛型方法调用）
            if (instr.Operand is MethodSpec methodSpec)
            {
                bool modified = false;
                IMethod newMethod = methodSpec.Method;
                GenericInstMethodSig newGenericSig = methodSpec.GenericInstMethodSig;

                // 修复泛型方法签名中的类型参数
                if (methodSpec.GenericInstMethodSig != null)
                {
                    var newArgs = new List<TypeSig>();
                    foreach (var arg in methodSpec.GenericInstMethodSig.GenericArguments)
                    {
                        var newArg = FixTypeSig(arg, typeDefMap, sourceAssemblyName);
                        if (newArg != null && newArg != arg)
                        {
                            newArgs.Add(newArg);
                            modified = true;
                        }
                        else
                        {
                            newArgs.Add(arg);
                        }
                    }
                    if (modified)
                    {
                        newGenericSig = new GenericInstMethodSig(newArgs.ToArray());
                    }
                }

                // 修复方法本身
                if (methodSpec.Method is MemberRef specMemberRef)
                {
                    var declType = specMemberRef.DeclaringType;

                    // 处理 TypeSpec（泛型类型）的情况
                    if (declType is TypeSpec ts)
                    {
                        var fixed_ts = FixTypeSigForTypeSpec(ts, typeDefMap, sourceAssemblyName, module);
                        if (fixed_ts != null && specMemberRef.MethodSig != null)
                        {
                            var newMemberRef = new MemberRefUser(module, specMemberRef.Name, specMemberRef.MethodSig, fixed_ts);
                            newMethod = newMemberRef;
                            modified = true;
                        }
                    }
                    else if (IsFromAssembly(declType, sourceAssemblyName))
                    {
                        var fullName = declType.FullName;
                        if (typeDefMap.TryGetValue(fullName, out var localType))
                        {
                            var localMethod = localType.Methods.FirstOrDefault(m =>
                                m.Name == specMemberRef.Name &&
                                m.GenericParameters.Count > 0);

                            if (localMethod != null)
                            {
                                newMethod = localMethod;
                                modified = true;
                            }
                        }
                    }
                }

                if (modified && newMethod is IMethodDefOrRef methodDefOrRef)
                {
                    instr.Operand = new MethodSpecUser(methodDefOrRef, newGenericSig);
                }
            }
        }

        /// <summary>
        /// 修复 TypeSpec 中的类型签名
        /// </summary>
        private static TypeSpec FixTypeSigForTypeSpec(TypeSpec ts, Dictionary<string, TypeDef> typeDefMap,
            string sourceAssemblyName, ModuleDef module)
        {
            var fixed_sig = FixTypeSig(ts.TypeSig, typeDefMap, sourceAssemblyName);
            if (fixed_sig != null && fixed_sig != ts.TypeSig)
            {
                return new TypeSpecUser(fixed_sig);
            }
            return null;
        }

        /// <summary>
        /// 检查类型是否来自指定的程序集
        /// </summary>
        private static bool IsFromAssembly(ITypeDefOrRef typeRef, string assemblyName)
        {
            if (typeRef == null) return false;

            if (typeRef is TypeRef tr)
            {
                var scope = tr.ResolutionScope;
                if (scope is AssemblyRef asmRef)
                {
                    return asmRef.Name == assemblyName;
                }
                if (scope is TypeRef parentRef)
                {
                    return IsFromAssembly(parentRef, assemblyName);
                }
            }

            // 处理 TypeSpec（泛型类型实例化）
            if (typeRef is TypeSpec ts)
            {
                return IsTypeSigFromAssembly(ts.TypeSig, assemblyName);
            }

            return false;
        }

        /// <summary>
        /// 检查类型签名是否引用了指定的程序集
        /// </summary>
        private static bool IsTypeSigFromAssembly(TypeSig typeSig, string assemblyName)
        {
            if (typeSig == null) return false;

            switch (typeSig)
            {
                case ClassSig classSig:
                    return IsFromAssembly(classSig.TypeDefOrRef, assemblyName);

                case ValueTypeSig valueTypeSig:
                    return IsFromAssembly(valueTypeSig.TypeDefOrRef, assemblyName);

                case GenericInstSig genericInstSig:
                    // 检查泛型类型本身
                    if (genericInstSig.GenericType != null &&
                        IsFromAssembly(genericInstSig.GenericType.TypeDefOrRef, assemblyName))
                    {
                        return true;
                    }
                    // 检查泛型参数
                    foreach (var arg in genericInstSig.GenericArguments)
                    {
                        if (IsTypeSigFromAssembly(arg, assemblyName))
                            return true;
                    }
                    break;

                case SZArraySig szArraySig:
                    return IsTypeSigFromAssembly(szArraySig.Next, assemblyName);

                case ByRefSig byRefSig:
                    return IsTypeSigFromAssembly(byRefSig.Next, assemblyName);

                case PtrSig ptrSig:
                    return IsTypeSigFromAssembly(ptrSig.Next, assemblyName);
            }

            return false;
        }

        /// <summary>
        /// 比较两个方法签名是否匹配
        /// </summary>
        private static bool MatchMethodSig(MethodSig sig1, MethodSig sig2)
        {
            if (sig1 == null || sig2 == null) return false;
            if (sig1.Params.Count != sig2.Params.Count) return false;

            var ret1 = sig1.RetType?.FullName;
            var ret2 = sig2.RetType?.FullName;
            if (!string.Equals(ret1, ret2, StringComparison.Ordinal))
                return false;

            for (int i = 0; i < sig1.Params.Count; i++)
            {
                var p1 = sig1.Params[i]?.FullName;
                var p2 = sig2.Params[i]?.FullName;
                if (!string.Equals(p1, p2, StringComparison.Ordinal))
                    return false;
            }

            if (sig1.ParamsAfterSentinel != null || sig2.ParamsAfterSentinel != null)
            {
                var count1 = sig1.ParamsAfterSentinel?.Count ?? 0;
                var count2 = sig2.ParamsAfterSentinel?.Count ?? 0;
                if (count1 != count2)
                    return false;

                for (int i = 0; i < count1; i++)
                {
                    var p1 = sig1.ParamsAfterSentinel[i]?.FullName;
                    var p2 = sig2.ParamsAfterSentinel[i]?.FullName;
                    if (!string.Equals(p1, p2, StringComparison.Ordinal))
                        return false;
                }
            }

            return sig1.HasThis == sig2.HasThis;
        }

        private static void SaveModule(ModuleDefMD module, string outputPath)
        {
            var options = new ModuleWriterOptions(module)
            {
                WritePdb = false
            };

            // 设置全局选项以保留旧的 max stack 值
            options.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;

            module.Write(outputPath, options);
            var fileInfo = new FileInfo(outputPath);
            Console.WriteLine($"Saved: {outputPath} ({fileInfo.Length / 1024} KB)");
        }

        /// <summary>
        /// 创建一个占位 DLL，包含类型转发器将所有 OBDCloud.WebSocket 类型转发到 CarDemo.Android
        /// </summary>
        private static void CreateEmptyPlaceholderDll(string outputDir, string assemblyName)
        {
            var module = new ModuleDefUser(assemblyName + ".dll");
            module.Kind = ModuleKind.Dll;

            // 使用版本 0.0.0.0 以匹配 CarDemo.Android.dll 中的 AssemblyRef
            var assembly = new AssemblyDefUser(assemblyName, new Version(0, 0, 0, 0));
            assembly.Modules.Add(module);

            // 添加对 CarDemo.Android 的程序集引用
            var targetAsmRef = new AssemblyRefUser("CarDemo.Android", new Version(1, 0, 0, 0));

            // 需要转发的类型列表
            var typesToForward = new[]
            {
                "OBDCloud.WebSocket.WSMessage",
                "OBDCloud.WebSocket.BTDeviceInfo",
                "OBDCloud.WebSocket.WebSocketBridge",
                "OBDCloud.WebSocket.WebSocketOBDConnection",
                "OBDCloud.WebSocket.WebSocketServerBridge",
                "OBDCloud.WebSocket.WebSocketUIBridge",
                "OBDCloud.WebSocket.OBDCloudManager",
                "OBDCloud.WebSocket.WebSocketBluetoothConnection",
                "OBDCloud.WebSocket.WebSocketDeviceDiscovery",
                "OBDCloud.WebSocket.IWebSocketBridge",
                "OBDCloud.WebSocket.MessageType",
                "OBDCloud.WebSocket.MessageAction",
                "OBDCloud.WebSocket.ConnectRequestData",
                "OBDCloud.WebSocket.OBDConnectRequestData",
                "OBDCloud.WebSocket.ConnectResult",
                "OBDCloud.WebSocket.ErrorData",
                "OBDCloud.WebSocket.WSMessageFactory",
                "OBDCloud.WebSocket.WebSocketConnectionAdapter",
            };

            // 添加类型转发器（通过 ExportedType 实现）
            foreach (var fullName in typesToForward)
            {
                var lastDot = fullName.LastIndexOf('.');
                var ns = fullName.Substring(0, lastDot);
                var name = fullName.Substring(lastDot + 1);

                // 添加 ExportedType（类型转发器）到模块
                var exportedType = new ExportedTypeUser(module, 0, ns, name, TypeAttributes.Public | TypeAttributes.Forwarder, targetAsmRef);
                module.ExportedTypes.Add(exportedType);
            }

            var outputPath = Path.Combine(outputDir, assemblyName + ".dll");
            module.Write(outputPath);

            var fileInfo = new FileInfo(outputPath);
            Console.WriteLine($"  Created forwarder placeholder: {outputPath} ({fileInfo.Length} bytes)");
            Console.WriteLine($"  Added {module.ExportedTypes.Count} type forwarders");
        }

        /// <summary>
        /// 移除对指定程序集的引用
        /// 首先检查是否还有 TypeRef/MemberRef 引用该程序集
        /// </summary>
        private static void RemoveAssemblyReference(ModuleDefMD module, string assemblyName)
        {
            // 查找程序集引用
            var asmRef = module.GetAssemblyRefs().FirstOrDefault(r => r.Name == assemblyName);
            if (asmRef == null)
            {
                Console.WriteLine($"    Assembly reference '{assemblyName}' not found (already removed or never added)");
                return;
            }

            // 检查是否还有 TypeRef 引用该程序集
            var referencingTypeRefs = module.GetTypeRefs()
                .Where(tr => IsFromAssembly(tr, assemblyName))
                .ToList();

            if (referencingTypeRefs.Count > 0)
            {
                Console.WriteLine($"    WARNING: Found {referencingTypeRefs.Count} TypeRefs still referencing '{assemblyName}':");
                foreach (var tr in referencingTypeRefs.Take(10))
                {
                    Console.WriteLine($"      - {tr.FullName}");
                }
                if (referencingTypeRefs.Count > 10)
                    Console.WriteLine($"      ... and {referencingTypeRefs.Count - 10} more");

                // 尝试修复这些 TypeRef
                Console.WriteLine("    Attempting to fix remaining TypeRefs...");
                FixRemainingTypeRefs(module, assemblyName, referencingTypeRefs);
            }

            // 再次检查
            referencingTypeRefs = module.GetTypeRefs()
                .Where(tr => IsFromAssembly(tr, assemblyName))
                .ToList();

            if (referencingTypeRefs.Count > 0)
            {
                Console.WriteLine($"    Still have {referencingTypeRefs.Count} unfixable TypeRefs, cannot remove assembly reference");
                return;
            }

            // 移除程序集引用
            // dnlib 的 AssemblyRefs 是只读的，但我们可以通过清理来实现
            // 实际上，如果没有任何 TypeRef 引用它，dnlib 在写入时会自动忽略它
            Console.WriteLine($"    No remaining references to '{assemblyName}', it will be removed during save");
        }

        /// <summary>
        /// 修复剩余的 TypeRef 引用
        /// </summary>
        private static void FixRemainingTypeRefs(ModuleDefMD module, string assemblyName, List<TypeRef> typeRefs)
        {
            var typeDefMap = new Dictionary<string, TypeDef>();
            foreach (var type in module.GetTypes())
            {
                if (!type.IsGlobalModuleType)
                    typeDefMap[type.FullName] = type;
            }

            foreach (var typeRef in typeRefs)
            {
                // 检查是否有本地 TypeDef 可以替换
                if (typeDefMap.TryGetValue(typeRef.FullName, out var localType))
                {
                    Console.WriteLine($"      Fixing: {typeRef.FullName} -> local TypeDef");

                    // 我们需要找到所有使用这个 TypeRef 的地方并替换为 TypeDef
                    // 这包括字段、方法参数、返回类型、IL指令等

                    // 遍历所有类型的成员
                    foreach (var type in module.GetTypes())
                    {
                        // 检查字段类型
                        foreach (var field in type.Fields)
                        {
                            if (field.FieldType is ClassSig cs && cs.TypeDefOrRef == typeRef)
                            {
                                field.FieldType = new ClassSig(localType);
                            }
                        }

                        // 检查方法
                        foreach (var method in type.Methods)
                        {
                            // 检查方法体中的指令
                            if (method.HasBody)
                            {
                                foreach (var instr in method.Body.Instructions)
                                {
                                    if (instr.Operand == typeRef)
                                    {
                                        instr.Operand = localType;
                                    }
                                    else if (instr.Operand is MemberRef mr && mr.DeclaringType == typeRef)
                                    {
                                        // MemberRef 的 DeclaringType 是 TypeRef，需要替换为 TypeDef
                                        // 找到本地类型中对应的成员
                                        if (mr.IsMethodRef)
                                        {
                                            var localMethod = localType.FindMethod(mr.Name, mr.MethodSig);
                                            if (localMethod != null)
                                                instr.Operand = localMethod;
                                        }
                                        else if (mr.IsFieldRef)
                                        {
                                            var localField = localType.FindField(mr.Name);
                                            if (localField != null)
                                                instr.Operand = localField;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"      Cannot fix: {typeRef.FullName} (no local TypeDef found)");
                }
            }
        }

        /// <summary>
        /// 最终清理：修改所有指向指定程序集的 TypeRef 的 ResolutionScope 为当前模块
        /// 这样在保存时就不会包含对该程序集的引用
        /// </summary>
        private static void FinalTypeRefCleanup(ModuleDefMD module, string assemblyName)
        {
            // 构建 TypeDef 映射
            var typeDefMap = new Dictionary<string, TypeDef>();
            foreach (var type in module.GetTypes())
            {
                if (!type.IsGlobalModuleType)
                    typeDefMap[type.FullName] = type;
            }

            // 先统计有多少 TypeRef
            var targetTypeRefs = module.GetTypeRefs()
                .Where(tr => IsFromAssembly(tr, assemblyName))
                .ToList();
            Console.WriteLine($"    Found {targetTypeRefs.Count} TypeRefs pointing to {assemblyName}");

            int fixed_count = 0;
            int replaced_count = 0;
            var typeRefCache = new Dictionary<TypeDef, TypeRef>();

            // 遍历所有 TypeRef
            foreach (var typeRef in targetTypeRefs)
            {
                Console.WriteLine($"      Processing TypeRef: {typeRef.FullName}");

                // 尝试查找本地 TypeDef
                if (typeDefMap.TryGetValue(typeRef.FullName, out var localType))
                {
                    Console.WriteLine($"        -> Found local TypeDef");

                    // 方案1: 直接修改 ResolutionScope 指向当前模块
                    SetTypeRefResolutionScope(typeRef, localType, module, typeRefCache);
                    fixed_count++;

                    // 方案2: 尝试替换所有使用这个 TypeRef 的地方为 TypeDef
                    foreach (var type in module.GetTypes())
                    {
                        foreach (var method in type.Methods)
                        {
                            if (!method.HasBody) continue;

                            for (int i = 0; i < method.Body.Instructions.Count; i++)
                            {
                                var instr = method.Body.Instructions[i];
                                if (instr.Operand == typeRef)
                                {
                                    instr.Operand = localType;
                                    replaced_count++;
                                }
                                else if (instr.Operand is MemberRef mr && mr.DeclaringType == typeRef)
                                {
                                    if (mr.IsMethodRef)
                                    {
                                        var localMethod = localType.Methods.FirstOrDefault(m =>
                                            m.Name == mr.Name && MatchMethodSig(m.MethodSig, mr.MethodSig));
                                        if (localMethod != null)
                                        {
                                            instr.Operand = localMethod;
                                            replaced_count++;
                                        }
                                    }
                                    else if (mr.IsFieldRef)
                                    {
                                        var localField = localType.Fields.FirstOrDefault(f => f.Name == mr.Name);
                                        if (localField != null)
                                        {
                                            instr.Operand = localField;
                                            replaced_count++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"        -> No local TypeDef found!");
                }
            }

            // 额外修复：处理 ResolutionScope 为模块但应指向嵌套类型的 TypeRef
            int localFixed = 0;
            foreach (var typeRef in module.GetTypeRefs().ToList())
            {
                if (typeRef.ResolutionScope is ModuleDef || typeRef.ResolutionScope is ModuleRef)
                {
                    if (typeDefMap.TryGetValue(typeRef.FullName, out var localType))
                    {
                        if (localType.DeclaringType != null)
                        {
                            SetTypeRefResolutionScope(typeRef, localType, module, typeRefCache);
                            localFixed++;
                        }
                    }
                    else if (string.IsNullOrEmpty(typeRef.Namespace))
                    {
                        var matches = typeDefMap.Values.Where(t => t.Name == typeRef.Name).ToList();
                        if (matches.Count == 1 && matches[0].DeclaringType != null)
                        {
                            SetTypeRefResolutionScope(typeRef, matches[0], module, typeRefCache);
                            localFixed++;
                        }
                    }
                }
            }

            Console.WriteLine($"    Fixed {fixed_count} TypeRef ResolutionScopes");
            Console.WriteLine($"    Replaced {replaced_count} IL operands");
            Console.WriteLine($"    Fixed {localFixed} local nested TypeRefs");

            // 验证修复后的状态
            var remaining = module.GetTypeRefs()
                .Where(tr => IsFromAssembly(tr, assemblyName))
                .ToList();
            Console.WriteLine($"    Remaining TypeRefs pointing to {assemblyName}: {remaining.Count}");
        }
    }
}
