// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;

namespace Unreal
{
    internal static class Classes
    {
        private static readonly Dictionary<IntPtr, Type> TypeByPtr = new Dictionary<IntPtr, Type>();
        private static readonly Dictionary<Type, IntPtr> PtrByType = new Dictionary<Type, IntPtr>();

        static Classes()
        {
            Register(NativeMethods.Member_Class, typeof(Member));
            Register(NativeMethods.SByteProperty_Class, typeof(SByteProperty));
            Register(NativeMethods.Enum_Class, typeof(Enum));
            Register(NativeMethods.CompoundMember_Class, typeof(CompoundMember));
            Register(NativeMethods.Struct_Class, typeof(Struct));
            Register(NativeMethods.Class_Class, typeof(Class));
            Register(NativeMethods.ByteProperty_Class, typeof(ByteProperty));
            Register(NativeMethods.Int32Property_Class, typeof(Int32Property));
            Register(NativeMethods.SingleProperty_Class, typeof(SingleProperty));
            Register(NativeMethods.UInt64Property_Class, typeof(UInt64Property));
            Register(NativeMethods.ClassProperty_Class, typeof(ClassProperty));
            Register(NativeMethods.UInt32Property_Class, typeof(UInt32Property));
            Register(NativeMethods.InterfaceProperty_Class, typeof(InterfaceProperty));
            Register(NativeMethods.NameProperty_Class, typeof(NameProperty));
            Register(NativeMethods.StringProperty_Class, typeof(StringProperty));
            Register(NativeMethods.Property_Class, typeof(Property));
            Register(NativeMethods.ObjectProperty_Class, typeof(ObjectProperty));
            Register(NativeMethods.BooleanProperty_Class, typeof(BooleanProperty));
            Register(NativeMethods.UInt16Property_Class, typeof(UInt16Property));
            Register(NativeMethods.Method_Class, typeof(Method));
            Register(NativeMethods.StructProperty_Class, typeof(StructProperty));
            Register(NativeMethods.ArrayProperty_Class, typeof(ArrayProperty));
            Register(NativeMethods.Int64Property_Class, typeof(Int64Property));
            Register(NativeMethods.DelegateProperty_Class, typeof(DelegateProperty));
            Register(NativeMethods.NumericProperty_Class, typeof(NumericProperty));
            Register(NativeMethods.MulticastDelegateProperty_Class, typeof(MulticastDelegateProperty));
            Register(NativeMethods.ObjectPropertyBase_Class, typeof(ObjectPropertyBase));
            Register(NativeMethods.WeakObjectProperty_Class, typeof(WeakObjectProperty));
            Register(NativeMethods.LazyObjectProperty_Class, typeof(LazyObjectProperty));
            Register(NativeMethods.SoftObjectProperty_Class, typeof(SoftObjectProperty));
            Register(NativeMethods.TextProperty_Class, typeof(TextProperty));
            Register(NativeMethods.Int16Property_Class, typeof(Int16Property));
            Register(NativeMethods.DoubleProperty_Class, typeof(DoubleProperty));
            Register(NativeMethods.SoftClassProperty_Class, typeof(SoftClassProperty));
            Register(NativeMethods.Package_Class, typeof(Package));
            Register(NativeMethods.DelegateMethod_Class, typeof(DelegateMethod));
            Register(NativeMethods.MapProperty_Class, typeof(MapProperty));
            Register(NativeMethods.SetProperty_Class, typeof(SetProperty));
            Register(NativeMethods.EnumProperty_Class, typeof(EnumProperty));
            Register(NativeMethods.SparseDelegateMethod_Class, typeof(SparseDelegateMethod));
            Register(NativeMethods.MulticastInlineDelegateProperty_Class, typeof(MulticastInlineDelegateProperty));
            Register(NativeMethods.MulticastSparseDelegateProperty_Class, typeof(MulticastSparseDelegateProperty));
            Register(NativeMethods.PropertyPathProperty_Class, typeof(PropertyPathProperty));
            Register(NativeMethods.Interface_Class, typeof(Interface));
        }

        internal static IntPtr GetClass(Type type)
        {
            if (PtrByType.TryGetValue(type, out IntPtr @class))
            {
                return @class;
            }

            Throw.NotSupportedException();
            return default;
        }

        internal static Type GetTypeByObject(IntPtr @object)
        {
            static Type GetTypeByClass(IntPtr @class) =>
                TypeByPtr.TryGetValue(@class, out Type? type) ? type :
                    (TypeByPtr[@class] = GetTypeByClass(CompoundMember.NativeMethods.GetBaseMember(@class)));

            return GetTypeByClass(Object.NativeMethods.GetClass(@object));
        }

        internal static Type GetTypeByProperty(IntPtr property)
        {
            static Type GetTypeByPropertyClass(IntPtr @class) =>
                TypeByPtr.TryGetValue(@class, out Type? type) ? type :
                    (TypeByPtr[@class] = GetTypeByPropertyClass(PropertyClass.NativeMethods.GetBaseClass(@class)));

            return GetTypeByPropertyClass(Property.NativeMethods.GetClass(property));
        }

        internal static void Register(IntPtr @class, Type type)
        {
            _ = TypeByPtr.TryAdd(@class, type);
            _ = PtrByType.TryAdd(type, @class);
        }

        internal static void Unregister(IntPtr @class, Type type)
        {
            _ = TypeByPtr.Remove(@class);
            _ = PtrByType.Remove(type);
        }

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
            public static readonly IntPtr Method_Class;
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
            public static readonly IntPtr PropertyPathProperty_Class;
            public static readonly IntPtr Interface_Class;
        }
    }
}
