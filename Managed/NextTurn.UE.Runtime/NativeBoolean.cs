// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NextTurn.UE
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct NativeBoolean
    {
        private readonly byte value;

        public static implicit operator NativeBoolean(bool value) => Unsafe.As<bool, NativeBoolean>(ref value);

        public static implicit operator bool(NativeBoolean value) => Unsafe.As<NativeBoolean, bool>(ref value);
    }
}
