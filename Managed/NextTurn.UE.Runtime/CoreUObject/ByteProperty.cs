// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class ByteProperty : NumericProperty<byte>
    {
        internal ByteProperty(IntPtr pointer) : base(pointer) { }

        public Enum? MetaEnum => Object.CreateOrNull<Enum>(NativeMethods.GetMetaEnum(this.pointer));

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetMetaEnum(IntPtr property);
        }
    }
}
