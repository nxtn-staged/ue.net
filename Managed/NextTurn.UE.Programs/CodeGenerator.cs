// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using Unreal;
using Enum = Unreal.Enum;
using Object = Unreal.Object;
using SR = System.Reflection;

namespace NextTurn.UE.Programs
{
    internal static class CodeGenerator
    {
        private const string AssemblyName = "NextTurn.UE.Project";
        private const string ModuleName = AssemblyName + ".dll";
        private const string XmlDocumentFileName = AssemblyName + ".xml";

        private const string ClassFieldName = "Class";
        private const string PropertyFieldSuffix = "_Property";
        private const string MethodFieldSuffix = "_Method";
        private const string OffsetFieldSuffix = "_Offset";

        private const string ScriptNameMetaDataKey = "ScriptName";

        private const SR.BindingFlags LookupAllAttributes = SR.BindingFlags.Instance | SR.BindingFlags.Static | SR.BindingFlags.Public | SR.BindingFlags.NonPublic;

        private const TypeAttributes CommonTypeAttributes = TypeAttributes.Public | TypeAttributes.BeforeFieldInit;
        private const FieldAttributes CommonFieldAttributes = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;

        private static ModuleDefinition module = null!;
        private static XmlDocument document = null!;

        /// <summary>
        /// The <see cref="void"/> type.
        /// </summary>
        private static TypeReference voidType = null!;

        /// <summary>
        /// The <see cref="sbyte"/> type.
        /// </summary>
        private static TypeReference sbyteType = null!;

        /// <summary>
        /// The <see cref="byte"/> type.
        /// </summary>
        private static TypeReference byteType = null!;

        /// <summary>
        /// The <see cref="short"/> type.
        /// </summary>
        private static TypeReference int16Type = null!;

        /// <summary>
        /// The <see cref="ushort"/> type.
        /// </summary>
        private static TypeReference uint16Type = null!;

        /// <summary>
        /// The <see cref="int"/> type.
        /// </summary>
        private static TypeReference int32Type = null!;

        /// <summary>
        /// The <see cref="uint"/> type.
        /// </summary>
        private static TypeReference uint32Type = null!;

        /// <summary>
        /// The <see cref="long"/> type.
        /// </summary>
        private static TypeReference int64Type = null!;

        /// <summary>
        /// The <see cref="ulong"/> type.
        /// </summary>
        private static TypeReference uint64Type = null!;

        /// <summary>
        /// The <see cref="float"/> type.
        /// </summary>
        private static TypeReference singleType = null!;

        /// <summary>
        /// The <see cref="double"/> type.
        /// </summary>
        private static TypeReference doubleType = null!;

        /// <summary>
        /// The <see cref="string"/> type.
        /// </summary>
        private static TypeReference stringType = null!;

        /// <summary>
        /// The <see cref="IntPtr"/> type.
        /// </summary>
        private static TypeReference intPtrType = null!;

        /// <summary>
        /// The <see cref="byte"/> pointer type.
        /// </summary>
        private static TypeReference bytePointerType = null!;

        /// <summary>
        /// The <see cref="ValueType"/> type.
        /// </summary>
        private static TypeReference valueTypeType = null!;

        /// <summary>
        /// The <see cref="System.Enum"/> type.
        /// </summary>
        private static TypeReference enumType = null!;

        /// <summary>
        /// The <see cref="NativeBoolean"/> type.
        /// </summary>
        private static TypeReference nativeBooleanType = null!;

        /// <summary>
        /// The <see cref="FixedSizeArray{T}"/> type.
        /// </summary>
        private static TypeReference fixedSizeArrayType = null!;

        /// <summary>
        /// The <see cref="Name"/> type.
        /// </summary>
        private static TypeReference nameType = null!;

        /// <summary>
        /// The <see cref="Text"/> type.
        /// </summary>
        private static TypeReference textType = null!;

        /// <summary>
        /// The <see cref="Object"/> type.
        /// </summary>
        private static TypeReference unrealObjectType = null!;

        /// <summary>
        /// The <see cref="Class"/> type.
        /// </summary>
        private static TypeReference unrealClassType = null!;

        /// <summary>
        /// The <see cref="Property"/> type.
        /// </summary>
        private static TypeReference unrealPropertyType = null!;

        /// <summary>
        /// The <see cref="Method"/> type.
        /// </summary>
        private static TypeReference unrealMethodType = null!;

        /// <summary>
        /// The <see cref="WeakObjectReference{T}"/> type.
        /// </summary>
        private static TypeReference weakObjectReferenceTType = null!;

        /// <summary>
        /// The <see cref="LazyObjectReference{T}"/> type.
        /// </summary>
        private static TypeReference lazyObjectReferenceTType = null!;

        /// <summary>
        /// The <see cref="SoftObjectReference{T}"/> type.
        /// </summary>
        private static TypeReference softObjectReferenceTType = null!;

        /// <summary>
        /// The <see cref="Array{T}"/> type.
        /// </summary>
        private static TypeReference unrealArrayType = null!;

        /// <summary>
        /// The <see cref="Map{TKey, TValue}"/> type.
        /// </summary>
        private static TypeReference unrealMapType = null!;

        /// <summary>
        /// The <see cref="Set{T}"/> type.
        /// </summary>
        private static TypeReference unrealSetType = null!;

        /// <summary>
        /// The <see cref="ScriptDelegate"/> type.
        /// </summary>
        private static TypeReference scriptDelegateType = null!;

        /// <summary>
        /// The <see cref="IDynamicDelegate"/> type.
        /// </summary>
        private static TypeReference idynamicDelegateType = null!;

        /// <summary>
        /// The <see cref="DynamicMulticastDelegate"/> type.
        /// </summary>
        private static TypeReference dynamicMulticastDelegateType = null!;

        /// <summary>
        /// The <see cref="BooleanMarshaler"/> type.
        /// </summary>
        private static TypeReference booleanMarshalerType = null!;

        /// <summary>
        /// The <see cref="ObjectMarshaler"/> type.
        /// </summary>
        private static TypeReference objectMarshalerType = null!;

        /// <summary>
        /// The <see cref="StringMarshaler"/> type.
        /// </summary>
        private static TypeReference stringMarshalerType = null!;

        /// <summary>
        /// The <see cref="TextMarshaler"/> type.
        /// </summary>
        private static TypeReference textMarshalerType = null!;

        /// <summary>
        /// The <see cref="TrivialMarshaler{T}"/> type.
        /// </summary>
        private static TypeReference trivialMarshalerType = null!;

        /// <summary>
        /// The <see cref="IsReadOnlyAttribute.IsReadOnlyAttribute"/> constructor.
        /// </summary>
        private static MethodReference isReadOnlyAttributeConstructor = null!;

        /// <summary>
        /// The <see cref="FixedSizeArray{T}.FixedSizeArray(IntPtr, int)"/> constructor.
        /// </summary>
        private static MethodReference fixedSizeArrayConstructor = null!;

        /// <summary>
        /// The <see cref="Package.Any"/> property get method.
        /// </summary>
        private static MethodReference packageGetAnyMethod = null!;

        /// <summary>
        /// The <see cref="Object.Find{T}(Object, string)"/> method.
        /// </summary>
        private static MethodReference objectFindMethod = null!;

