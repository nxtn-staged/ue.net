// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class DelegateMethod : Method
    {
        internal DelegateMethod(IntPtr pointer) : base(pointer) { }

        public static unsafe Span<char> NameSuffix
        {
            get
            {
                NativeMethods.GetNameSuffix(out void* suffixPtr, out int suffixLength);
                return new Span<char>(suffixPtr, suffixLength);
            }
        }

        private static new class NativeMethods
        {
            [Calli]
            public static extern unsafe void GetNameSuffix(out void* suffixPtr, out int suffixLength);
        }
    }
}
