// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    public sealed class BooleanProperty : Property, IProperty<bool>
    {
        internal BooleanProperty(IntPtr pointer) : base(pointer) { }

        public bool GetValue(Object @object, int index = 0) =>
            NativeMethods.GetValue(this.pointer, GetPointerOrThrow(@object),
                (uint)index < (uint)this.ArrayLength ? index : throw new ArgumentOutOfRangeException(nameof(index)));

        public void SetValue(Object @object, bool value, int index = 0) =>
            NativeMethods.SetValue(this.pointer, GetPointerOrThrow(@object), value,
                (uint)index < (uint)this.ArrayLength ? index : throw new ArgumentOutOfRangeException(nameof(index)));

        private static new class NativeMethods
        {
            [Calli]
            public static extern bool GetValue(IntPtr property, IntPtr @object, int index);

            [Calli]
            public static extern void SetValue(IntPtr property, IntPtr @object, bool value, int index);
        }
    }
}