        /// <summary>
        /// The <see cref="CompoundMember.FindMember{T}(string)"/> method.
        /// </summary>
        private static MethodReference compoundMemberFindMemberMethod = null!;

        /// <summary>
        /// The <see cref="Class.FindMethod(Name)"/> method.
        /// </summary>
        private static MethodReference classFindMethodMethod = null!;

        /// <summary>
        /// The <see cref="Property.Offset"/> property get method.
        /// </summary>
        private static MethodReference propertyGetOffsetMethod = null!;

        /// <summary>
        /// The <see cref="Property.GetValue{T}(Object, int)"/> method.
        /// </summary>
        private static MethodReference propertyGetValueMethod = null!;

        /// <summary>
        /// The <see cref="Property.GetValuePtr{T}(Object, int)"/> method.
        /// </summary>
        private static MethodReference propertyGetValuePtrMethod = null!;

        /// <summary>
        /// The <see cref="Property.SetValue{T}(Object, T, int)"/> method.
        /// </summary>
        private static MethodReference propertySetValueMethod = null!;

        /// <summary>
        /// The <see cref="Method.Invoke(Object, void*)"/> method.
        /// </summary>
        private static MethodReference methodInvokeMethod = null!;

        /// <summary>
        /// The <see cref="ScriptDelegate.InvokeInternal(void*)"/> method.
        /// </summary>
        private static MethodReference scriptDelegateInvokeMethod = null!;

        /// <summary>
        /// The <see cref="DynamicMulticastDelegate.InvokeInternal(void*)"/> method.
        /// </summary>
        private static MethodReference dynamicMulticastDelegateInvokeMethod = null!;

        /// <summary>
        /// The <see cref="BooleanMarshaler.ToManaged(IntPtr, int)"/> method.
        /// </summary>
        private static MethodReference booleanMarshalerToManagedMethod = null!;

        /// <summary>
        /// The <see cref="BooleanMarshaler.ToNative(IntPtr, int, bool)"/> method.
        /// </summary>
        private static MethodReference booleanMarshalerToNativeMethod = null!;

        /// <summary>
        /// The <see cref="ObjectMarshaler.ToManaged(IntPtr)"/> method.
        /// </summary>
        private static MethodReference objectMarshalerToManagedMethod = null!;

        /// <summary>
        /// The <see cref="ObjectMarshaler.ToNative(IntPtr, Object)"/> method.
        /// </summary>
        private static MethodReference objectMarshalerToNativeMethod = null!;

        /// <summary>
        /// The <see cref="StringMarshaler.ToManaged(ScriptArray*)"/> method.
        /// </summary>
        private static MethodReference stringMarshalerToManagedMethod = null!;

        /// <summary>
        /// The <see cref="StringMarshaler.ToNative(ScriptArray*, string)"/> method.
        /// </summary>
        private static MethodReference stringMarshalerToNativeMethod = null!;

        /// <summary>
        /// The <see cref="TextMarshaler.ToManaged(IntPtr, int)"/> method.
        /// </summary>
        private static MethodReference textMarshalerToManagedMethod = null!;

        /// <summary>
        /// The <see cref="TextMarshaler.ToNative(IntPtr, int, Text)"/> method.
        /// </summary>
        private static MethodReference textMarshalerToNativeMethod = null!;

        /// <summary>
        /// The <see cref="TrivialMarshaler{T}.ToManaged(IntPtr, int)"/> method.
        /// </summary>
        private static MethodReference trivialMarshalerToManagedMethod = null!;

        /// <summary>
        /// The <see cref="TrivialMarshaler{T}.ToNative(IntPtr, int, T)"/> method.
        /// </summary>
        private static MethodReference trivialMarshalerToNativeMethod = null!;

        private static Dictionary<Class, TypeDefinition> classes = null!;
        private static Dictionary<Enum, TypeDefinition> enums = null!;
        private static Dictionary<Struct, TypeDefinition> structs = null!;
        private static Dictionary<Method, TypeDefinition> delegates = null!;
        private static Dictionary<Class, TypeDefinition> interfaces = null!;

        [UnmanagedCallersOnly]
        internal static void AddClass(IntPtr unrealClass)
        {
            var c = Object.Create<Class>(unrealClass);
            if (!classes.ContainsKey(c))
            {
                classes.Add(c, module.DefineType(default, default!, default, default!));
            }
        }

        private static TypeDefinition ExportClass(Class unrealClass)
        {
            TypeDefinition type = classes[unrealClass];
            if (!string.IsNullOrEmpty(type.Name))
            {
                return type;
            }

            // type = module.DefineType(null, GetName(unrealClass), CommonTypeAttributes, unrealObjectType);
            type.Name = GetName(unrealClass);
            type.Attributes = CommonTypeAttributes;
            type.BaseType = unrealObjectType;
            if (unrealClass.HasAnyFlags(ClassFlags.Abstract))
            {
                type.Attributes |= TypeAttributes.Abstract;
            }

            FieldDefinition classField = type.DefineField(ClassFieldName, CommonFieldAttributes, unrealClassType);
            MethodDefinition cctor = type.DefineTypeInitializer(voidType);
            ILProcessor cctorCode = cctor.Body.GetILProcessor();

            cctorCode.Emit(OpCodes.Call, packageGetAnyMethod);
            cctorCode.Emit(OpCodes.Ldstr, GetName(unrealClass));
            cctorCode.Emit(OpCodes.Call, objectFindMethod.MakeGenericMethod(unrealClassType));
            cctorCode.Emit(OpCodes.Stsfld, classField);

            ExportProperties(type, classField, cctorCode, unrealClass);
            ExportMethods(type, classField, cctorCode, unrealClass);

            cctorCode.Emit(OpCodes.Ret);

            foreach (ImplementedInterface implementedInterface in unrealClass.Interfaces)
            {
                TypeDefinition interfaceType = ExportInterface(implementedInterface.InterfaceClass);
                type.Interfaces.Add(new InterfaceImplementation(interfaceType));
            }

            // classes[unrealClass] = type;
            return type;
        }

