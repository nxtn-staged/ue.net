// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections.Generic;
using Unreal;

namespace Leftice
{
    internal static class Classes
    {
        private static readonly Dictionary<IntPtr, Type> classes = new Dictionary<IntPtr, Type>
        {
            { NativeMethods.Member_Class, typeof(Member) },
            { NativeMethods.SByteProperty_Class, typeof(SByteProperty) },
            { NativeMethods.Enum_Class, typeof(Unreal.Enum) },
            { NativeMethods.CompoundMember_Class, typeof(CompoundMember) },
            { NativeMethods.Struct_Class, typeof(Struct) },
            { NativeMethods.Class_Class, typeof(Class) },
            { NativeMethods.ByteProperty_Class, typeof(ByteProperty) },
            { NativeMethods.Int32Property_Class, typeof(Int32Property) },
            { NativeMethods.SingleProperty_Class, typeof(SingleProperty) },
            { NativeMethods.UInt64Property_Class, typeof(UInt64Property) },
            { NativeMethods.ClassProperty_Class, typeof(ClassProperty) },
            { NativeMethods.UInt32Property_Class, typeof(UInt32Property) },
            { NativeMethods.InterfaceProperty_Class, typeof(InterfaceProperty) },
            { NativeMethods.NameProperty_Class, typeof(NameProperty) },
            { NativeMethods.StringProperty_Class, typeof(StringProperty) },
            { NativeMethods.Property_Class, typeof(Property) },
            { NativeMethods.ObjectProperty_Class, typeof(ObjectProperty) },
            { NativeMethods.BooleanProperty_Class, typeof(BooleanProperty) },
            { NativeMethods.UInt16Property_Class, typeof(UInt16Property) },
            { NativeMethods.Function_Class, typeof(Method) },
            { NativeMethods.StructProperty_Class, typeof(StructProperty) },
            { NativeMethods.ArrayProperty_Class, typeof(ArrayProperty) },
            { NativeMethods.Int64Property_Class, typeof(Int64Property) },
            { NativeMethods.DelegateProperty_Class, typeof(DelegateProperty) },
            { NativeMethods.NumericProperty_Class, typeof(NumericProperty) },
            { NativeMethods.MulticastDelegateProperty_Class, typeof(MulticastDelegateProperty) },
            { NativeMethods.ObjectPropertyBase_Class, typeof(ObjectPropertyBase) },
            { NativeMethods.WeakObjectProperty_Class, typeof(WeakObjectProperty) },
            { NativeMethods.LazyObjectProperty_Class, typeof(LazyObjectProperty) },
            { NativeMethods.SoftObjectProperty_Class, typeof(SoftObjectProperty) },
            { NativeMethods.TextProperty_Class, typeof(TextProperty) },
            { NativeMethods.Int16Property_Class, typeof(Int16Property) },
            { NativeMethods.DoubleProperty_Class, typeof(DoubleProperty) },
            { NativeMethods.SoftClassProperty_Class, typeof(SoftClassProperty) },
            { NativeMethods.Package_Class, typeof(Package) },
            { NativeMethods.DelegateMethod_Class, typeof(DelegateMethod) },
            { NativeMethods.MapProperty_Class, typeof(MapProperty) },
            { NativeMethods.SetProperty_Class, typeof(SetProperty) },
            { NativeMethods.EnumProperty_Class, typeof(EnumProperty) },
            { NativeMethods.SparseDelegateMethod_Class, typeof(SparseDelegateMethod) },
            { NativeMethods.MulticastInlineDelegateProperty_Class, typeof(MulticastInlineDelegateProperty) },
            { NativeMethods.MulticastSparseDelegateProperty_Class, typeof(MulticastSparseDelegateProperty) },
        };

        internal static IntPtr GetClass(Type type)
        {
            foreach (KeyValuePair<IntPtr, Type> pair in classes)
            {
                if (pair.Value == type)
                {
                    return pair.Key;
                }
            }

            throw new NotSupportedException();
        }

        internal static Type GetClassType(IntPtr @object) => GetClassTypeByClass(Unreal.Object.NativeMethods.GetClass(@object));

        private static Type GetClassTypeByClass(IntPtr @class) =>
            classes.TryGetValue(@class, out Type? type) ? type :
                (classes[@class] = GetClassTypeByClass(CompoundMember.NativeMethods.GetInheritanceBaseMember(@class)));

        private static class NativeMethods
        {
            public static readonly IntPtr Member_Class;
            public static readonly IntPtr SByteProperty_Class;
            public static readonly IntPtr Enum_Class;
            public static readonly IntPtr CompoundMember_Class;
            public static readonly IntPtr Struct_Class;
            public static readonly IntPtr Class_Class;
            public static readonly IntPtr ByteProperty_Class;
            public static readonly IntPtr Int32Property_Class;
            public static readonly IntPtr SingleProperty_Class;
            public static readonly IntPtr UInt64Property_Class;
            public static readonly IntPtr ClassProperty_Class;
            public static readonly IntPtr UInt32Property_Class;
            public static readonly IntPtr InterfaceProperty_Class;
            public static readonly IntPtr NameProperty_Class;
            public static readonly IntPtr StringProperty_Class;
            public static readonly IntPtr Property_Class;
            public static readonly IntPtr ObjectProperty_Class;
            public static readonly IntPtr BooleanProperty_Class;
            public static readonly IntPtr UInt16Property_Class;
            public static readonly IntPtr Function_Class;
            public static readonly IntPtr StructProperty_Class;
            public static readonly IntPtr ArrayProperty_Class;
            public static readonly IntPtr Int64Property_Class;
            public static readonly IntPtr DelegateProperty_Class;
            public static readonly IntPtr NumericProperty_Class;
            public static readonly IntPtr MulticastDelegateProperty_Class;
            public static readonly IntPtr ObjectPropertyBase_Class;
            public static readonly IntPtr WeakObjectProperty_Class;
            public static readonly IntPtr LazyObjectProperty_Class;
            public static readonly IntPtr SoftObjectProperty_Class;
            public static readonly IntPtr TextProperty_Class;
            public static readonly IntPtr Int16Property_Class;
            public static readonly IntPtr DoubleProperty_Class;
            public static readonly IntPtr SoftClassProperty_Class;
            public static readonly IntPtr Package_Class;
            public static readonly IntPtr DelegateMethod_Class;
            public static readonly IntPtr MapProperty_Class;
            public static readonly IntPtr SetProperty_Class;
            public static readonly IntPtr EnumProperty_Class;
            public static readonly IntPtr SparseDelegateMethod_Class;
            public static readonly IntPtr MulticastInlineDelegateProperty_Class;
            public static readonly IntPtr MulticastSparseDelegateProperty_Class;
        }
    }
}
