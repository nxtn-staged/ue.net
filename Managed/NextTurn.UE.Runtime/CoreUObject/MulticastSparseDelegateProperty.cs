// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class MulticastSparseDelegateProperty : MulticastDelegateProperty
    {
        internal MulticastSparseDelegateProperty(IntPtr pointer) : base(pointer) { }

        public override unsafe MulticastScriptDelegate GetValue(Object @object, int index = 0)
        {
            if ((uint)index >= (uint)this.ArrayLength)
            {
                Throw.IndexArgumentOutOfRangeException();
            }

            return *NativeMethods.GetValuePointer(this.pointer, @object.pointer, index);
        }

        public override void SetValue(Object @object, MulticastScriptDelegate value, int index = 0)
        {
            if ((uint)index >= (uint)this.ArrayLength)
            {
                Throw.IndexArgumentOutOfRangeException();
            }

            NativeMethods.SetValue(this.pointer, @object.pointer, index, value);
        }
    }
}