        private static TypeDefinition ExportDelegate(DelegateMethod signatureMethod)
        {
            if (delegates.TryGetValue(signatureMethod, out TypeDefinition? type))
            {
                return type;
            }

            string typeName = signatureMethod.Name.ToString()[..^DelegateMethod.NameSuffix.Length];
            type = signatureMethod.Parent is Class unrealClass
                ? classes[unrealClass].DefineNestedType(typeName, CommonTypeAttributes, valueTypeType)
                : module.DefineType(null, typeName, CommonTypeAttributes, valueTypeType);

            type.Interfaces.Add(new InterfaceImplementation(idynamicDelegateType));

            FieldDefinition delegateField = type.DefineField("delegate", FieldAttributes.Private | FieldAttributes.InitOnly, scriptDelegateType);

            MethodDefinition getMethodNameMethod = type.DefineMethod(
                "get_" + nameof(IDynamicDelegate.MethodName), MethodAttributes.Public | MethodAttributes.SpecialName, nameType);

            ILProcessor getMethodNameMethodCode = getMethodNameMethod.Body.GetILProcessor();

            getMethodNameMethodCode.Emit(OpCodes.Ldarg_0);
            getMethodNameMethodCode.Emit(OpCodes.Ldflda, delegateField);
            getMethodNameMethodCode.Emit(OpCodes.Call, module.ImportReference(GetPropertyGetMethodInfo<ScriptDelegate>(nameof(ScriptDelegate.MethodName))));
            getMethodNameMethodCode.Emit(OpCodes.Ret);

            PropertyDefinition getMethodNameProperty = type.DefineProperty(
                nameof(IDynamicDelegate.MethodName), PropertyAttributes.None, nameType);

            getMethodNameProperty.GetMethod = getMethodNameMethod;

            MethodDefinition getTargetMethod = type.DefineMethod(
                "get_" + nameof(IDynamicDelegate.Target), MethodAttributes.Public | MethodAttributes.SpecialName, unrealObjectType);

            ILProcessor getTargetMethodCode = getTargetMethod.Body.GetILProcessor();

            getTargetMethodCode.Emit(OpCodes.Ldarg_0);
            getTargetMethodCode.Emit(OpCodes.Ldflda, delegateField);
            getTargetMethodCode.Emit(OpCodes.Call, module.ImportReference(GetPropertyGetMethodInfo<ScriptDelegate>(nameof(ScriptDelegate.Target))));
            getTargetMethodCode.Emit(OpCodes.Ret);

            PropertyDefinition getTargetProperty = type.DefineProperty(
                nameof(IDynamicDelegate.Target), PropertyAttributes.None, unrealObjectType);

            getTargetProperty.GetMethod = getTargetMethod;

            MethodDefinition invokeMethod = type.DefineMethod("Invoke", MethodAttributes.Public, voidType);

            VariableDefinition parametersLocal = invokeMethod.DeclareLocal(bytePointerType);

            ILProcessor methodCode = invokeMethod.Body.GetILProcessor();

            methodCode.Emit(OpCodes.Ldc_I4, signatureMethod.ParametersSize);
            methodCode.Emit(OpCodes.Localloc);
            methodCode.Emit(OpCodes.Stloc_S, parametersLocal);

            foreach (Property unrealParameter in signatureMethod.ParameterProperties)
            {
                TypeReference parameterType = GetPropertyType(unrealParameter);
                if (parameterType is null) continue;

                if (unrealParameter.IsReturnParameter)
                {
                    invokeMethod.ReturnType = parameterType;
                    continue;
                }

                // | C++              | C#           |
                // | ---------------- | ------------ |
                // | out              | out          |
                // | out ref          | ref          |
                // | out ref readonly | ref readonly |
                ParameterAttributes parameterAttributes = ParameterAttributes.None;
                if (unrealParameter.HasAnyFlags(PropertyFlags.OutParameter))
                {
                    parameterType = parameterType.MakeByReferenceType();

                    if (!unrealParameter.HasAnyFlags(PropertyFlags.ByReferenceParameter))
                    {
                        parameterAttributes |= ParameterAttributes.Out;
                    }
                    else if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                    {
                        parameterAttributes |= ParameterAttributes.In;
                    }
                }

                ParameterDefinition parameter = invokeMethod.DefineParameter(
                    GetName(unrealParameter), parameterAttributes, parameterType);

                if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                {
                    parameter.CustomAttributes.Add(new CustomAttribute(isReadOnlyAttributeConstructor));
                }
            }

            // methodCode.Emit(OpCodes.Call, scriptDelegateInvokeMethod);
            methodCode.Emit(OpCodes.Ret);

            delegates.Add(signatureMethod, type);
            return type;
        }

        private static TypeDefinition ExportEnum(Enum unrealEnum, TypeReference underlyingType)
        {
            if (enums.TryGetValue(unrealEnum, out TypeDefinition? type))
            {
                return type;
            }

            IEnumerable<KeyValuePair<Name, long>> namesAndValues = unrealEnum.NamesAndValues;

            /*
            int underscoreIndex = pairs.First().Key.ToString().IndexOf('_');
            bool hasCommonPrefix;
            string commonPrefix;
            if (underscoreIndex >= 0)
            {
                hasCommonPrefix = true;
                commonPrefix = pairs.First().Key.ToString().Substring(0, underscoreIndex + 1);
            }
            else
            {
                hasCommonPrefix = false;
                commonPrefix = null!;
            }
            */

            type = module.DefineEnum(null, GetName(unrealEnum), CommonTypeAttributes, underlyingType, enumType);

            bool hasMax = false;
            foreach (KeyValuePair<Name, long> pair in namesAndValues)
            {
                /*
                if (hasCommonPrefix)
                {
                    if (pair.Key.ToString().StartsWith(commonPrefix))
                    {
                        _ = new FieldDefinition(pair.Key.ToString().Substring(underscoreIndex + 1), FieldAttributes.InitOnly, default);
                    }
                    else
                    {
                        hasCommonPrefix = false;
                    }
                }
                */

                Debug.Assert(!hasMax);

                string name = pair.Key.ToString();
                string commonPrefix = unrealEnum.Name.ToString() + "::";

                if (!name.StartsWith(commonPrefix))
                {
                    _ = type.DefineLiteral(name, pair.Value, underlyingType);
                    continue;
                }

                Debug.Assert(name.StartsWith(commonPrefix));

                if (name.EndsWith("MAX"))
                {
                    hasMax = true;
                    continue;
                }

                _ = type.DefineLiteral(name[commonPrefix.Length..], pair.Value, underlyingType);
            }

            if (hasMax)
            {
                Debug.Assert(hasMax);
            }

            enums.Add(unrealEnum, type);
            return type;
        }

        private static TypeDefinition ExportInterface(Class unrealClass)
        {
            if (interfaces.TryGetValue(unrealClass, out TypeDefinition? type))
            {
                return type;
            }

            type = module.DefineType(null, "I" + GetName(unrealClass), TypeAttributes.Public | TypeAttributes.Interface, null!);
            interfaces.Add(unrealClass, type);

            foreach (Method unrealMethod in unrealClass.EnumerateMembers<Method>())
            {
                MethodDefinition method = type.DefineMethod(GetName(unrealMethod), MethodAttributes.Abstract, voidType);

                foreach (Property unrealParameter in unrealMethod.ParameterProperties)
                {
                    TypeReference parameterType = GetPropertyType(unrealParameter);
                    if (parameterType is null) continue;

                    if (unrealParameter.IsReturnParameter)
                    {
                        method.ReturnType = parameterType;
                        continue;
                    }

                    ParameterAttributes parameterAttributes = ParameterAttributes.None;
                    if (unrealParameter.HasAnyFlags(PropertyFlags.OutParameter))
                    {
                        parameterType = parameterType.MakeByReferenceType();

                        if (!unrealParameter.HasAnyFlags(PropertyFlags.ByReferenceParameter))
                        {
                            parameterAttributes |= ParameterAttributes.Out;
                        }
                        else if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                        {
                            parameterAttributes |= ParameterAttributes.In;
                        }
                    }

                    ParameterDefinition parameter = method.DefineParameter(
                        GetName(unrealParameter), parameterAttributes, parameterType);

                    if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                    {
                        parameter.CustomAttributes.Add(new CustomAttribute(isReadOnlyAttributeConstructor));
                    }
                }
            }

            return type;
        }

