// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml;
using Unreal;
using Unreal.Editor;
using Enum = Unreal.Enum;
using Object = Unreal.Object;

namespace Leftice.Programs
{
    internal static class AssemblyGenerator
    {
        private const string AssemblyName = "Leftice.Project";
        private const string ModuleName = AssemblyName + ".dll";
        private const string XmlDocumentFileName = AssemblyName + ".xml";

        private const string ScriptNameMetaDataKey = "ScriptName";

        private const FieldAttributes CommonFieldAttributes = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;

        private static AssemblyBuilder assemblyBuilder = null!;
        private static ModuleBuilder moduleBuilder = null!;
        private static XmlDocument document = null!;

        private static HashSet<Class> classes = null!;
        private static Dictionary<Enum, TypeInfo> enums = null!;

        internal static unsafe void AddClass_Export(IntPtr @class, ScriptArray* sourceHeaderFileNameArray, ScriptArray* generatedHeaderFileNameArray) =>
            AddClass(new Class(@class), StringMarshaler.ToManaged(sourceHeaderFileNameArray), StringMarshaler.ToManaged(generatedHeaderFileNameArray));

        private static void AddClass(Class @class, string sourceHeaderFileName, string generatedHeaderFileName)
        {
            _ = classes.Add(@class);
        }

        private static void ExportClass(Class @class)
        {
            TypeBuilder typeBuilder = moduleBuilder.DefineType(GetName(@class), TypeAttributes.BeforeFieldInit, typeof(Object));

            FieldBuilder classFieldBuilder = typeBuilder.DefineField("class", typeof(Class), CommonFieldAttributes);

            ConstructorBuilder cctorBuilder = typeBuilder.DefineTypeInitializer();
            ILGenerator cctorGenerator = cctorBuilder.GetILGenerator();
            cctorGenerator.Emit(OpCodes.Ldsfld, GetFieldInfo(() => Package.Any));
            cctorGenerator.Emit(OpCodes.Ldstr, GetName(@class));
            cctorGenerator.Emit(OpCodes.Call, GetMethodInfo(() => Object.Find<Class>(default, default)));
            cctorGenerator.Emit(OpCodes.Stsfld, classFieldBuilder);

            ExportProperties(typeBuilder, classFieldBuilder, cctorGenerator, @class);
            ExportFunctions(typeBuilder, classFieldBuilder, cctorGenerator, @class);

            _ = typeBuilder.CreateType();
        }

        private static Type ExportEnum(Enum @enum)
        {
            IEnumerable<KeyValuePair<Name, long>> pairs = default!;

            int underscoreIndex = System.Linq.Enumerable.First(pairs).Key.ToString().IndexOf('_');
            bool hasCommonPrefix;
            string commonPrefix;
            if (underscoreIndex >= 0)
            {
                hasCommonPrefix = true;
                commonPrefix = System.Linq.Enumerable.First(pairs).Key.ToString().Substring(0, underscoreIndex + 1);
            }
            else
            {
                hasCommonPrefix = false;
                commonPrefix = null!;
            }

            EnumBuilder enumBuilder = moduleBuilder.DefineEnum(GetName(@enum), TypeAttributes.Public, typeof(long));
            foreach (KeyValuePair<Name, long> pair in pairs)
            {
                if (hasCommonPrefix)
                {
                    if (pair.Key.ToString().StartsWith(commonPrefix))
                    {
                        _ = enumBuilder.DefineLiteral(pair.Key.ToString().Substring(underscoreIndex + 1), pair.Value);
                    }
                    else
                    {
                        hasCommonPrefix = false;
                    }
                }
            }

            return enums[@enum] = enumBuilder.CreateTypeInfo()!;
        }

        private static void ExportFunctionParameters(TypeBuilder typeBuilder, FieldInfo functionField, ILGenerator cctorGenerator, Function function)
        {
            foreach (Property parameterProperty in function.ParameterProperties)
            {
                FieldBuilder parameterOffsetField = typeBuilder.DefineField(function.Name + "_" + parameterProperty.Name + "_Offset", typeof(int), CommonFieldAttributes);

                cctorGenerator.Emit(OpCodes.Ldsfld, functionField);
                cctorGenerator.Emit(OpCodes.Ldstr, parameterProperty.Name.ToString());
                cctorGenerator.Emit(OpCodes.Call, GetMethodInfo(() => default(Function)!.FindField<Property>(default(string)!)));
                cctorGenerator.Emit(OpCodes.Call, GetPropertyGetMethodInfo(() => default(Property)!.Offset));
                cctorGenerator.Emit(OpCodes.Stsfld, parameterOffsetField);
            }
        }

