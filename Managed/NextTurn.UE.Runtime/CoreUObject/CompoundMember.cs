// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class CompoundMember : Member
    {
        internal CompoundMember(IntPtr pointer) : base(pointer) { }

        public CompoundMember? BaseMember =>
            CreateOrNull<CompoundMember>(NativeMethods.GetBaseMember(this.pointer));

        public string CppPrefix => CharArrayMarshaler.ToManaged(NativeMethods.GetCppPrefix(this.pointer))!;

        public Member? FirstMember =>
            CreateOrNull<Member>(NativeMethods.GetFirstMember(this.pointer));

        public CompoundMember? InheritanceBaseMember =>
            CreateOrNull<CompoundMember>(NativeMethods.GetInheritanceBaseMember(this.pointer));

        public IEnumerable<T> EnumerateMembers<T>()
            where T : Member =>
            new MemberEnumerator<T>(this.pointer);

        public IEnumerable<T> EnumerateProperties<T>()
            where T : Property =>
            new PropertyEnumerator<T>(this.pointer);

        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        public T? FindMember<T>(string name)
            where T : Member =>
            name is null ? throw new ArgumentNullException(nameof(name)) :
            this.FindMember<T>(new Name(name, Name.Mode.Find));

        public T? FindMember<T>(Name name)
            where T : Member =>
            name.IsNone ? null : System.Linq.Enumerable.FirstOrDefault(this.EnumerateMembers<T>(), field => field.Name == name);

        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
        public T? FindProperty<T>(string name)
            where T : Property =>
            name is null ? throw new ArgumentNullException(nameof(name)) :
            this.FindProperty<T>(new Name(name, Name.Mode.Find));

        public T? FindProperty<T>(Name name)
            where T : Property =>
            name.IsNone ? null : System.Linq.Enumerable.FirstOrDefault(this.EnumerateProperties<T>(), property => property.Name == name);

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetBaseMember(IntPtr @struct);

            [ReadOffset]
            public static extern IntPtr GetFirstMember(IntPtr @struct);

            [ReadOffset]
            public static extern IntPtr GetFirstProperty(IntPtr @struct);

            [Calli]
            public static extern IntPtr GetCppPrefix(IntPtr member);

            [Calli]
            public static extern IntPtr GetInheritanceBaseMember(IntPtr @struct);
        }

        private sealed class MemberEnumerator<T> : IEnumerable<T>, IEnumerator<T>
            where T : Member
        {
            private static readonly Dictionary<Type, ClassTypeFlags> TypeToTypeFlags = new Dictionary<Type, ClassTypeFlags>
            {
                { typeof(Member), ClassTypeFlags.Member },
                { typeof(SByteProperty), ClassTypeFlags.SByteProperty },
                { typeof(Enum), ClassTypeFlags.Enum },
                { typeof(CompoundMember), ClassTypeFlags.CompoundMember },
                { typeof(Struct), ClassTypeFlags.Struct },
                { typeof(Class), ClassTypeFlags.Class },
                { typeof(ByteProperty), ClassTypeFlags.ByteProperty },
                { typeof(Int32Property), ClassTypeFlags.Int32Property },
                { typeof(SingleProperty), ClassTypeFlags.SingleProperty },
                { typeof(UInt64Property), ClassTypeFlags.UInt64Property },
                { typeof(ClassProperty), ClassTypeFlags.ClassProperty },
                { typeof(UInt32Property), ClassTypeFlags.UInt32Property },
                { typeof(InterfaceProperty), ClassTypeFlags.InterfaceProperty },
                { typeof(NameProperty), ClassTypeFlags.NameProperty },
                { typeof(StringProperty), ClassTypeFlags.StringProperty },
                { typeof(Property), ClassTypeFlags.Property },
                { typeof(ObjectProperty), ClassTypeFlags.ObjectProperty },
                { typeof(BooleanProperty), ClassTypeFlags.BooleanProperty },
                { typeof(UInt16Property), ClassTypeFlags.UInt16Property },
                { typeof(Method), ClassTypeFlags.Method },
                { typeof(StructProperty), ClassTypeFlags.StructProperty },
                { typeof(ArrayProperty), ClassTypeFlags.ArrayProperty },
                { typeof(Int64Property), ClassTypeFlags.Int64Property },
                { typeof(DelegateProperty), ClassTypeFlags.DelegateProperty },
                { typeof(NumericProperty), ClassTypeFlags.NumericProperty },
                { typeof(MulticastDelegateProperty), ClassTypeFlags.MulticastDelegateProperty },
                { typeof(ObjectPropertyBase), ClassTypeFlags.ObjectPropertyBase },
                { typeof(WeakObjectProperty), ClassTypeFlags.WeakObjectProperty },
                { typeof(LazyObjectProperty), ClassTypeFlags.LazyObjectProperty },
                { typeof(SoftObjectProperty), ClassTypeFlags.SoftObjectProperty },
                { typeof(TextProperty), ClassTypeFlags.TextProperty },
                { typeof(Int16Property), ClassTypeFlags.Int16Property },
                { typeof(DoubleProperty), ClassTypeFlags.DoubleProperty },
                { typeof(SoftClassProperty), ClassTypeFlags.SoftClassProperty },
                { typeof(Package), ClassTypeFlags.Package },
                { typeof(DelegateMethod), ClassTypeFlags.DelegateMethod },
                { typeof(MapProperty), ClassTypeFlags.MapProperty },
                { typeof(SetProperty), ClassTypeFlags.SetProperty },
                { typeof(EnumProperty), ClassTypeFlags.EnumProperty },
                { typeof(SparseDelegateMethod), ClassTypeFlags.SparseDelegateMethod },
                { typeof(MulticastInlineDelegateProperty), ClassTypeFlags.MulticastInlineDelegateProperty },
                { typeof(MulticastSparseDelegateProperty), ClassTypeFlags.MulticastSparseDelegateProperty },
                { typeof(PropertyPathProperty), ClassTypeFlags.FieldPathProperty },
            };

            private readonly ClassTypeFlags classTypeFlags;
            private IntPtr compoundMember;
            private IntPtr member;
            private int interfaceIndex;
            private readonly bool inherit;
            private readonly bool includeDeprecatedProperties;
            private readonly bool includeClassInterfaces;

            internal MemberEnumerator(IntPtr compoundMember)
            {
                if (!TypeToTypeFlags.TryGetValue(typeof(T), out this.classTypeFlags))
                {
                    throw new NotImplementedException();
                }

                this.compoundMember = compoundMember;
                this.member = IntPtr.Zero;
                this.interfaceIndex = -1;
                this.inherit = true;
                this.includeDeprecatedProperties = true;
                this.includeClassInterfaces = false;
            }

            public T Current => Create<T>(this.member);

            object IEnumerator.Current => this.Current;

            public void Dispose() { }

            public IEnumerator<T> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this;

            public unsafe bool MoveNext()
            {
                if (this.member == IntPtr.Zero)
                {
                    if (this.compoundMember == IntPtr.Zero)
                    {
                        return false;
                    }

                    this.member = NativeMethods.GetFirstMember(this.compoundMember);
                }
                else
                {
                    this.member = Member.NativeMethods.GetNext(this.member);
                }

                while (this.compoundMember != IntPtr.Zero)
                {
                    while (this.member != IntPtr.Zero)
                    {
                        IntPtr memberClass = Object.NativeMethods.GetClass(this.member);

                        if (Class.NativeMethods.HasAllTypeFlags(memberClass, this.classTypeFlags) &&
                            (this.includeDeprecatedProperties ||
                            !Class.NativeMethods.HasAllTypeFlags(memberClass, ClassTypeFlags.Property) ||
                            !Property.UnsafeMethods.HasAllPropertyFlags(this.member, PropertyFlags.Deprecated)))
                        {
                            return true;
                        }

                        this.member = Member.NativeMethods.GetNext(this.member);
                    }

                    if (this.includeClassInterfaces)
                    {
                        this.interfaceIndex++;
                        if (this.interfaceIndex < Class.NativeMethods.GetInterfaces(this.compoundMember)->Count)
                        {
                            ImplementedInterface* implementedInterface = Class.NativeMethods.GetInterfaces(this.compoundMember)->GetItemPtr<ImplementedInterface>(this.interfaceIndex);
                            this.member = implementedInterface->@class != IntPtr.Zero ? NativeMethods.GetFirstMember(implementedInterface->@class) : IntPtr.Zero;
                            continue;
                        }
                    }

                    if (this.inherit)
                    {
                        this.compoundMember = NativeMethods.GetInheritanceBaseMember(this.compoundMember);
                        if (this.compoundMember != IntPtr.Zero)
                        {
                            this.member = NativeMethods.GetFirstMember(this.compoundMember);
                            this.interfaceIndex = -1;
                            continue;
                        }
                    }

                    break;
                }

                this.compoundMember = IntPtr.Zero;
                this.member = IntPtr.Zero;
                return false;
            }

            /// <summary>
            /// This method is not supported.
            /// </summary>
            /// <exception cref="NotSupportedException">
            /// Always thrown.
            /// </exception>
            public void Reset() => throw new NotSupportedException();
        }

        private sealed class PropertyEnumerator<T> : IEnumerable<T>, IEnumerator<T>
            where T : Property
        {
            private static readonly Dictionary<Type, ClassTypeFlags> TypeToTypeFlags = new Dictionary<Type, ClassTypeFlags>
            {
                { typeof(Member), ClassTypeFlags.Member },
                { typeof(SByteProperty), ClassTypeFlags.SByteProperty },
                { typeof(Enum), ClassTypeFlags.Enum },
                { typeof(CompoundMember), ClassTypeFlags.CompoundMember },
                { typeof(Struct), ClassTypeFlags.Struct },
                { typeof(Class), ClassTypeFlags.Class },
                { typeof(ByteProperty), ClassTypeFlags.ByteProperty },
                { typeof(Int32Property), ClassTypeFlags.Int32Property },
                { typeof(SingleProperty), ClassTypeFlags.SingleProperty },
                { typeof(UInt64Property), ClassTypeFlags.UInt64Property },
                { typeof(ClassProperty), ClassTypeFlags.ClassProperty },
                { typeof(UInt32Property), ClassTypeFlags.UInt32Property },
                { typeof(InterfaceProperty), ClassTypeFlags.InterfaceProperty },
                { typeof(NameProperty), ClassTypeFlags.NameProperty },
                { typeof(StringProperty), ClassTypeFlags.StringProperty },
                { typeof(Property), ClassTypeFlags.Property },
                { typeof(ObjectProperty), ClassTypeFlags.ObjectProperty },
                { typeof(BooleanProperty), ClassTypeFlags.BooleanProperty },
                { typeof(UInt16Property), ClassTypeFlags.UInt16Property },
                { typeof(Method), ClassTypeFlags.Method },
                { typeof(StructProperty), ClassTypeFlags.StructProperty },
                { typeof(ArrayProperty), ClassTypeFlags.ArrayProperty },
                { typeof(Int64Property), ClassTypeFlags.Int64Property },
                { typeof(DelegateProperty), ClassTypeFlags.DelegateProperty },
                { typeof(NumericProperty), ClassTypeFlags.NumericProperty },
                { typeof(MulticastDelegateProperty), ClassTypeFlags.MulticastDelegateProperty },
                { typeof(ObjectPropertyBase), ClassTypeFlags.ObjectPropertyBase },
                { typeof(WeakObjectProperty), ClassTypeFlags.WeakObjectProperty },
                { typeof(LazyObjectProperty), ClassTypeFlags.LazyObjectProperty },
                { typeof(SoftObjectProperty), ClassTypeFlags.SoftObjectProperty },
                { typeof(TextProperty), ClassTypeFlags.TextProperty },
                { typeof(Int16Property), ClassTypeFlags.Int16Property },
                { typeof(DoubleProperty), ClassTypeFlags.DoubleProperty },
                { typeof(SoftClassProperty), ClassTypeFlags.SoftClassProperty },
                { typeof(Package), ClassTypeFlags.Package },
                { typeof(DelegateMethod), ClassTypeFlags.DelegateMethod },
                { typeof(MapProperty), ClassTypeFlags.MapProperty },
                { typeof(SetProperty), ClassTypeFlags.SetProperty },
                { typeof(EnumProperty), ClassTypeFlags.EnumProperty },
                { typeof(SparseDelegateMethod), ClassTypeFlags.SparseDelegateMethod },
                { typeof(MulticastInlineDelegateProperty), ClassTypeFlags.MulticastInlineDelegateProperty },
                { typeof(MulticastSparseDelegateProperty), ClassTypeFlags.MulticastSparseDelegateProperty },
                { typeof(PropertyPathProperty), ClassTypeFlags.FieldPathProperty },
            };

            private readonly ClassTypeFlags classTypeFlags;
            private IntPtr compoundMember;
            private IntPtr property;
            private int interfaceIndex;
            private readonly bool inherit;
            private readonly bool includeDeprecatedProperties;
            private readonly bool includeClassInterfaces;

            internal PropertyEnumerator(IntPtr compoundMember)
            {
                if (!TypeToTypeFlags.TryGetValue(typeof(T), out this.classTypeFlags))
                {
                    throw new NotImplementedException();
                }

                this.compoundMember = compoundMember;
                this.property = IntPtr.Zero;
                this.interfaceIndex = -1;
                this.inherit = true;
                this.includeDeprecatedProperties = true;
                this.includeClassInterfaces = false;
            }

            public T Current => Property.Create<T>(this.property);

            object IEnumerator.Current => this.Current;

            public void Dispose() { }

            public IEnumerator<T> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => this;

            public unsafe bool MoveNext()
            {
                if (this.property == IntPtr.Zero)
                {
                    if (this.compoundMember == IntPtr.Zero)
                    {
                        return false;
                    }

                    this.property = NativeMethods.GetFirstProperty(this.compoundMember);
                }
                else
                {
                    this.property = Property.NativeMethods.GetNext(this.property);
                }

                while (this.compoundMember != IntPtr.Zero)
                {
                    while (this.property != IntPtr.Zero)
                    {
                        IntPtr propertyClass = Property.NativeMethods.GetClass(this.property);

                        if (PropertyClass.NativeMethods.HasAllTypeFlags(propertyClass, this.classTypeFlags) &&
                            (this.includeDeprecatedProperties ||
                            !PropertyClass.NativeMethods.HasAllTypeFlags(propertyClass, ClassTypeFlags.Property) ||
                            !Property.UnsafeMethods.HasAllPropertyFlags(this.property, PropertyFlags.Deprecated)))
                        {
                            return true;
                        }

                        this.property = Property.NativeMethods.GetNext(this.property);
                    }

                    if (this.includeClassInterfaces)
                    {
                        this.interfaceIndex++;
                        if (this.interfaceIndex < Class.NativeMethods.GetInterfaces(this.compoundMember)->Count)
                        {
                            ImplementedInterface* implementedInterface = Class.NativeMethods.GetInterfaces(this.compoundMember)->GetItemPtr<ImplementedInterface>(this.interfaceIndex);
                            this.property = implementedInterface->@class != IntPtr.Zero ? NativeMethods.GetFirstProperty(implementedInterface->@class) : IntPtr.Zero;
                            continue;
                        }
                    }

                    if (this.inherit)
                    {
                        this.compoundMember = NativeMethods.GetInheritanceBaseMember(this.compoundMember);
                        if (this.compoundMember != IntPtr.Zero)
                        {
                            this.property = NativeMethods.GetFirstProperty(this.compoundMember);
                            this.interfaceIndex = -1;
                            continue;
                        }
                    }

                    break;
                }

                this.compoundMember = IntPtr.Zero;
                this.property = IntPtr.Zero;
                return false;
            }

            /// <summary>
            /// This method is not supported.
            /// </summary>
            /// <exception cref="NotSupportedException">
            /// Always thrown.
            /// </exception>
            public void Reset() => throw new NotSupportedException();
        }
    }
}