        private static void ExportMethodParameter(TypeDefinition type, FieldReference methodField, ILProcessor cctorCode, Method unrealMethod, Property unrealParameter)
        {
            FieldDefinition parameterOffsetField = type.DefineField(
                unrealMethod.Name + "_" + unrealParameter.Name + OffsetFieldSuffix, CommonFieldAttributes, int32Type);

            cctorCode.Emit(OpCodes.Ldsfld, methodField);
            cctorCode.Emit(OpCodes.Ldstr, unrealParameter.Name.ToString());
            cctorCode.Emit(OpCodes.Call, compoundMemberFindMemberMethod.MakeGenericMethod(unrealPropertyType));
            cctorCode.Emit(OpCodes.Call, propertyGetOffsetMethod);
            cctorCode.Emit(OpCodes.Stsfld, parameterOffsetField);
        }

        private static void ExportMethods(TypeDefinition type, FieldReference classField, ILProcessor cctorCode, Class unrealClass)
        {
            foreach (Method unrealMethod in unrealClass.EnumerateMembers<Method>())
            {
                FieldDefinition methodField = type.DefineField(
                    unrealMethod.Name + MethodFieldSuffix, CommonFieldAttributes, unrealMethodType);

                cctorCode.Emit(OpCodes.Ldsfld, classField);
                cctorCode.Emit(OpCodes.Ldstr, unrealMethod.Name.ToString());
                cctorCode.Emit(OpCodes.Call, classFindMethodMethod);
                cctorCode.Emit(OpCodes.Stsfld, methodField);

                MethodAttributes methodAttributes = MethodAttributes.CompilerControlled;

                if (unrealMethod.IsFamily)
                {
                    methodAttributes |= MethodAttributes.Family;
                }

                if (unrealMethod.IsPublic)
                {
                    methodAttributes |= MethodAttributes.Public;
                }

                if (unrealMethod.IsStatic)
                {
                    methodAttributes |= MethodAttributes.Static;
                }

                MethodDefinition method = type.DefineMethod(GetName(unrealMethod), methodAttributes, voidType);

                VariableDefinition parametersLocal = method.DeclareLocal(bytePointerType);

                ILProcessor methodCode = method.Body.GetILProcessor();

                methodCode.Emit(OpCodes.Ldc_I4, unrealMethod.ParametersSize);
                methodCode.Emit(OpCodes.Localloc);
                methodCode.Emit(OpCodes.Stloc_S, parametersLocal);

                Property[] unrealParameters = new Property[100];
                ParameterDefinition[] parameters = new ParameterDefinition[100];
                int count = 0;
                foreach (Property unrealParameter in unrealMethod.ParameterProperties)
                {
                    unrealParameters[count] = unrealParameter;
                    ExportMethodParameter(type, methodField, cctorCode, unrealMethod, unrealParameter);

                    TypeReference parameterType = GetPropertyType(unrealParameter);
                    if (parameterType is null)
                    {
                        count++;
                        continue;
                    }

                    if (unrealParameter.IsReturnParameter)
                    {
                        method.ReturnType = parameterType;
                        count++;
                        continue;
                    }

                    // | C++              | C#           |
                    // | ---------------- | ------------ |
                    // | out              | out          |
                    // | out ref          | ref          |
                    // | out ref readonly | ref readonly |
                    ParameterAttributes parameterAttributes = ParameterAttributes.None;
                    if (unrealParameter.HasAnyFlags(PropertyFlags.OutParameter))
                    {
                        parameterType = parameterType.MakeByReferenceType();

                        if (!unrealParameter.HasAnyFlags(PropertyFlags.ByReferenceParameter))
                        {
                            parameterAttributes |= ParameterAttributes.Out;
                        }
                        else if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                        {
                            parameterAttributes |= ParameterAttributes.In;
                        }
                    }

                    ParameterDefinition parameter = method.DefineParameter(
                        GetName(unrealParameter), parameterAttributes, parameterType);

                    parameters[count] = parameter;
                    count++;

                    if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                    {
                        parameter.CustomAttributes.Add(new CustomAttribute(isReadOnlyAttributeConstructor));
                    }

                    if (!unrealParameter.HasAnyFlags(PropertyFlags.OutParameter) ||
                        unrealParameter.HasAnyFlags(PropertyFlags.ByReferenceParameter))
                    {
                        methodCode.Emit(OpCodes.Ldloc_S, parametersLocal);
                        methodCode.Emit(OpCodes.Ldc_I4, unrealParameter.Offset);
                        methodCode.Emit(OpCodes.Add);
                        methodCode.Emit(OpCodes.Ldc_I4_0);
                        methodCode.Emit(OpCodes.Ldarg_S, parameter);
                        if (GetPropertyMarshalerToNativeMethod(unrealParameter) != null)
                        {
                            methodCode.Emit(OpCodes.Call, GetPropertyMarshalerToNativeMethod(unrealParameter));
                        }
                    }
                }

                methodCode.Emit(OpCodes.Ldsfld, methodField);
                methodCode.Emit(OpCodes.Ldarg_0);
                methodCode.Emit(OpCodes.Ldloc_S, parametersLocal);
                methodCode.Emit(OpCodes.Call, methodInvokeMethod);

                for (int i = 0; i < count; i++)
                {
                    Property unrealParameter = unrealParameters[i];
                    ParameterDefinition parameter = parameters[i];

                    if (parameter is null)
                    {
                        continue;
                    }

                    if (unrealParameter.HasAnyFlags(PropertyFlags.OutParameter) && !unrealParameter.IsReturnParameter)
                    {
                        methodCode.Emit(OpCodes.Ldloc_S, parametersLocal);
                        methodCode.Emit(OpCodes.Ldc_I4, unrealParameter.Offset);
                        methodCode.Emit(OpCodes.Add);
                        methodCode.Emit(OpCodes.Ldc_I4_0);
                        if (GetPropertyMarshalerToManagedMethod(unrealParameter) != null)
                        {
                            methodCode.Emit(OpCodes.Call, GetPropertyMarshalerToManagedMethod(unrealParameter));
                        }

                        methodCode.Emit(OpCodes.Starg_S, parameter);
                    }
                }

                methodCode.Emit(OpCodes.Ret);
            }
        }

