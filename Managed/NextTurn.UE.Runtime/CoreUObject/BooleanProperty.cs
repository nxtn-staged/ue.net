// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class BooleanProperty : Property, IProperty<bool>
    {
        internal BooleanProperty(IntPtr pointer) : base(pointer) { }

        public bool GetValue(Object @object, int index = 0)
        {
            if ((uint)index >= (uint)this.ArrayLength)
            {
                Throw.IndexArgumentOutOfRangeException();
            }

            return NativeMethods.GetValue(this.pointer, Object.GetPointerOrThrow(@object), index);
        }

        public void SetValue(Object @object, bool value, int index = 0)
        {
            if ((uint)index >= (uint)this.ArrayLength)
            {
                Throw.IndexArgumentOutOfRangeException();
            }

            NativeMethods.SetValue(this.pointer, Object.GetPointerOrThrow(@object), value, index);
        }

        private static new class NativeMethods
        {
            [Calli]
            public static extern bool GetValue(IntPtr property, IntPtr @object, int index);

            [Calli]
            public static extern void SetValue(IntPtr property, IntPtr @object, bool value, int index);
        }
    }
}
