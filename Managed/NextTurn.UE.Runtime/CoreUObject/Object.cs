// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class Object : INativeHashable
    {
        internal IntPtr pointer;

        internal Object(IntPtr pointer)
        {
            Debug.Assert(pointer != IntPtr.Zero);

            this.pointer = pointer;
        }

        public Class Class => Create<Class>(NativeMethods.GetClass(this.pointer));

        public Name Name => NativeMethods.GetName(this.pointer);

        public bool IsPendingDestruction
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Package Package => Create<Package>(NativeMethods.GetPackage(this.pointer));

        internal Object? Parent => CreateOrNull<Object>(NativeMethods.GetParent(this.pointer));

        internal static T Create<T>(IntPtr pointer)
            where T : Object
        {
            Debug.Assert(pointer != IntPtr.Zero);
            return (Activator.CreateInstance(
                Classes.GetTypeByObject(pointer),
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                new object[] { pointer },
                null) as T)!;
        }

        public static T CreateNew<T>()
            where T : Object =>
            Create<T>(NativeMethods.Create(Class.GetClass<T>().pointer));

        internal static T? CreateOrNull<T>(IntPtr pointer)
            where T : Object =>
            pointer == IntPtr.Zero ? null : Create<T>(pointer);

        public static T? Find<T>(Object? declaringObject, string? name)
            where T : Object => default;

        public unsafe string GetPathName()
        {
            NativeMethods.GetPathName(this.pointer, out ScriptArray result);
            return StringMarshaler.ToManagedFinally(&result);
        }

        internal static IntPtr GetPointerOrThrow(Object @object) =>
            @object?.pointer ?? throw new ArgumentNullException(nameof(@object));

        internal static IntPtr GetPointerOrZero(Object? @object) =>
            @object?.pointer ?? IntPtr.Zero;

        public void AddToRoot() => throw new NotImplementedException();

        public override bool Equals(object? value) => value is Object other && this.pointer == other.pointer;

        public override int GetHashCode() => this.pointer.GetHashCode();

        int INativeHashable.GetNativeHashCode() => NativeHashCode.GetHashCode(this.pointer);

        public void RemoveFromRoot() => throw new NotImplementedException();

        internal static class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetClass(IntPtr @object);

            [ReadOffset]
            public static extern Name GetName(IntPtr @object);

            [ReadOffset]
            public static extern IntPtr GetParent(IntPtr @object);

            [Calli]
            public static extern IntPtr Create(IntPtr @class);

            [Calli]
            public static extern IntPtr GetPackage(IntPtr @object);

            [Calli]
            public static extern void GetPathName(IntPtr @object, out ScriptArray result);
        }
    }
}
