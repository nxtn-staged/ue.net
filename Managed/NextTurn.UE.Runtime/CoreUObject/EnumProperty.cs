// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class EnumProperty : Property
    {
        internal EnumProperty(IntPtr pointer) : base(pointer) { }

        public Enum MetaEnum => Object.Create<Enum>(NativeMethods.GetMetaEnum(this.pointer));

        public NumericProperty UnderlyingProperty => Create<NumericProperty>(NativeMethods.GetUnderlyingProperty(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetMetaEnum(IntPtr property);

            [ReadOffset]
            public static extern IntPtr GetUnderlyingProperty(IntPtr property);
        }
    }
}
