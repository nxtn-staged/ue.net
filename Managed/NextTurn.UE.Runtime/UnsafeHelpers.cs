// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace NextTurn.UE
{
    internal static unsafe class UnsafeHelpers
    {
        internal static T Read<T>(IntPtr source, int byteOffset) where T : unmanaged =>
            *(T*)(source + byteOffset);

        internal static void Write<T>(IntPtr destination, int byteOffset, T value) where T : unmanaged =>
            *(T*)(destination + byteOffset) = value;
    }
}