        private static TypeDefinition ExportMulticastDelegate(DelegateMethod signatureMethod)
        {
            if (delegates.TryGetValue(signatureMethod, out TypeDefinition? type))
            {
                return type;
            }

            string typeName = signatureMethod.Name.ToString()[..^DelegateMethod.NameSuffix.Length];
            type = signatureMethod.Parent is Class unrealClass
                ? classes[unrealClass].DefineNestedType(typeName, CommonTypeAttributes, dynamicMulticastDelegateType)
                : module.DefineType(null, typeName, CommonTypeAttributes, dynamicMulticastDelegateType);

            MethodDefinition invokeMethod = type.DefineMethod("Invoke", MethodAttributes.Public, voidType);

            VariableDefinition parametersLocal = invokeMethod.DeclareLocal(bytePointerType);

            ILProcessor methodCode = invokeMethod.Body.GetILProcessor();

            methodCode.Emit(OpCodes.Ldc_I4, signatureMethod.ParametersSize);
            methodCode.Emit(OpCodes.Localloc);
            methodCode.Emit(OpCodes.Stloc_S, parametersLocal);

            foreach (Property unrealParameter in signatureMethod.ParameterProperties)
            {
                TypeReference parameterType = GetPropertyType(unrealParameter);
                if (parameterType is null) continue;

                if (unrealParameter.IsReturnParameter)
                {
                    invokeMethod.ReturnType = parameterType;
                    continue;
                }

                // | C++              | C#           |
                // | ---------------- | ------------ |
                // | out              | out          |
                // | out ref          | ref          |
                // | out ref readonly | ref readonly |
                ParameterAttributes parameterAttributes = ParameterAttributes.None;
                if (unrealParameter.HasAnyFlags(PropertyFlags.OutParameter))
                {
                    parameterType = parameterType.MakeByReferenceType();

                    if (!unrealParameter.HasAnyFlags(PropertyFlags.ByReferenceParameter))
                    {
                        parameterAttributes |= ParameterAttributes.Out;
                    }
                    else if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                    {
                        parameterAttributes |= ParameterAttributes.In;
                    }
                }

                ParameterDefinition parameter = invokeMethod.DefineParameter(
                    GetName(unrealParameter), parameterAttributes, parameterType);

                if (unrealParameter.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                {
                    parameter.CustomAttributes.Add(new CustomAttribute(isReadOnlyAttributeConstructor));
                }
            }

            // methodCode.Emit(OpCodes.Call, multicastDynamicDelegateInvokeMethod);
            methodCode.Emit(OpCodes.Ret);

            delegates.Add(signatureMethod, type);
            return type;
        }

        private static void ExportProperties(TypeDefinition type, FieldReference classField, ILProcessor cctorCode, Class unrealClass)
        {
            foreach (Property unrealProperty in unrealClass.EnumerateProperties<Property>())
            {
                string propertyName = GetName(unrealProperty);

                FieldDefinition propertyField = type.DefineField(
                    propertyName + PropertyFieldSuffix, CommonFieldAttributes, unrealPropertyType);

                cctorCode.Emit(OpCodes.Ldsfld, classField);
                cctorCode.Emit(OpCodes.Ldstr, propertyName);
                cctorCode.Emit(OpCodes.Call, compoundMemberFindMemberMethod.MakeGenericMethod(unrealPropertyType));
                cctorCode.Emit(OpCodes.Stsfld, propertyField);

                TypeReference propertyType = GetPropertyType(unrealProperty);
                if (propertyType is null) continue;
                if (unrealProperty.ArrayLength == 1)
                {
                    MethodDefinition getMethod = type.DefineMethod(
                        "get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, propertyType);

                    ILProcessor getMethodCode = getMethod.Body.GetILProcessor();

                    getMethodCode.Emit(OpCodes.Ldsfld, propertyField);
                    getMethodCode.Emit(OpCodes.Ldarg_0);
                    getMethodCode.Emit(OpCodes.Ldc_I4_0);
                    getMethodCode.Emit(OpCodes.Call, propertyGetValueMethod.MakeGenericMethod(propertyType));
                    getMethodCode.Emit(OpCodes.Ret);

                    MethodDefinition setMethod = type.DefineMethod(
                        "set_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, voidType);

                    setMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, propertyType));

                    ILProcessor setMethodCode = setMethod.Body.GetILProcessor();

                    setMethodCode.Emit(OpCodes.Ldsfld, propertyField);
                    setMethodCode.Emit(OpCodes.Ldarg_0);
                    setMethodCode.Emit(OpCodes.Ldarg_1);
                    setMethodCode.Emit(OpCodes.Ldc_I4_0);
                    setMethodCode.Emit(OpCodes.Call, propertySetValueMethod.MakeGenericMethod(propertyType));
                    setMethodCode.Emit(OpCodes.Ret);

                    PropertyDefinition property = type.DefineProperty(propertyName, PropertyAttributes.None, propertyType);

                    property.GetMethod = getMethod;
                    property.SetMethod = setMethod;
                }
                else
                {
                    TypeReference elementType = propertyType;

                    propertyType = fixedSizeArrayType.MakeGenericType(elementType);

                    MethodDefinition getMethod = type.DefineMethod(
                        "get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, propertyType);

                    ILProcessor getMethodCode = getMethod.Body.GetILProcessor();

                    getMethodCode.Emit(OpCodes.Ldsfld, propertyField);
                    getMethodCode.Emit(OpCodes.Ldarg_0);
                    getMethodCode.Emit(OpCodes.Ldc_I4_0);
                    getMethodCode.Emit(OpCodes.Call, propertyGetValuePtrMethod.MakeGenericMethod(elementType));
                    getMethodCode.Emit(OpCodes.Ldc_I4, unrealProperty.ArrayLength);
                    getMethodCode.Emit(OpCodes.Newobj, fixedSizeArrayConstructor.MakeGenericMethod(elementType));
                    getMethodCode.Emit(OpCodes.Ret);

                    PropertyDefinition property = type.DefineProperty(propertyName, PropertyAttributes.None, propertyType);

                    property.GetMethod = getMethod;
                }
            }
        }

        private static TypeDefinition ExportStruct(StructProperty structProperty)
        {
            Struct unrealStruct = structProperty.MetaStruct;
            if (structs.TryGetValue(unrealStruct, out TypeDefinition? type))
            {
                return type;
            }

            type = module.DefineType(null, GetName(unrealStruct), CommonTypeAttributes | TypeAttributes.ExplicitLayout, valueTypeType);
            structs.Add(unrealStruct, type);

            FieldDefinition structField = type.DefineField("Struct", CommonFieldAttributes, module.ImportReference(typeof(Struct)));
            MethodDefinition cctor = type.DefineTypeInitializer(voidType);
            ILProcessor cctorCode = cctor.Body.GetILProcessor();

            cctorCode.Emit(OpCodes.Call, packageGetAnyMethod);
            cctorCode.Emit(OpCodes.Ldstr, GetName(unrealStruct));
            cctorCode.Emit(OpCodes.Call, objectFindMethod.MakeGenericMethod(module.ImportReference(typeof(Struct))));
            cctorCode.Emit(OpCodes.Stsfld, structField);

            foreach (Property unrealProperty in unrealStruct.EnumerateProperties<Property>())
            {
                string propertyName = GetName(unrealProperty);

                FieldDefinition propertyField = type.DefineField(
                    propertyName + PropertyFieldSuffix, CommonFieldAttributes, unrealPropertyType);

                cctorCode.Emit(OpCodes.Ldsfld, structField);
                cctorCode.Emit(OpCodes.Ldstr, propertyName);
                cctorCode.Emit(OpCodes.Call, compoundMemberFindMemberMethod.MakeGenericMethod(unrealPropertyType));
                cctorCode.Emit(OpCodes.Stsfld, propertyField);

                TypeReference propertyType = GetPropertyType(unrealProperty);
                if (propertyType is null) continue;

                if (structProperty.HasAnyFlags(PropertyFlags.Trivial))
                {
                    FieldDefinition backingField = type.DefineField(
                        propertyName + "_Field", FieldAttributes.Private, propertyType);

                    backingField.Offset = unrealProperty.Offset;

                    if (unrealProperty.ArrayLength == 1)
                    {
                        MethodDefinition getMethod = type.DefineMethod(
                            "get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, propertyType);

                        ILProcessor getMethodCode = getMethod.Body.GetILProcessor();

                        getMethodCode.Emit(OpCodes.Ldarg_0);
                        getMethodCode.Emit(OpCodes.Ldfld, backingField);
                        getMethodCode.Emit(OpCodes.Ret);

                        MethodDefinition setMethod = type.DefineMethod(
                            "set_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, voidType);

                        setMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, propertyType));

                        ILProcessor setMethodCode = setMethod.Body.GetILProcessor();

                        setMethodCode.Emit(OpCodes.Ldarg_0);
                        setMethodCode.Emit(OpCodes.Ldarg_1);
                        setMethodCode.Emit(OpCodes.Stfld, backingField);
                        setMethodCode.Emit(OpCodes.Ret);

                        PropertyDefinition property = type.DefineProperty(propertyName, PropertyAttributes.None, propertyType);

                        property.GetMethod = getMethod;
                        property.SetMethod = setMethod;
                    }
                }
                else
                {
                    if (unrealProperty.ArrayLength == 1)
                    {
                        MethodDefinition getMethod = type.DefineMethod(
                            "get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, propertyType);

                        ILProcessor getMethodCode = getMethod.Body.GetILProcessor();

                        getMethodCode.Emit(OpCodes.Ldsfld, propertyField);
                        getMethodCode.Emit(OpCodes.Ldarg_0);
                        getMethodCode.Emit(OpCodes.Ldc_I4_0);
                        getMethodCode.Emit(OpCodes.Call, propertyGetValueMethod.MakeGenericMethod(propertyType));
                        getMethodCode.Emit(OpCodes.Ret);

                        MethodDefinition setMethod = type.DefineMethod(
                            "set_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName, voidType);

                        setMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, propertyType));

                        ILProcessor setMethodCode = setMethod.Body.GetILProcessor();

                        setMethodCode.Emit(OpCodes.Ldsfld, propertyField);
                        setMethodCode.Emit(OpCodes.Ldarg_0);
                        setMethodCode.Emit(OpCodes.Ldarg_1);
                        setMethodCode.Emit(OpCodes.Ldc_I4_0);
                        setMethodCode.Emit(OpCodes.Call, propertySetValueMethod.MakeGenericMethod(propertyType));
                        setMethodCode.Emit(OpCodes.Ret);

                        PropertyDefinition property = type.DefineProperty(propertyName, PropertyAttributes.None, propertyType);

                        property.GetMethod = getMethod;
                        property.SetMethod = setMethod;
                    }
                }
            }

            cctorCode.Emit(OpCodes.Ret);

            return type;
        }

        [UnmanagedCallersOnly]
        internal static void FinishExport()
        {
            _ = Debugger.Launch();

            foreach (KeyValuePair<Class, TypeDefinition> pair in classes)
            {
                if (string.IsNullOrEmpty(pair.Value.Name))
                {
                    _ = ExportClass(pair.Key);
                }
            }

            GenerateEntryPoint();

            // document.Save(XmlDocumentFileName);
            module.Write(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(SR.Assembly.GetExecutingAssembly().Location)!, ModuleName));
        }

        private static void GenerateEntryPoint()
        {
            TypeDefinition entryPointType = module.DefineType(null, "EntryPoint", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed, module.TypeSystem.Object);

            MethodDefinition loadMethod = entryPointType.DefineMethod("Load", MethodAttributes.Assembly | MethodAttributes.Static, voidType);
            MethodDefinition unloadMethod = entryPointType.DefineMethod("Unload", MethodAttributes.Assembly | MethodAttributes.Static, voidType);

            ILProcessor loadMethodCode = loadMethod.Body.GetILProcessor();
            ILProcessor unloadMethodCode = unloadMethod.Body.GetILProcessor();

            ExportClasses();

            loadMethodCode.Emit(OpCodes.Ret);
            unloadMethodCode.Emit(OpCodes.Ret);

            void ExportClasses()
            {
                TypeDefinition classesType = module.DefineType(null, "ScriptClasses", TypeAttributes.NotPublic | TypeAttributes.Abstract | TypeAttributes.Sealed, module.TypeSystem.Object);

                ILProcessor cctorCode = classesType.DefineTypeInitializer(voidType).Body.GetILProcessor();

                MethodDefinition registerAllMethod = classesType.DefineMethod("Register", MethodAttributes.Assembly | MethodAttributes.Static, voidType);
                MethodDefinition unregisterAllMethod = classesType.DefineMethod("Unregister", MethodAttributes.Assembly | MethodAttributes.Static, voidType);

                loadMethodCode.Emit(OpCodes.Call, registerAllMethod);
                unloadMethodCode.Emit(OpCodes.Call, unregisterAllMethod);

                ILProcessor registerAllMethodCode = registerAllMethod.Body.GetILProcessor();
                ILProcessor unregisterAllMethodCode = unregisterAllMethod.Body.GetILProcessor();

                MethodReference registerMethod = module.ImportReference(GetMethodInfo(typeof(Classes), nameof(Classes.Register)));
                MethodReference unregisterMethod = module.ImportReference(GetMethodInfo(typeof(Classes), nameof(Classes.Unregister)));
                foreach ((Class unrealClass, TypeDefinition type) in classes)
                {
                    FieldDefinition classField = classesType.DefineField(type.Name + "_Class", CommonFieldAttributes, intPtrType);

                    cctorCode.Emit(OpCodes.Ldtoken, unrealClassType);
                    cctorCode.Emit(OpCodes.Call, module.ImportReference(GetMethodInfo<Type>(nameof(Type.GetTypeFromHandle))));
                    cctorCode.Emit(OpCodes.Call, module.ImportReference(GetMethodInfo(typeof(Classes), nameof(Classes.GetClass))));
                    cctorCode.Emit(OpCodes.Ldstr, unrealClass.GetPathName());
                    cctorCode.Emit(OpCodes.Call, module.ImportReference(GetMethodInfo(typeof(Class.NativeMethods), nameof(Class.NativeMethods.FindObject))));
                    cctorCode.Emit(OpCodes.Stsfld, classField);

                    registerAllMethodCode.Emit(OpCodes.Ldsfld, classField);
                    registerAllMethodCode.Emit(OpCodes.Ldtoken, type);
                    registerAllMethodCode.Emit(OpCodes.Call, module.ImportReference(GetMethodInfo<Type>(nameof(Type.GetTypeFromHandle))));
                    registerAllMethodCode.Emit(OpCodes.Call, registerMethod);

                    unregisterAllMethodCode.Emit(OpCodes.Ldsfld, classField);
                    unregisterAllMethodCode.Emit(OpCodes.Ldtoken, type);
                    unregisterAllMethodCode.Emit(OpCodes.Call, module.ImportReference(GetMethodInfo<Type>(nameof(Type.GetTypeFromHandle))));
                    unregisterAllMethodCode.Emit(OpCodes.Call, unregisterMethod);
                }

                cctorCode.Emit(OpCodes.Ret);
                registerAllMethodCode.Emit(OpCodes.Ret);
                unregisterAllMethodCode.Emit(OpCodes.Ret);
            }
        }

        private static SR.ConstructorInfo GetConstructorInfo(Type type, params Type[] types) =>
            type.GetConstructor(types)!;

        private static SR.ConstructorInfo GetConstructorInfo<T>() =>
            typeof(T).GetConstructor(Type.EmptyTypes)!;

        private static SR.MethodInfo GetMethodInfo(Type type, string name) =>
            type.GetMethod(name, LookupAllAttributes)!;

        private static SR.MethodInfo GetMethodInfo<T>(string name) =>
            GetMethodInfo(typeof(T), name);

        private static SR.MethodInfo GetMethodInfo<T>(string name, params Type[] types) =>
            typeof(T).GetMethod(name, LookupAllAttributes, null, types, null)!;

        private static SR.MethodInfo GetPropertyGetMethodInfo<T>(string name) =>
            typeof(T).GetProperty(name, LookupAllAttributes)!.GetGetMethod()!;

        private static TypeReference GetPropertyMarshaler(Property unrealProperty) => unrealProperty switch
        {
            NumericProperty _ => trivialMarshalerType.MakeGenericType(GetPropertyType(unrealProperty)),

            EnumProperty enumProperty => trivialMarshalerType.MakeGenericType(GetPropertyType(enumProperty.UnderlyingProperty)),

            BooleanProperty _ => booleanMarshalerType,

            NameProperty _ => trivialMarshalerType.MakeGenericType(nameType),
            StringProperty _ => stringMarshalerType,
            TextProperty _ => textMarshalerType,

            /*StructProperty _ => default!,*/

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => objectMarshalerType,
                WeakObjectProperty _ => trivialMarshalerType.MakeGenericType(GetPropertyType(unrealProperty)),
                LazyObjectProperty _ => trivialMarshalerType.MakeGenericType(GetPropertyType(unrealProperty)),
                SoftObjectProperty _ => trivialMarshalerType.MakeGenericType(GetPropertyType(unrealProperty)),
                _ => null!
            },

            /*ArrayProperty arrayProperty => typeof(Array<>)
            /*.MakeGenericType(GetPropertyType(arrayProperty.ItemProperty)),

            MapProperty mapProperty => typeof(Map<,>)
            /*.MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => typeof(Set<>)
            /*.MakeGenericType(GetPropertyType(setProperty.ItemProperty)),*/

            DelegateProperty _ => null!,

            MulticastDelegateProperty _ => null!,

            _ => null!// throw new NotImplementedException(),
        };

