// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Leftice
{
    internal static unsafe class UnsafeHelpers
    {
        internal static T Read<T>(IntPtr source, int byteOffset) where T : unmanaged =>
            *(T*)(source + byteOffset);

        internal static void Write<T>(IntPtr destination, int byteOffset, T value) where T : unmanaged =>
            *(T*)(destination + byteOffset) = value;
    }
}