        private static void ExportFunctions(TypeBuilder typeBuilder, FieldInfo classField, ILGenerator cctorGenerator, Class @class)
        {
            foreach (Function function in @class.EnumerateChildren<Function>())
            {
                FieldBuilder functionFieldBuilder = typeBuilder.DefineField(function.Name + "_Function", typeof(IntPtr), CommonFieldAttributes);

                cctorGenerator.Emit(OpCodes.Ldsfld, classField);
                cctorGenerator.Emit(OpCodes.Ldstr, function.Name.ToString());
                cctorGenerator.Emit(OpCodes.Call, GetMethodInfo(() => default(Class)!.FindFunction(default)));
                cctorGenerator.Emit(OpCodes.Stsfld, functionFieldBuilder);

                MethodAttributes methodAttributes = default;
                if (function.IsStatic)
                {
                    methodAttributes |= MethodAttributes.Static;
                }

                Type returnType = null!;
                Type[] parameterTypes = new Type[function.ParameterCount];
                Property[] parameterProperties = new Property[function.ParameterCount];
                int index = 0;
                foreach (Property parameterProperty in function.ParameterProperties)
                {
                    if (parameterProperty.IsReturnParameter)
                    {
                        returnType = GetPropertyType(parameterProperty);
                        continue;
                    }

                    parameterProperties[index] = parameterProperty;

                    Type parameterType = GetPropertyType(parameterProperty);
                    if (parameterProperty.HasAnyFlags(PropertyFlags.ByRefParameter))
                    {
                        parameterType = parameterType.MakeByRefType();
                    }

                    parameterTypes[index] = parameterType;

                    index++;
                }

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(GetName(function), default, returnType, parameterTypes);
                for (int i = 0; i < function.ParameterCount; i++)
                {
                    Property parameterProperty = parameterProperties[i];

                    ParameterAttributes parameterAttributes = default;
                    if (parameterProperty.HasAnyFlags(PropertyFlags.OutParameter))
                    {
                        parameterAttributes |= ParameterAttributes.Out;
                    }

                    ParameterBuilder parameterBuilder = methodBuilder.DefineParameter(i + 1, parameterAttributes, GetName(parameterProperty));
                    if (parameterProperty.HasAnyFlags(PropertyFlags.ReadOnlyParameter))
                    {
                        parameterBuilder.SetCustomAttribute(new CustomAttributeBuilder(GetConstructorInfo(() => new IsReadOnlyAttribute()), Array.Empty<object>()));
                    }
                }
            }
        }