        private static MethodReference GetPropertyMarshalerToManagedMethod(Property unrealProperty) => unrealProperty switch
        {
            NumericProperty _ => trivialMarshalerToManagedMethod.WithGenericType(GetPropertyType(unrealProperty)),

            EnumProperty enumProperty => trivialMarshalerToManagedMethod.WithGenericType(GetPropertyType(enumProperty.UnderlyingProperty)),

            BooleanProperty _ => booleanMarshalerToManagedMethod,

            NameProperty _ => trivialMarshalerToManagedMethod.WithGenericType(nameType),
            StringProperty _ => stringMarshalerToManagedMethod,
            TextProperty _ => textMarshalerToManagedMethod,

            /*StructProperty _ => default!,*/

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => objectMarshalerToManagedMethod,
                WeakObjectProperty _ => trivialMarshalerToManagedMethod.WithGenericType(GetPropertyType(unrealProperty)),
                LazyObjectProperty _ => trivialMarshalerToManagedMethod.WithGenericType(GetPropertyType(unrealProperty)),
                SoftObjectProperty _ => trivialMarshalerToManagedMethod.WithGenericType(GetPropertyType(unrealProperty)),
                _ => null!
            },

            /*ArrayProperty arrayProperty => typeof(Array<>)
            /*.MakeGenericType(GetPropertyType(arrayProperty.ItemProperty)),

            MapProperty mapProperty => typeof(Map<,>)
            /*.MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => typeof(Set<>)
            /*.MakeGenericType(GetPropertyType(setProperty.ItemProperty)),*/

