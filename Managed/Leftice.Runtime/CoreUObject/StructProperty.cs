// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class StructProperty : Property
    {
        internal StructProperty(IntPtr pointer) : base(pointer) { }

        public Struct MetaStruct => Create<Struct>(NativeMethods.GetMetaStruct(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetMetaStruct(IntPtr property);
        }
    }
}
