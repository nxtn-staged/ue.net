// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class ClassProperty : ObjectProperty
    {
        internal ClassProperty(IntPtr pointer) : base(pointer) { }

        public Class MetaClass => Create<Class>(NativeMethods.GetMetaClass(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetMetaClass(IntPtr property);
        }
    }
}
