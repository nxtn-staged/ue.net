// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System.Diagnostics;
using System.Linq;
using Leftice.Processors;
using Microsoft.Build.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Leftice.Sdk
{
    public sealed class Processor : Microsoft.Build.Utilities.Task
    {
        private const string ImportTableModuleName = "Leftice.Runtime.dll";
        private const string ImportTableNamespace = "Leftice";
        private const string ImportTableTypeName = "ImportTable";
        private const string GetFieldMethodName = "GetField";
        private const string GetMethodMethodName = "GetMethod";
        private const string GetOffsetMethodName = "GetOffset";
        private const string GetMethodName = "Get";
        private const string DisposeMethodName = "Dispose";

        private const string ImportTableReferenceTypeName = "ImportTableReference";

        private const string NativeMethodsTypeName = "NativeMethods";
        private const string MethodFieldSuffix = "_Method";
        private const string OffsetFieldSuffix = "_Offset";

        private const string ProcessorsNamespace = nameof(Leftice) + "." + nameof(Processors);

        private static TypeReference voidType;
        private static TypeReference byteType;
        private static TypeReference int32Type;
        private static TypeReference intPtrType;

        private static TypeReference importTableType;
        private static MethodReference getFieldMethod;
        private static MethodReference getMethodMethod;
        private static MethodReference getOffsetMethod;
        private static MethodReference getMethod;
        private static MethodReference disposeMethod;

        [Required]
        public ITaskItem AssemblyFile { get; set; }

        public override bool Execute()
        {
            ProcessModule(this.AssemblyFile.ItemSpec);
            return true;
        }

        private static bool AreEqual(TypeReference type, string @namespace, string name) =>
            type.Namespace == @namespace && type.Name == name;

        private static bool AreEqual(TypeReference type, System.Type other) => AreEqual(type, other.Namespace, other.Name);

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
                is MethodReference method ? method : throw new System.ArgumentException();

        private static TypeReference GetTypeReference(ModuleDefinition module, string @namespace, string name) =>
            module.GetTypeReferences().First(type => AreEqual(type, @namespace, name));

        private static void ImplementCalli(TypeDefinition type, VariableDefinition tableVariable, ILProcessor cctorProcessor, MethodDefinition method)
        {
            method.CustomAttributes.Clear();

            var methodField = new FieldDefinition(
                method.Name + MethodFieldSuffix,
                FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly,
                intPtrType);

            type.Fields.Add(methodField);

            cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
            cctorProcessor.Emit(OpCodes.Ldstr, method.Name);
            cctorProcessor.Emit(OpCodes.Call, getMethodMethod);
            cctorProcessor.Emit(OpCodes.Stsfld, methodField);

            var ilProcessor = method.Body.GetILProcessor();

            var callSite = new CallSite(TransformType(method.ReturnType))
            {
                CallingConvention = MethodCallingConvention.C,
            };

            foreach (var parameter in method.Parameters)
            {
                ilProcessor.Emit(OpCodes.Ldarga_S, parameter);
                callSite.Parameters.Add(TransformParameter(parameter));
            }

            ilProcessor.Emit(OpCodes.Ldsfld, methodField);
            ilProcessor.Emit(OpCodes.Calli, callSite);
            ilProcessor.Emit(OpCodes.Ret);

            static ParameterDefinition TransformParameter(ParameterDefinition parameter) =>
                AreEqual(parameter.ParameterType, typeof(bool)) ? new ParameterDefinition(byteType) : parameter;

            static TypeReference TransformType(TypeReference type) =>
                AreEqual(type, typeof(bool)) ? byteType : type;
        }

        private static void ImplementOffset(bool asPointer, TypeDefinition type, VariableDefinition tableVariable, ILProcessor cctorProcessor, MethodDefinition method)
        {
            Debug.Assert(method.Parameters.Count == 1);

            method.CustomAttributes.Clear();

            Debug.Assert(method.Name.StartsWith("Get"));

            string propertyName = method.Name.Substring("Get".Length);

            var offsetField = new FieldDefinition(
                propertyName + OffsetFieldSuffix,
                FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly,
                int32Type);

            type.Fields.Add(offsetField);

            cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
            cctorProcessor.Emit(OpCodes.Ldstr, propertyName);
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

        private static void ProcessMethod(TypeDefinition type, VariableDefinition tableVariable, ILProcessor cctorProcessor, MethodDefinition method)
        {
            if (!method.HasBody)
            {
                return;
            }

            if (!method.HasCustomAttributes)
            {
                return;
            }

            foreach (var attribute in method.CustomAttributes)
            {
                var attributeType = attribute.AttributeType;

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(CalliAttribute)))
                {
                    ImplementCalli(type, tableVariable, cctorProcessor, method);
                    return;
                }

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(ReadOffsetAttribute)))
                {
                    ImplementOffset(false, type, tableVariable, cctorProcessor, method);
                    return;
                }

                if (AreEqual(attributeType, ProcessorsNamespace, nameof(PointerOffsetAttribute)))
                {
                    ImplementOffset(true, type, tableVariable, cctorProcessor, method);
                    return;
                }
            }
        }

        private static void ProcessModule(string path)
        {
            using var module = ModuleDefinition.ReadModule(path, new ReaderParameters { ReadWrite = true });

            voidType = module.ImportReference(typeof(void));
            byteType = module.ImportReference(typeof(byte));
            int32Type = module.ImportReference(typeof(int));
            intPtrType = module.ImportReference(typeof(System.IntPtr));

            if (module.Name == ImportTableModuleName)
            {
                var importTableTypeDefinition = module.GetType(ImportTableNamespace, ImportTableTypeName);
                importTableType = importTableTypeDefinition;
                getFieldMethod = GetMethodDefinition(importTableTypeDefinition, GetFieldMethodName)!;
                getMethodMethod = GetMethodDefinition(importTableTypeDefinition, GetMethodMethodName)!;
                getOffsetMethod = GetMethodDefinition(importTableTypeDefinition, GetOffsetMethodName)!;
                getMethod = GetMethodDefinition(importTableTypeDefinition, GetMethodName)!;
                disposeMethod = GetMethodDefinition(importTableTypeDefinition, DisposeMethodName)!;
            }
            else
            {
                importTableType = GetTypeReference(module, ImportTableNamespace, ImportTableTypeName);
                getFieldMethod = GetMethodReference(module, importTableType, GetFieldMethodName);
                getMethodMethod = GetMethodReference(module, importTableType, GetMethodMethodName);
                getOffsetMethod = GetMethodReference(module, importTableType, GetOffsetMethodName);
                getMethod = GetMethodReference(module, importTableType, GetMethodName);
                disposeMethod = GetMethodReference(module, importTableType, DisposeMethodName);
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

            module.Write();

            System.IO.File.WriteAllText(path + ".timestamp", string.Empty);
        }

        private static void ProcessType(TypeDefinition type)
        {
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
               ".cctor",
               MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
               voidType);

            type.Methods.Add(cctor);

            var tableVariable = new VariableDefinition(importTableType);
            cctor.Body.Variables.Add(tableVariable);

            var cctorProcessor = cctor.Body.GetILProcessor();

            cctorProcessor.Emit(OpCodes.Ldstr, type.DeclaringType.Name);
            cctorProcessor.Emit(OpCodes.Call, getMethod);
            cctorProcessor.Emit(OpCodes.Stloc_0);

            foreach (var field in type.Fields)
            {
                cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
                cctorProcessor.Emit(OpCodes.Ldstr, field.Name);
                cctorProcessor.Emit(OpCodes.Call, getFieldMethod);
                cctorProcessor.Emit(OpCodes.Stsfld, field);
            }

            foreach (var method in type.Methods)
            {
                ProcessMethod(type, tableVariable, cctorProcessor, method);
            }

            cctorProcessor.Emit(OpCodes.Ldloca_S, tableVariable);
            cctorProcessor.Emit(OpCodes.Call, disposeMethod);
            cctorProcessor.Emit(OpCodes.Ret);
        }
    }
}
