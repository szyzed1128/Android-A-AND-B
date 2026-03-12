using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

internal static class Verify
{
    public static void Run(string dllPath)
    {
        if (string.IsNullOrWhiteSpace(dllPath) || !File.Exists(dllPath))
            throw new FileNotFoundException($"DLL not found: {dllPath}");

        var module = ModuleDefMD.Load(dllPath);

        Console.WriteLine("=== 验证修改后的DLL ===\n");

        // 检查程序集引用
        Console.WriteLine("=== Assembly References ===");
        foreach (var asmRef in module.GetAssemblyRefs())
        {
            var marker = asmRef.Name.Contains("OBDCloud") ? " *** PROBLEM ***" : "";
            Console.WriteLine($"  {asmRef.Name} {asmRef.Version}{marker}");
        }

        // 检查所有 TypeRef
        Console.WriteLine("\n=== TypeRefs referencing OBDCloud.WebSocket ===");
        int typeRefCount = 0;
        foreach (var typeRef in module.GetTypeRefs())
        {
            if (IsOBDCloud(typeRef))
            {
                Console.WriteLine($"  TypeRef: {typeRef.FullName}");
                typeRefCount++;
            }
        }
        if (typeRefCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查所有 MemberRef
        Console.WriteLine("\n=== MemberRefs referencing OBDCloud types ===");
        int memberRefCount = 0;
        foreach (var memberRef in module.GetMemberRefs())
        {
            if (memberRef.DeclaringType != null)
            {
                var declType = memberRef.DeclaringType;
                if (declType is TypeRef tr && tr.ResolutionScope is AssemblyRef ar2 && ar2.Name.Contains("OBDCloud"))
                {
                    Console.WriteLine($"  MemberRef: {declType.FullName}.{memberRef.Name} -> {ar2.Name}");
                    memberRefCount++;
                }
                else if (declType is TypeSpec ts)
                {
                    if (HasOBDCloudReference(ts.TypeSig))
                    {
                        Console.WriteLine($"  MemberRef (TypeSpec): {ts.FullName}.{memberRef.Name}");
                        memberRefCount++;
                    }
                }
            }
        }
        if (memberRefCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查所有方法体中的指令
        Console.WriteLine("\n=== IL Instructions referencing OBDCloud ===");
        int instrCount = 0;
        foreach (var type in module.GetTypes())
        {
            foreach (var method in type.Methods)
            {
                if (!method.HasBody) continue;
                foreach (var instr in method.Body.Instructions)
                {
                    if (instr.Operand == null) continue;
                    string issue = CheckOperand(instr.Operand);
                    if (issue != null)
                    {
                        Console.WriteLine($"  {type.Name}.{method.Name} IL_{instr.Offset:X4}: {issue}");
                        instrCount++;
                    }
                }
            }
        }
        if (instrCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查字段类型
        Console.WriteLine("\n=== Field types referencing OBDCloud ===");
        int fieldCount = 0;
        foreach (var type in module.GetTypes())
        {
            foreach (var field in type.Fields)
            {
                if (HasOBDCloudReference(field.FieldType))
                {
                    Console.WriteLine($"  {type.Name}.{field.Name}: {field.FieldType.FullName}");
                    fieldCount++;
                }
            }
        }
        if (fieldCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查方法签名
        Console.WriteLine("\n=== Method signatures referencing OBDCloud ===");
        int methodSigCount = 0;
        foreach (var type in module.GetTypes())
        {
            foreach (var method in type.Methods)
            {
                if (method.MethodSig == null) continue;
                if (HasOBDCloudReference(method.MethodSig.RetType))
                {
                    Console.WriteLine($"  {type.Name}.{method.Name} return type: {method.MethodSig.RetType.FullName}");
                    methodSigCount++;
                }
                foreach (var param in method.MethodSig.Params)
                {
                    if (HasOBDCloudReference(param))
                    {
                        Console.WriteLine($"  {type.Name}.{method.Name} param: {param.FullName}");
                        methodSigCount++;
                    }
                }
            }
        }
        if (methodSigCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查基类和接口
        Console.WriteLine("\n=== Base types / interfaces referencing OBDCloud ===");
        int baseCount = 0;
        foreach (var type in module.GetTypes())
        {
            if (type.BaseType != null && IsOBDCloud(type.BaseType))
            {
                Console.WriteLine($"  {type.Name} extends {type.BaseType.FullName}");
                baseCount++;
            }
            foreach (var iface in type.Interfaces)
            {
                if (IsOBDCloud(iface.Interface))
                {
                    Console.WriteLine($"  {type.Name} implements {iface.Interface.FullName}");
                    baseCount++;
                }
            }
        }
        if (baseCount == 0)
            Console.WriteLine("  None found (good!)");

        // 检查自定义属性
        Console.WriteLine("\n=== Custom attributes referencing OBDCloud ===");
        int caCount = 0;
        foreach (var type in module.GetTypes())
        {
            foreach (var ca in type.CustomAttributes)
            {
                if (IsOBDCloud(ca.AttributeType))
                {
                    Console.WriteLine($"  {type.Name}: [{ca.AttributeType.FullName}]");
                    caCount++;
                }
            }
        }
        if (caCount == 0)
            Console.WriteLine("  None found (good!)");

        Console.WriteLine($"\n=== Summary ===");
        int total = typeRefCount + memberRefCount + instrCount + fieldCount + methodSigCount + baseCount + caCount;
        Console.WriteLine($"Total OBDCloud references: {total}");
        if (total == 0)
            Console.WriteLine("All clear! No OBDCloud.WebSocket references found.");
        else
            Console.WriteLine("*** PROBLEMS FOUND - references to OBDCloud.WebSocket still exist ***");
    }

    static bool IsOBDCloud(ITypeDefOrRef typeRef)
    {
        if (typeRef == null) return false;
        if (typeRef is TypeRef tr)
        {
            if (tr.ResolutionScope is AssemblyRef ar)
                return ar.Name.Contains("OBDCloud");
            if (tr.ResolutionScope is TypeRef parent)
                return IsOBDCloud(parent);
        }
        if (typeRef is TypeSpec ts)
            return HasOBDCloudReference(ts.TypeSig);
        return false;
    }

    static bool HasOBDCloudReference(TypeSig sig)
    {
        if (sig == null) return false;

        switch (sig)
        {
            case ClassSig cs:
                return IsOBDCloud(cs.TypeDefOrRef);
            case ValueTypeSig vs:
                return IsOBDCloud(vs.TypeDefOrRef);
            case GenericInstSig gis:
                if (HasOBDCloudReference(gis.GenericType)) return true;
                foreach (var arg in gis.GenericArguments)
                    if (HasOBDCloudReference(arg)) return true;
                return false;
            case SZArraySig sza:
                return HasOBDCloudReference(sza.Next);
            case ByRefSig brs:
                return HasOBDCloudReference(brs.Next);
            default:
                if (sig.Next != null)
                    return HasOBDCloudReference(sig.Next);
                return false;
        }
    }

    static string CheckOperand(object operand)
    {
        if (operand is ITypeDefOrRef t && IsOBDCloud(t))
            return $"TypeRef: {t.FullName}";
        if (operand is MemberRef mr && mr.DeclaringType != null && IsOBDCloud(mr.DeclaringType))
            return $"MemberRef: {mr.DeclaringType.FullName}.{mr.Name}";
        if (operand is MethodSpec ms && ms.Method is MemberRef mr2 && mr2.DeclaringType != null && IsOBDCloud(mr2.DeclaringType))
            return $"MethodSpec: {mr2.DeclaringType.FullName}.{mr2.Name}";
        if (operand is TypeSpec ts2 && HasOBDCloudReference(ts2.TypeSig))
            return $"TypeSpec: {ts2.FullName}";
        return null;
    }
}
