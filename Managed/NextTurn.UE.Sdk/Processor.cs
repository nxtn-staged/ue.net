// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Build.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NextTurn.UE.Annotations;

namespace NextTurn.UE.Sdk
{
    public sealed class Processor : Microsoft.Build.Utilities.Task
    {
        private const string ImportTableModuleName = "NextTurn.UE.Runtime.dll";
        private const string ImportTableNamespace = "NextTurn.UE";
        private const string ImportTableTypeName = "ImportTable";
        private const string GetFieldMethodName = "GetField";
        private const string GetMethodMethodName = "GetMethod";
        private const string GetOffsetMethodName = "GetOffset";
        private const string GetMethodName = "Get";

        private const string ImportTableReferenceTypeName = "ImportTableReference";

        private const string NativeMethodsTypeName = "NativeMethods";
        private const string MethodFieldSuffix = "_Method";
        private const string OffsetFieldSuffix = "_Offset";

        private const string SystemRuntimeAssemblyName = "System.Runtime";

        private const string ProcessorsNamespace = nameof(NextTurn) + "." + nameof(UE) + "." + nameof(Annotations);

        private static AssemblyNameReference systemRuntimeReference;

        private static TypeReference voidType;

        private static TypeReference byteType;
        private static TypeReference charType;
        private static TypeReference charPtrType;
        private static TypeReference charRefType;
        private static TypeReference int32Type;
        private static TypeReference intPtrType;

        private static MethodReference stringGetReferenceMethod;

        private static TypeReference importTableType;
        private static MethodReference getFieldMethod;
        private static MethodReference getMethodMethod;
        private static MethodReference getOffsetMethod;
        private static MethodReference getMethod;

        [Required]
        public ITaskItem AssemblyFile { get; set; }

        public override bool Execute()
        {
            ProcessModule(this.AssemblyFile.ItemSpec);
            return true;
        }

        private static bool AreEqual(TypeReference type, string @namespace, string name) =>
            type.Namespace == @namespace && type.Name == name;

        private static bool AreEqual(TypeReference type, Type other) => AreEqual(type, other.Namespace, other.Name);

        private static TypeReference CreateTypeReference(ModuleDefinition module, Type type) =>
            new TypeReference(type.Namespace, type.Name, module, systemRuntimeReference, type.IsValueType);

        private static AssemblyNameReference GetAssemblyReference(ModuleDefinition module, string name) =>
            module.AssemblyReferences.Single(assembly => assembly.Name == name);

        private static MethodDefinition GetMethodDefinition(TypeDefinition type, string name)
        {
            foreach (var method in type.Methods)
            {
                if (method.Name == name)
                {
                    return method;
                }
            }

            return null;
        }

        private static MethodReference GetMethodReference(ModuleDefinition module, TypeReference declaringType, string name) =>
            module.GetMemberReferences().First(member => member.DeclaringType == declaringType && member.Name == name)
                is MethodReference method ? method : throw new ArgumentException();

        private static TypeReference GetTypeReference(ModuleDefinition module, string @namespace, string name) =>
            module.GetTypeReferences().First(type => AreEqual(type, @namespace, name));

        private static TypeReference GetTypeReference(ModuleDefinition module, Type type) =>
            GetTypeReference(module, type.Namespace, type.Name);

