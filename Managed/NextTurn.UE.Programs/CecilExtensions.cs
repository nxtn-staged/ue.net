// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NextTurn.UE.Programs
{
    internal static class CecilExtensions
    {
        internal static VariableDefinition DeclareLocal(
            this MethodDefinition method,
            TypeReference localType)
        {
            VariableDefinition result = new VariableDefinition(localType);
            method.Body.Variables.Add(result);
            return result;
        }

        internal static TypeDefinition DefineEnum(
            this ModuleDefinition module,
            string? @namespace,
            string name,
            TypeAttributes attributes,
            TypeReference underlyingType,
            TypeReference enumType)
        {
            const FieldAttributes UnderlyingFieldAttributes =
                FieldAttributes.Public |
                FieldAttributes.SpecialName |
                FieldAttributes.RTSpecialName;

            TypeDefinition result = DefineType(module, @namespace, name, attributes, enumType);
            FieldDefinition underlyingField = new FieldDefinition("value__", UnderlyingFieldAttributes, underlyingType);
            result.Fields.Add(underlyingField);
            return result;
        }

        internal static FieldDefinition DefineField(
            this TypeDefinition type,
            string name,
            FieldAttributes attributes,
            TypeReference fieldType)
        {
            FieldDefinition result = new FieldDefinition(name, attributes, fieldType);
            type.Fields.Add(result);
            return result;
        }

        internal static FieldDefinition DefineLiteral(
            this TypeDefinition type,
            string literalName,
            object literalValue,
            TypeReference underlyingType)
        {
            const FieldAttributes LiteralFieldAttributes =
                FieldAttributes.Public |
                FieldAttributes.Static |
                FieldAttributes.Literal;

            FieldDefinition result = new FieldDefinition(literalName, LiteralFieldAttributes, underlyingType)
            {
                Constant = literalValue,
            };

            type.Fields.Add(result);
            return result;
        }

        internal static MethodDefinition DefineMethod(
            this TypeDefinition type,
            string name,
            MethodAttributes attributes,
            TypeReference returnType)
        {
            MethodDefinition result = new MethodDefinition(name, attributes, returnType);
            type.Methods.Add(result);
            return result;
        }

        internal static TypeDefinition DefineNestedType(
            this TypeDefinition type,
            string name,
            TypeAttributes attributes,
            TypeReference baseType)
        {
            TypeDefinition result = new TypeDefinition(null, name, attributes, baseType);
            type.NestedTypes.Add(result);
            return result;
        }

        internal static ParameterDefinition DefineParameter(
            this MethodDefinition method,
            string? name,
            ParameterAttributes attributes,
            TypeReference parameterType)
        {
            ParameterDefinition result = new ParameterDefinition(name, attributes, parameterType);
            method.Parameters.Add(result);
            return result;
        }

        internal static PropertyDefinition DefineProperty(
            this TypeDefinition type,
            string name,
            PropertyAttributes attributes,
            TypeReference propertyType)
        {
            PropertyDefinition result = new PropertyDefinition(name, attributes, propertyType);
            type.Properties.Add(result);
            return result;
        }

        internal static TypeDefinition DefineType(
            this ModuleDefinition module,
            string? @namespace,
            string name,
            TypeAttributes attributes,
            TypeReference baseType)
        {
            TypeDefinition result = new TypeDefinition(@namespace, name, attributes, baseType);
            module.Types.Add(result);
            return result;
        }

        internal static MethodDefinition DefineTypeInitializer(this TypeDefinition type, TypeReference voidType)
        {
            const MethodAttributes TypeInitializerAttributes =
                MethodAttributes.Private |
                MethodAttributes.Static |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName;

            MethodDefinition result = new MethodDefinition(".cctor", TypeInitializerAttributes, voidType);
            type.Methods.Add(result);
            return result;
        }

        internal static MethodReference MakeGenericMethod(this MethodReference method, params TypeReference[] typeArguments)
        {
            GenericInstanceMethod result = new GenericInstanceMethod(method);
            for (int i = 0; i < typeArguments.Length; i++)
            {
                result.GenericArguments.Add(typeArguments[i]);
            }

            return result;
        }

        internal static TypeReference MakeGenericType(this TypeReference type, params TypeReference[] typeArguments)
        {
            GenericInstanceType result = new GenericInstanceType(type);
            for (int i = 0; i < typeArguments.Length; i++)
            {
                result.GenericArguments.Add(typeArguments[i]);
            }

            return result;
        }

        internal static MethodReference WithGenericType(this MethodReference method, params TypeReference[] typeArguments)
        {
            GenericInstanceType type = new GenericInstanceType((method.DeclaringType as GenericInstanceType)!.ElementType);
            for (int i = 0; i < typeArguments.Length; i++)
            {
                type.GenericArguments.Add(typeArguments[i]);
            }

            MethodReference result = new MethodReference(method.Name, method.ReturnType, type)
            {
                CallingConvention = method.CallingConvention,
                ExplicitThis = method.ExplicitThis,
                HasThis = method.HasThis,
            };

            foreach (ParameterDefinition parameter in method.Parameters)
            {
                result.Parameters.Add(parameter);
            }

            return result;
        }
    }
}
