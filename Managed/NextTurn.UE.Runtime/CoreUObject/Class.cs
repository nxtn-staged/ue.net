// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class Class : CompoundMember
    {
        internal Class(IntPtr pointer) : base(pointer) { }

        public Class? BaseClass => CreateOrNull<Class>(CompoundMember.NativeMethods.GetBaseMember(this.pointer));

        public ClassFlags ClassFlags => NativeMethods.GetClassFlags(this.pointer);

        internal ClassTypeFlags ClassTypeFlags => NativeMethods.GetClassTypeFlags(this.pointer);

        internal unsafe IEnumerable<ImplementedInterface> Interfaces =>
            new ScriptArray.Enumerator<ImplementedInterface>(NativeMethods.GetInterfaces(this.pointer));

        public bool IsAbstract => this.HasAnyFlags(ClassFlags.Abstract);

        public bool IsInterface => this.HasAnyFlags(ClassFlags.Interface);

        public static Class GetClass<T>()
            where T : Object =>
            Create<Class>(Classes.GetClass(typeof(T)));

        public Method? FindMethod(Name name) => CreateOrNull<Method>(NativeMethods.FindMethod(this.pointer, name));

        public Object? FindObject(string name) => CreateOrNull<Object>(NativeMethods.FindObject(this.pointer, name));

        internal unsafe Array<IntPtr> FindObjects()
        {
            Array<IntPtr> result = new Array<IntPtr>();
            NativeMethods.FindObjects(this.pointer, result.array);
            return result;
        }

        internal unsafe Array<IntPtr> FindSubclasses()
        {
            Array<IntPtr> result = new Array<IntPtr>();
            NativeMethods.FindSubclasses(this.pointer, result.array);
            return result;
        }

        public bool HasAllFlags(ClassFlags flags) => (this.ClassFlags & flags) == flags;

        public bool HasAnyFlags(ClassFlags flags) => (this.ClassFlags & flags) != 0;

        internal bool HasAllTypeFlags(ClassTypeFlags flags) => (this.ClassTypeFlags & flags) == flags;

        internal bool HasAnyTypeFlags(ClassTypeFlags flags) => (this.ClassTypeFlags & flags) != 0;

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern ClassFlags GetClassFlags(IntPtr @class);

            [ReadOffset]
            public static extern ClassTypeFlags GetClassTypeFlags(IntPtr @class);

            [PointerOffset]
            public static extern unsafe ScriptArray* GetInterfaces(IntPtr @class);

            [Calli]
            public static extern IntPtr FindMethod(IntPtr @class, Name name);

            [Calli]
            public static extern IntPtr FindObject(IntPtr @class, string name);

            [Calli]
            public static extern unsafe void FindObjects(IntPtr @class, ScriptArray* result);

            [Calli]
            public static extern unsafe void FindSubclasses(IntPtr @class, ScriptArray* result);

            public static bool HasAllTypeFlags(IntPtr @class, ClassTypeFlags flags) =>
                (GetClassTypeFlags(@class) & flags) == flags;
        }
    }
}