        private static void ImplementCalli(
            bool managedCall,
            TypeDefinition type,
            VariableDefinition tableVariable,
            ILProcessor cctorProcessor,
            MethodDefinition method,
            int index)
        {
            var methodField = new FieldDefinition(
                method.Name + MethodFieldSuffix,
                FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly,
                intPtrType);

            type.Fields.Add(methodField);

            cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
            cctorProcessor.Emit(OpCodes.Ldc_I4, index);
            cctorProcessor.Emit(OpCodes.Call, getMethodMethod);
            cctorProcessor.Emit(OpCodes.Stsfld, methodField);

            var ilProcessor = method.Body.GetILProcessor();

            var callSite = new CallSite(TransformBooleanType(method.ReturnType))
            {
                CallingConvention = managedCall ? MethodCallingConvention.Default : MethodCallingConvention.C,
            };

            var brNextStart = default(Instruction);

            foreach (var parameter in method.Parameters)
            {
                var nextArgStart = ilProcessor.Create(OpCodes.Ldarg_S, parameter);
                ilProcessor.Append(nextArgStart);
                if (brNextStart is not null)
                {
                    ilProcessor.Replace(brNextStart, ilProcessor.Create(OpCodes.Br_S, nextArgStart));
                    brNextStart = null;
                }

                if (parameter.ParameterType is ByReferenceType byReferenceType)
                {
                    var pinnedVariable = new VariableDefinition(new PinnedType(byReferenceType));
                    method.Body.Variables.Add(pinnedVariable);

                    ilProcessor.Emit(OpCodes.Stloc_S, pinnedVariable);
                    ilProcessor.Emit(OpCodes.Ldloc_S, pinnedVariable);

                    callSite.Parameters.Add(new ParameterDefinition(new PointerType(byReferenceType.ElementType)));
                    continue;
                }

                if (AreEqual(parameter.ParameterType, typeof(bool)))
                {
                    callSite.Parameters.Add(new ParameterDefinition(byteType));
                    continue;
                }

                if (AreEqual(parameter.ParameterType, typeof(string)))
                {
                    callSite.Parameters.Add(new ParameterDefinition(charPtrType));

                    var variable = new VariableDefinition(new PinnedType(charRefType));
                    method.Body.Variables.Add(variable);

                    var brNotNullStart = ilProcessor.Create(OpCodes.Nop);
                    ilProcessor.Append(brNotNullStart);

                    ilProcessor.Emit(OpCodes.Ldc_I4_0);

                    brNextStart = ilProcessor.Create(OpCodes.Nop);
                    ilProcessor.Append(brNextStart);

                    var notNullStart = ilProcessor.Create(OpCodes.Ldarg_S, parameter);
                    ilProcessor.Append(notNullStart);
                    ilProcessor.Replace(brNotNullStart, ilProcessor.Create(OpCodes.Brtrue_S, notNullStart));

                    ilProcessor.Emit(OpCodes.Call, stringGetReferenceMethod);
                    ilProcessor.Emit(OpCodes.Stloc_S, variable);
                    ilProcessor.Emit(OpCodes.Ldloc_S, variable);

                    continue;
                }

                callSite.Parameters.Add(parameter);
            }

            var nextStart = ilProcessor.Create(OpCodes.Ldsfld, methodField);
            ilProcessor.Append(nextStart);
            if (brNextStart is not null)
            {
                ilProcessor.Replace(brNextStart, ilProcessor.Create(OpCodes.Br_S, nextStart));
            }

            ilProcessor.Emit(OpCodes.Calli, callSite);
            ilProcessor.Emit(OpCodes.Ret);

            static TypeReference TransformBooleanType(TypeReference type) =>
                AreEqual(type, typeof(bool)) ? byteType : type;
        }

        private static void ImplementOffset(
            bool asPointer,
            TypeDefinition type,
            VariableDefinition tableVariable,
            ILProcessor cctorProcessor,
            MethodDefinition method,
            int index)
        {
            Debug.Assert(method.Parameters.Count == 1);
            Debug.Assert(method.Name.StartsWith("Get"));

#if NETCOREAPP
            string propertyName = method.Name["Get".Length..];
#else
            string propertyName = method.Name.Substring("Get".Length);
#endif

            var offsetField = new FieldDefinition(
                propertyName + OffsetFieldSuffix,
                FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly,
                int32Type);

            type.Fields.Add(offsetField);

            cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
            cctorProcessor.Emit(OpCodes.Ldc_I4, index);
            cctorProcessor.Emit(OpCodes.Call, getOffsetMethod);
            cctorProcessor.Emit(OpCodes.Stsfld, offsetField);

            var ilProcessor = method.Body.GetILProcessor();

            ilProcessor.Emit(OpCodes.Ldarg_0);
            ilProcessor.Emit(OpCodes.Ldsfld, offsetField);
            ilProcessor.Emit(OpCodes.Add);
            if (!asPointer)
            {
                ilProcessor.Emit(OpCodes.Ldobj, method.ReturnType);
            }

            ilProcessor.Emit(OpCodes.Ret);
        }

