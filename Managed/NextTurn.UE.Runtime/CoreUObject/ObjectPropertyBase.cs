// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class ObjectPropertyBase : Property
    {
        internal ObjectPropertyBase(IntPtr pointer) : base(pointer) { }

        public Class PropertyClass => Object.Create<Class>(NativeMethods.GetPropertyClass(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetPropertyClass(IntPtr property);
        }
    }
}
