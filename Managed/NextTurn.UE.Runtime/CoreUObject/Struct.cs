// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Struct : CompoundMember
    {
        internal Struct(IntPtr pointer) : base(pointer) { }

        public static Struct GetStruct<T>()
            where T : struct =>
            Create<Struct>(Structs.GetStruct(typeof(T)));

        // internal CppStruct CppStruct => new CppStruct(NativeMethods.GetCppStruct(this.pointer));

        // private static new class NativeMethods
        // {
        //     [ReadOffset]
        //     public static extern IntPtr GetCppStruct(IntPtr @struct);
        // }
    }
}
