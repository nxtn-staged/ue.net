// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public abstract class MulticastDelegateProperty : Property, IProperty<MulticastScriptDelegate>
    {
        internal MulticastDelegateProperty(IntPtr pointer) : base(pointer) { }

        public DelegateMethod SignatureMethod => Object.Create<DelegateMethod>(NativeMethods.GetSignatureMethod(this.pointer));

        public abstract MulticastScriptDelegate GetValue(Object @object, int index = 0);

        public abstract void SetValue(Object @object, MulticastScriptDelegate value, int index = 0);

        private protected static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetSignatureMethod(IntPtr property);

            [Calli]
            public static extern unsafe MulticastScriptDelegate* GetValuePointer(IntPtr property, IntPtr @object, int index);

            [Calli]
            public static extern void SetValue(IntPtr property, IntPtr @object, int index, in MulticastScriptDelegate value);
        }
    }
}
