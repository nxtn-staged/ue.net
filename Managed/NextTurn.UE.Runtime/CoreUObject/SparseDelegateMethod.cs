// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class SparseDelegateMethod : DelegateMethod
    {
        internal SparseDelegateMethod(IntPtr pointer) : base(pointer) { }

        public Name DelegateName => NativeMethods.GetDelegateName(this.pointer);

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern Name GetDelegateName(IntPtr function);
        }
    }
}