            DelegateProperty _ => null!,

            MulticastDelegateProperty _ => null!,

            _ => null!// throw new NotImplementedException(),
        };

        private static MethodReference GetPropertyMarshalerToNativeMethod(Property unrealProperty) => unrealProperty switch
        {
            NumericProperty _ => trivialMarshalerToNativeMethod.WithGenericType(GetPropertyType(unrealProperty)),

            EnumProperty enumProperty => trivialMarshalerToNativeMethod.WithGenericType(GetPropertyType(enumProperty.UnderlyingProperty)),

            BooleanProperty _ => booleanMarshalerToNativeMethod,

            NameProperty _ => trivialMarshalerToNativeMethod.WithGenericType(nameType),
            StringProperty _ => stringMarshalerToNativeMethod,
            TextProperty _ => textMarshalerToNativeMethod,

            /*StructProperty _ => default!,*/

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => objectMarshalerToNativeMethod,
                WeakObjectProperty _ => trivialMarshalerToNativeMethod.WithGenericType(GetPropertyType(unrealProperty)),
                LazyObjectProperty _ => trivialMarshalerToNativeMethod.WithGenericType(GetPropertyType(unrealProperty)),
                SoftObjectProperty _ => trivialMarshalerToNativeMethod.WithGenericType(GetPropertyType(unrealProperty)),
                _ => null!
            },

            /*ArrayProperty arrayProperty => typeof(Array<>)
            /*.MakeGenericType(GetPropertyType(arrayProperty.ItemProperty)),

            MapProperty mapProperty => typeof(Map<,>)
            /*.MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => typeof(Set<>)
            /*.MakeGenericType(GetPropertyType(setProperty.ItemProperty)),*/

            DelegateProperty _ => null!,

            MulticastDelegateProperty _ => null!,

