// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Leftice
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct NativeBoolean
    {
        private readonly byte value;

        public static implicit operator NativeBoolean(bool value) => Unsafe.As<bool, NativeBoolean>(ref value);

        public static implicit operator bool(NativeBoolean value) => Unsafe.As<NativeBoolean, bool>(ref value);
    }
}