        private static void ExportProperties(TypeBuilder typeBuilder, FieldInfo classField, ILGenerator cctorGenerator, Class @class)
        {
            foreach (Property property in @class.EnumerateChildren<Property>())
            {
                FieldBuilder propertyFieldBuilder = typeBuilder.DefineField(GetName(property) + "_Property", typeof(Property), CommonFieldAttributes);

                cctorGenerator.Emit(OpCodes.Ldsfld, classField);
                cctorGenerator.Emit(OpCodes.Ldstr, GetName(property));
                cctorGenerator.Emit(OpCodes.Call, GetMethodInfo(() => default(Struct)!.FindField<Property>(default(string)!)));
                cctorGenerator.Emit(OpCodes.Stsfld, propertyFieldBuilder);

                Type propertyType = GetPropertyType(property);

                if (property.ArrayLength == 1)
                {
                    MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(
                      "get_" + GetName(property), MethodAttributes.SpecialName, propertyType, null);

                    ILGenerator getMethodGenerator = getMethodBuilder.GetILGenerator();
                    getMethodGenerator.Emit(OpCodes.Ldsfld, propertyFieldBuilder);
                    getMethodGenerator.Emit(OpCodes.Ldarg_0);
                    getMethodGenerator.Emit(OpCodes.Ldc_I4_0);
                    getMethodGenerator.Emit(OpCodes.Call, GetMethodInfo(
                        () => default(Property)!.GetValue<IntPtr>(default!, default)));
                    getMethodGenerator.Emit(OpCodes.Ret);

                    MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(
                        "set_" + GetName(property), MethodAttributes.SpecialName, typeof(void), new[] { propertyType });

                    ILGenerator setMethodGenerator = setMethodBuilder.GetILGenerator();
                    setMethodGenerator.Emit(OpCodes.Ldsfld, propertyFieldBuilder);
                    setMethodGenerator.Emit(OpCodes.Ldarg_0);
                    setMethodGenerator.Emit(OpCodes.Ldarg_1);
                    setMethodGenerator.Emit(OpCodes.Ldc_I4_0);
                    setMethodGenerator.Emit(OpCodes.Call, GetMethodInfo(
                        () => default(Property)!.SetValue<IntPtr>(default!, default, default)));

                    PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                        GetName(property), default, propertyType, null);

                    propertyBuilder.SetGetMethod(getMethodBuilder);
                    propertyBuilder.SetSetMethod(setMethodBuilder);
                }
                else
                {
                    propertyType = typeof(FixedSizeArray<>).MakeGenericType(propertyType);

                    MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(
                        "get_" + GetName(property), MethodAttributes.SpecialName, propertyType, null);

                    ILGenerator getMethodGenerator = getMethodBuilder.GetILGenerator();
                    getMethodGenerator.Emit(OpCodes.Ldsfld, propertyFieldBuilder);
                    getMethodGenerator.Emit(OpCodes.Ldarg_0);
                    getMethodGenerator.Emit(OpCodes.Ldc_I4_0);
                    unsafe
                    {
                        getMethodGenerator.Emit(OpCodes.Call, GetMethodInfo(
                            () => default(Property)!.GetValuePtr<IntPtr>(default!, default)));
                    }

                    getMethodGenerator.Emit(OpCodes.Ldc_I4, property.ArrayLength);
                    getMethodGenerator.Emit(OpCodes.Newobj, propertyType.GetConstructor(new[] { typeof(IntPtr), typeof(int) })!);
                    getMethodGenerator.Emit(OpCodes.Ret);

                    PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                        GetName(property), default, propertyType, null);
                }
            }
        }

        internal static void FinishExport()
        {
            foreach (Class @class in classes)
            {
                ExportClass(@class);
            }

            document.Save(XmlDocumentFileName);
        }

        private static ConstructorInfo GetConstructorInfo<T>(Expression<Func<T>> expression) =>
            ((expression.Body as NewExpression)!).Constructor;

        private static FieldInfo GetFieldInfo<T>(Expression<Func<T>> expression) =>
            (((expression.Body as MemberExpression)!).Member as FieldInfo)!;

        private static MethodInfo GetMethodInfo(Expression<Action> expression) =>
            ((expression.Body as MethodCallExpression)!).Method;

        private static MethodInfo GetMethodInfo<T>(Expression<Func<T>> expression) =>
            ((expression.Body as MethodCallExpression)!).Method;

        private static MethodInfo GetPropertyGetMethodInfo<T>(Expression<Func<T>> expression) =>
            ((((expression.Body as MemberExpression)!).Member as PropertyInfo)!).GetMethod!;

        private static Type GetPropertyMarshaler(Property property) => property switch
        {
            NumericProperty _ => typeof(TrivialMarshaler<>).MakeGenericType(GetPropertyType(property)),

            EnumProperty enumProperty => ExportEnum(enumProperty.Enum),

            BooleanProperty _ => typeof(BooleanMarshaler),

            NameProperty _ => typeof(TrivialMarshaler<Name>),
            StringProperty _ => typeof(StringMarshaler),
            TextProperty _ => typeof(TextMarshaler),

            StructProperty _ => default!,

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => typeof(Object),
                WeakObjectProperty _ => typeof(WeakObjectReference),
                LazyObjectProperty _ => typeof(LazyObjectReference),
                SoftObjectProperty _ => typeof(SoftObjectReference),
                _ => throw new NotImplementedException()
            },

            ArrayProperty arrayProperty => typeof(Array<>)
            .MakeGenericType(GetPropertyType(arrayProperty.ElementProperty)),

            MapProperty mapProperty => typeof(Map<,>)
            .MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => typeof(Set<>)
            .MakeGenericType(GetPropertyType(setProperty.ElementProperty)),

            _ => throw new NotImplementedException(),
        };

        private static Type GetPropertyType(Property property) => property switch
        {
            NumericProperty numericProperty => numericProperty switch
            {
                SByteProperty _ => typeof(sbyte),
                ByteProperty _ => typeof(byte),
                Int16Property _ => typeof(short),
                UInt16Property _ => typeof(ushort),
                Int32Property _ => typeof(int),
                UInt32Property _ => typeof(uint),
                Int64Property _ => typeof(long),
                UInt64Property _ => typeof(ulong),
                SingleProperty _ => typeof(float),
                DoubleProperty _ => typeof(double),
                _ => throw new NotImplementedException()
            },

            EnumProperty enumProperty => ExportEnum(enumProperty.Enum),

            BooleanProperty _ => typeof(NativeBoolean),

            NameProperty _ => typeof(Name),
            StringProperty _ => typeof(string),
            TextProperty _ => typeof(Text),

            StructProperty _ => default!,

            ObjectPropertyBase objectPropertyBase => objectPropertyBase switch
            {
                ObjectProperty _ => typeof(Object),
                WeakObjectProperty _ => typeof(WeakObjectReference),
                LazyObjectProperty _ => typeof(LazyObjectReference),
                SoftObjectProperty _ => typeof(SoftObjectReference),
                _ => throw new NotImplementedException()
            },

            ArrayProperty arrayProperty => typeof(Array<>)
            .MakeGenericType(GetPropertyType(arrayProperty.ElementProperty)),

            MapProperty mapProperty => typeof(Map<,>)
            .MakeGenericType(GetPropertyType(mapProperty.KeyProperty), GetPropertyType(mapProperty.ValueProperty)),

            SetProperty setProperty => typeof(Set<>)
            .MakeGenericType(GetPropertyType(setProperty.ElementProperty)),

            _ => throw new NotImplementedException(),
        };

        private static string GetName(Field field) =>
            field.TryGetMetaData(ScriptNameMetaDataKey, out string? name) ? name : field.Name.ToString();

        internal static void Initialize()
        {
            assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.Run);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
            document = new XmlDocument();
            classes = new HashSet<Class>();
            enums = new Dictionary<Enum, TypeInfo>();
        }

        internal delegate void Initialize_Delegate();

        internal delegate void AddClass_Delegate(IntPtr @class, ref ScriptArray sourceHeaderFileNameArray, ref ScriptArray generatedHeaderFileNameArray);

        internal delegate void FinishExport_Delegate();
    }
}
