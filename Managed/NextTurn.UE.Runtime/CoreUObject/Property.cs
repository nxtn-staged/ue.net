// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class Property
    {
        internal readonly IntPtr pointer;

        internal Property(IntPtr pointer)
        {
            Debug.Assert(pointer != IntPtr.Zero);

            this.pointer = pointer;
        }

        public int ArrayLength => NativeMethods.GetArrayLength(this.pointer);

        public int ElementSize => NativeMethods.GetElementSize(this.pointer);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Property"/> is a parameter.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Property"/> is a parameter; otherwise, <see langword="false"/>.
        /// </value>
        /// <seealso cref="IsReturnParameter"/>
        public bool IsParameter => this.HasAnyFlags(PropertyFlags.Parameter);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Property"/> is a return parameter.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="Property"/> is a return parameter; otherwise, <see langword="false"/>.
        /// </value>
        /// <seealso cref="IsParameter"/>
        public bool IsReturnParameter => this.HasAnyFlags(PropertyFlags.ReturnParameter);

        public Name Name => NativeMethods.GetName(this.pointer);

        public int Offset => NativeMethods.GetOffset(this.pointer);

        public PropertyFlags PropertyFlags => NativeMethods.GetPropertyFlags(this.pointer);

        public int Size => this.ArrayLength * this.ElementSize;

        internal static T Create<T>(IntPtr pointer)
            where T : Property
        {
            Debug.Assert(pointer != IntPtr.Zero);
            return (Activator.CreateInstance(
                Classes.GetTypeByProperty(pointer),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new object[] { pointer },
                null) as T)!;
        }

        public bool HasAllFlags(PropertyFlags flags) => (this.PropertyFlags & flags) == flags;

        public bool HasAnyFlags(PropertyFlags flags) => (this.PropertyFlags & flags) != 0;

        public unsafe void ClearValue(Object @object, int index = 0)
        {
            if (this.HasAllFlags(PropertyFlags.TriviallyDefaultConstructible | PropertyFlags.TriviallyDestructible))
            {
                new Span<byte>(this.GetValuePtr<byte>(@object, index), this.ElementSize).Clear();
            }
            else
            {
                NativeMethods.ClearValue(this.pointer, @object.pointer, index);
            }
        }

        public void FinalizeValues(Object @object)
        {
            if (!this.HasAnyFlags(PropertyFlags.TriviallyDestructible))
            {
                NativeMethods.FinalizeValues(this.pointer, @object.pointer);
            }
        }

        internal unsafe T GetValue<T>(Object @object, int index)
            where T : unmanaged =>
            *this.GetValuePtr<T>(@object, index);

        internal unsafe T* GetValuePtr<T>(Object @object, int index)
            where T : unmanaged
        {
            if ((uint)index >= (uint)this.ArrayLength)
            {
                Throw.IndexArgumentOutOfRangeException();
            }

            return (T*)NativeMethods.GetValuePointer(this.pointer, Object.GetPointerOrThrow(@object), index);
        }

        internal unsafe ref T GetValueRef<T>(Object @object, int index)
            where T : unmanaged =>
            ref Unsafe.AsRef<T>(this.GetValuePtr<T>(@object, index));

        public unsafe void InitializeValues(Object @object)
        {
            if (this.HasAnyFlags(PropertyFlags.TriviallyDefaultConstructible))
            {
                new Span<byte>(this.GetValuePtr<byte>(@object, 0), this.Size).Clear();
            }
            else
            {
                NativeMethods.InitializeValues(this.pointer, @object.pointer);
            }
        }

        internal unsafe void SetValue<T>(Object @object, T value, int index)
            where T : unmanaged =>
            *this.GetValuePtr<T>(@object, index) = value;

        internal static class UnsafeMethods
        {
            internal static bool Equals(IntPtr property, IntPtr left, IntPtr right)
            {
                throw new NotImplementedException();
            }

            internal static void FinalizeValue(IntPtr property, IntPtr value)
            {
                throw new NotImplementedException();
            }

            internal static int GetNativeHashCode(IntPtr property, IntPtr value)
            {
                throw new NotImplementedException();
            }

            internal static PropertyFlags GetPropertyFlags(IntPtr property) => NativeMethods.GetPropertyFlags(property);

            internal static bool HasAllPropertyFlags(IntPtr property, PropertyFlags flags) =>
                (GetPropertyFlags(property) & flags) == flags;

            internal static bool HasAnyPropertyFlags(IntPtr property, PropertyFlags flags) =>
                (GetPropertyFlags(property) & flags) != 0;

            internal static void InitializeValue(IntPtr property, IntPtr value)
            {
                throw new NotImplementedException();
            }
        }

        internal static class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetClass(IntPtr property);

            [ReadOffset]
            public static extern Name GetName(IntPtr property);

            [ReadOffset]
            public static extern IntPtr GetNext(IntPtr property);

            [ReadOffset]
            public static extern int GetArrayLength(IntPtr property);

            [ReadOffset]
            public static extern int GetElementSize(IntPtr property);

            [ReadOffset]
            public static extern int GetOffset(IntPtr property);

            [ReadOffset]
            public static extern PropertyFlags GetPropertyFlags(IntPtr property);

            [Calli]
            public static extern void ClearValue(IntPtr property, IntPtr @object, int index);

            [Calli]
            public static extern void FinalizeValues(IntPtr property, IntPtr @object);

            [Calli]
            public static extern unsafe void* GetValuePointer(IntPtr property, IntPtr @object, int index);

            [Calli]
            public static extern void InitializeValues(IntPtr property, IntPtr @object);
        }
    }
}
