// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public class ObjectPropertyBase : Property
    {
        internal ObjectPropertyBase(IntPtr pointer) : base(pointer) { }

        public Class PropertyClass => Create<Class>(NativeMethods.GetPropertyClass(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetPropertyClass(IntPtr property);
        }
    }
}
