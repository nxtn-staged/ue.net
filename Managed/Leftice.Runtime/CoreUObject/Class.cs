// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class Class : CompoundMember
    {
        internal Class(IntPtr pointer) : base(pointer) { }

        public Class? BaseClass => CreateOrNull<Class>(CompoundMember.NativeMethods.GetBaseMember(this.pointer));

        public ClassFlags ClassFlags => NativeMethods.GetClassFlags(this.pointer);

        internal ClassTypeFlags ClassTypeFlags => NativeMethods.GetClassTypeFlags(this.pointer);

        internal ScriptArray Interfaces => NativeMethods.GetInterfaces(this.pointer);

        public static Class GetClass<T>()
            where T : Object =>
            Create<Class>(Classes.GetClass(typeof(T)));

        public Method? FindMethod(Name name) => throw new NotImplementedException();

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

            [ReadOffset]
            public static extern ScriptArray GetInterfaces(IntPtr @class);

            public static bool HasAllTypeFlags(IntPtr @class, ClassTypeFlags flags) =>
                (GetClassTypeFlags(@class) & flags) == flags;
        }
    }
}
