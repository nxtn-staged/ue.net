// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class Struct : CompoundMember
    {
        internal Struct(IntPtr pointer) : base(pointer) { }

        internal CppStructOperations CppStructOperations => new CppStructOperations(NativeMethods.GetCppStructOperations(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetCppStructOperations(IntPtr @struct);
        }
    }
}