        private static void ProcessMethod(
            TypeDefinition type,
            VariableDefinition tableVariable,
            ILProcessor cctorProcessor,
            MethodDefinition method,
            ref int index)
        {
            if (!method.HasBody)
            {
                return;
            }

            if (!method.HasCustomAttributes)
            {
                return;
            }

            for (int i = 0; i < method.CustomAttributes.Count; i++)
            {
                var attribute = method.CustomAttributes[i];
                var attributeType = attribute.AttributeType;

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(CalliAttribute)))
                {
                    ImplementCalli(false, type, tableVariable, cctorProcessor, method, index++);
                    method.CustomAttributes.RemoveAt(i);
                    return;
                }

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(ReadOffsetAttribute)))
                {
                    ImplementOffset(false, type, tableVariable, cctorProcessor, method, index++);
                    method.CustomAttributes.RemoveAt(i);
                    return;
                }

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(PointerOffsetAttribute)))
                {
                    ImplementOffset(true, type, tableVariable, cctorProcessor, method, index++);
                    method.CustomAttributes.RemoveAt(i);
                    return;
                }

                if (AreEqual(attributeType, nameof(NextTurn) + '.' + nameof(UE), nameof(ImportAttribute)))
                {
                    method.CustomAttributes.RemoveAt(i);
                    switch ((ImportOperation)attribute.ConstructorArguments[0].Value)
                    {
                        case ImportOperation.Calli:
                            ImplementCalli(managedCall: false, type, tableVariable, cctorProcessor, method, index++);
                            break;
                        case ImportOperation.CalliManaged:
                            ImplementCalli(managedCall: true, type, tableVariable, cctorProcessor, method, index++);
                            break;
                        case ImportOperation.AsPointerOffset:
                            ImplementOffset(asPointer: true, type, tableVariable, cctorProcessor, method, index++);
                            break;
                        case ImportOperation.ReadOffset:
                            ImplementOffset(asPointer: false, type, tableVariable, cctorProcessor, method, index++);
                            break;
                    }
                }
            }
        }

        private static void ProcessModule(string path)
        {
            using var module = ModuleDefinition.ReadModule(path, new ReaderParameters
            {
                ReadWrite = true,
                ReadSymbols = true,
                SymbolReaderProvider = new PortablePdbReaderProvider(),
            });

            systemRuntimeReference = GetAssemblyReference(module, SystemRuntimeAssemblyName);

            voidType = module.TypeSystem.Void;

            byteType = module.TypeSystem.Byte;
            charType = module.TypeSystem.Char;

            charPtrType = new PointerType(charType);
            charRefType = new ByReferenceType(charType);

            int32Type = module.TypeSystem.Int32;
            intPtrType = module.TypeSystem.IntPtr;

            stringGetReferenceMethod = module.ImportReference(typeof(string).GetMethod("GetPinnableReference"));
            stringGetReferenceMethod.DeclaringType.Scope = systemRuntimeReference;
            var ina = module.ImportReference(typeof(System.Runtime.InteropServices.InAttribute));
            ina.Scope = systemRuntimeReference;
            stringGetReferenceMethod.ReturnType = new RequiredModifierType(ina, stringGetReferenceMethod.ReturnType);

            if (module.Name == ImportTableModuleName)
            {
                var importTableTypeDefinition = module.GetType(ImportTableNamespace, ImportTableTypeName);
                importTableType = importTableTypeDefinition;
                getFieldMethod = GetMethodDefinition(importTableTypeDefinition, GetFieldMethodName)!;
                getMethodMethod = GetMethodDefinition(importTableTypeDefinition, GetMethodMethodName)!;
                getOffsetMethod = GetMethodDefinition(importTableTypeDefinition, GetOffsetMethodName)!;
                getMethod = GetMethodDefinition(importTableTypeDefinition, GetMethodName)!;
            }
            else if (module.Name != "NextTurn.UE.Loader.dll")
            {
                importTableType = GetTypeReference(module, ImportTableNamespace, ImportTableTypeName);
                getFieldMethod = GetMethodReference(module, importTableType, GetFieldMethodName);
                getMethodMethod = GetMethodReference(module, importTableType, GetMethodMethodName);
                getOffsetMethod = GetMethodReference(module, importTableType, GetOffsetMethodName);
                getMethod = GetMethodReference(module, importTableType, GetMethodName);
            }

            var types = module.Types;

            foreach (var type in types)
            {
                ProcessType(type);
            }

            if (module.Name != ImportTableModuleName)
            {
                for (int i = 0; i < types.Count; i++)
                {
                    var type = types[i];
                    if (type.Name == ImportTableReferenceTypeName)
                    {
                        types.RemoveAt(i);
                        break;
                    }
                }
            }

            module.Write(new WriterParameters
            {
                WriteSymbols = true,
            });

            System.IO.File.WriteAllText(path + ".timestamp", string.Empty);
        }

        private static void ProcessType(TypeDefinition type)
        {
            int index = 0;

            if (type.Name == "Loader" || type.Name == "EntryPoint")
            {
                foreach (var method in type.Methods)
                {
                    ProcessMethod(type, null, null, method, ref index);
                }

                return;
            }

            if (type.Name != NativeMethodsTypeName)
            {
                foreach (var nestedType in type.NestedTypes)
                {
                    ProcessType(nestedType);
                }

                return;
            }

            Debug.Assert(type.IsAbstract);
            Debug.Assert(type.IsSealed);
            Debug.Assert(!type.HasNestedTypes);
            Debug.Assert(GetMethodDefinition(type, ".cctor") == null);

            var cctor = new MethodDefinition(
                attributes:
                    MethodAttributes.Private |
                    MethodAttributes.Static |
                    MethodAttributes.SpecialName |
                    MethodAttributes.RTSpecialName,
                name: ".cctor",
                returnType: voidType);

            type.Methods.Add(cctor);

            var tableVariable = new VariableDefinition(importTableType);
            cctor.Body.Variables.Add(tableVariable);

            var cctorProcessor = cctor.Body.GetILProcessor();

            cctorProcessor.Emit(OpCodes.Ldstr, type.DeclaringType.Name);

            var ldCount = cctorProcessor.Create(OpCodes.Nop);
            cctorProcessor.Append(ldCount);

            cctorProcessor.Emit(OpCodes.Call, getMethod);
            cctorProcessor.Emit(OpCodes.Stloc_0);

            foreach (var field in type.Fields)
            {
                cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
                cctorProcessor.Emit(OpCodes.Ldc_I4, index++);
                cctorProcessor.Emit(OpCodes.Call, getFieldMethod);
                cctorProcessor.Emit(OpCodes.Stsfld, field);
            }

            foreach (var method in type.Methods)
            {
                if (method.Name == ".cctor")
                {
                    continue;
                }

                ProcessMethod(type, tableVariable, cctorProcessor, method, ref index);
            }

            cctorProcessor.Replace(ldCount, cctorProcessor.Create(OpCodes.Ldc_I4, index));
            cctorProcessor.Emit(OpCodes.Ret);
        }
    }
}