            _ => null!// throw new NotImplementedException(),
        };

        private static TypeReference GetPropertyType(Property unrealProperty) => unrealProperty switch
        {
            NumericProperty numericProperty => numericProperty switch
            {
                SByteProperty _ => sbyteType,
                ByteProperty byteProperty => byteProperty.MetaEnum is null ? byteType : ExportEnum(byteProperty.MetaEnum, byteType),
                Int16Property _ => int16Type,
                UInt16Property _ => uint16Type,
                Int32Property _ => int32Type,
                UInt32Property _ => uint32Type,
                Int64Property _ => int64Type,
                UInt64Property _ => uint64Type,
                SingleProperty _ => singleType,
                DoubleProperty _ => doubleType,
                _ => throw new NotImplementedException()
            },

            EnumProperty enumProperty => ExportEnum(enumProperty.MetaEnum, GetPropertyType(enumProperty.UnderlyingProperty)),

            BooleanProperty _ => nativeBooleanType,

            NameProperty _ => nameType,
            StringProperty _ => stringType,
            TextProperty _ => textType,

            StructProperty structProperty => ExportStruct(structProperty),

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => unrealObjectType,

                WeakObjectProperty _ => weakObjectReferenceTType
                .MakeGenericType(ExportClass(objectPropertyBase.PropertyClass)),

                LazyObjectProperty _ => lazyObjectReferenceTType
                .MakeGenericType(ExportClass(objectPropertyBase.PropertyClass)),

                SoftObjectProperty _ => softObjectReferenceTType
                .MakeGenericType(ExportClass(objectPropertyBase.PropertyClass)),

                _ => throw new NotImplementedException()
            },

            InterfaceProperty interfaceProperty => ExportInterface(interfaceProperty.InterfaceClass),

            ArrayProperty arrayProperty => unrealArrayType
            .MakeGenericType(GetPropertyType(arrayProperty.ItemProperty)),

            MapProperty mapProperty => unrealMapType
            .MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => unrealSetType
            .MakeGenericType(GetPropertyType(setProperty.ItemProperty)),

            DelegateProperty delegateProperty => ExportDelegate(delegateProperty.SignatureMethod),

            MulticastDelegateProperty multicastDelegateProperty => multicastDelegateProperty switch
            {
                MulticastInlineDelegateProperty multicastInlineDelegateProperty =>
                ExportMulticastDelegate(multicastInlineDelegateProperty.SignatureMethod),

                MulticastSparseDelegateProperty _ => default!,
                _ => throw new NotImplementedException()
            },

            _ => throw new NotImplementedException(),
        };

        private static string GetName(Member member) =>
            /*member.TryGetMetaData(ScriptNameMetaDataKey, out string? name) ? name :*/ member.Name.ToString();

        private static string GetName(Property property) => property.Name.ToString();

        [UnmanagedCallersOnly]
        internal static void Initialize()
        {
            ModuleDefinition localModule = ModuleDefinition.CreateModule(ModuleName, ModuleKind.Dll);
            module = localModule;

            document = new XmlDocument();

            voidType = localModule.TypeSystem.Void;
            sbyteType = localModule.TypeSystem.SByte;
            byteType = localModule.TypeSystem.Byte;
            int16Type = localModule.TypeSystem.Int16;
            uint16Type = localModule.TypeSystem.UInt16;
            int32Type = localModule.TypeSystem.Int32;
            uint32Type = localModule.TypeSystem.UInt32;
            int64Type = localModule.TypeSystem.Int64;
            uint64Type = localModule.TypeSystem.UInt64;
            singleType = localModule.TypeSystem.Single;
            doubleType = localModule.TypeSystem.Double;
            stringType = localModule.TypeSystem.String;
            intPtrType = localModule.TypeSystem.IntPtr;

            bytePointerType = new PointerType(byteType);

            valueTypeType = localModule.ImportReference(typeof(ValueType));
            enumType = localModule.ImportReference(typeof(System.Enum));

            nativeBooleanType = localModule.ImportReference(typeof(NativeBoolean));
            fixedSizeArrayType = localModule.ImportReference(typeof(FixedSizeArray<>));

            nameType = localModule.ImportReference(typeof(Name));
            textType = localModule.ImportReference(typeof(Text));

            unrealObjectType = localModule.ImportReference(typeof(Object));
            unrealClassType = localModule.ImportReference(typeof(Class));
            unrealPropertyType = localModule.ImportReference(typeof(Property));
            unrealMethodType = localModule.ImportReference(typeof(Method));
            weakObjectReferenceTType = localModule.ImportReference(typeof(WeakObjectReference<>));
            lazyObjectReferenceTType = localModule.ImportReference(typeof(LazyObjectReference<>));
            softObjectReferenceTType = localModule.ImportReference(typeof(SoftObjectReference<>));

            unrealArrayType = localModule.ImportReference(typeof(Array<>));
            unrealMapType = localModule.ImportReference(typeof(Map<,>));
            unrealSetType = localModule.ImportReference(typeof(Set<>));

            scriptDelegateType = localModule.ImportReference(typeof(ScriptDelegate));
            idynamicDelegateType = localModule.ImportReference(typeof(IDynamicDelegate));
            dynamicMulticastDelegateType = localModule.ImportReference(typeof(DynamicMulticastDelegate));

            booleanMarshalerType = localModule.ImportReference(typeof(BooleanMarshaler));
            objectMarshalerType = localModule.ImportReference(typeof(ObjectMarshaler));
            stringMarshalerType = localModule.ImportReference(typeof(StringMarshaler));
            textMarshalerType = localModule.ImportReference(typeof(TextMarshaler));
            trivialMarshalerType = localModule.ImportReference(typeof(TrivialMarshaler<>));

            isReadOnlyAttributeConstructor = localModule.ImportReference(GetConstructorInfo<IsReadOnlyAttribute>());

            fixedSizeArrayConstructor = localModule.ImportReference(GetConstructorInfo(typeof(FixedSizeArray<>), typeof(IntPtr), typeof(int)));

            packageGetAnyMethod = localModule.ImportReference(GetPropertyGetMethodInfo<Package>(nameof(Package.Any)));
            objectFindMethod = localModule.ImportReference(GetMethodInfo<Object>(nameof(Object.Find)));
            compoundMemberFindMemberMethod = localModule.ImportReference(GetMethodInfo<CompoundMember>(nameof(CompoundMember.FindMember), typeof(string)));
            classFindMethodMethod = localModule.ImportReference(GetMethodInfo<Class>(nameof(Class.FindMethod)));
            propertyGetOffsetMethod = localModule.ImportReference(GetPropertyGetMethodInfo<Property>(nameof(Property.Offset)));
            propertyGetValueMethod = localModule.ImportReference(GetMethodInfo<Property>(nameof(Property.GetValue)));
            propertyGetValuePtrMethod = localModule.ImportReference(GetMethodInfo<Property>(nameof(Property.GetValuePtr)));
            propertySetValueMethod = localModule.ImportReference(GetMethodInfo<Property>(nameof(Property.SetValue)));
            methodInvokeMethod = localModule.ImportReference(GetMethodInfo<Method>(nameof(Method.Invoke)));
            scriptDelegateInvokeMethod = localModule.ImportReference(GetMethodInfo<ScriptDelegate>(nameof(ScriptDelegate.InvokeInternal)));
            dynamicMulticastDelegateInvokeMethod = localModule.ImportReference(GetMethodInfo<DynamicMulticastDelegate>("InvokeInternal"));

            booleanMarshalerToManagedMethod = localModule.ImportReference(GetMethodInfo(typeof(BooleanMarshaler), nameof(BooleanMarshaler.ToManaged)));
            booleanMarshalerToNativeMethod = localModule.ImportReference(GetMethodInfo(typeof(BooleanMarshaler), nameof(BooleanMarshaler.ToNative)));
            objectMarshalerToManagedMethod = localModule.ImportReference(GetMethodInfo(typeof(ObjectMarshaler), nameof(ObjectMarshaler.ToManaged)));
            objectMarshalerToNativeMethod = localModule.ImportReference(GetMethodInfo(typeof(ObjectMarshaler), nameof(ObjectMarshaler.ToNative)));
            stringMarshalerToManagedMethod = localModule.ImportReference(GetMethodInfo(typeof(StringMarshaler), nameof(StringMarshaler.ToManaged)));
            stringMarshalerToNativeMethod = localModule.ImportReference(GetMethodInfo(typeof(StringMarshaler), nameof(StringMarshaler.ToNative)));
            textMarshalerToManagedMethod = localModule.ImportReference(GetMethodInfo(typeof(TextMarshaler), nameof(TextMarshaler.ToManaged)));
            textMarshalerToNativeMethod = localModule.ImportReference(GetMethodInfo(typeof(TextMarshaler), nameof(TextMarshaler.ToNative)));
            trivialMarshalerToManagedMethod = localModule.ImportReference(GetMethodInfo(typeof(TrivialMarshaler<>), nameof(TrivialMarshaler<IntPtr>.ToManaged)));
            trivialMarshalerToNativeMethod = localModule.ImportReference(GetMethodInfo(typeof(TrivialMarshaler<>), nameof(TrivialMarshaler<IntPtr>.ToNative)));

            classes = new Dictionary<Class, TypeDefinition>();

            enums = new Dictionary<Enum, TypeDefinition>();
            structs = new Dictionary<Struct, TypeDefinition>();
            delegates = new Dictionary<Method, TypeDefinition>();
            interfaces = new Dictionary<Class, TypeDefinition>();
        }
    }
}
