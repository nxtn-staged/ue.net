// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NextTurn.UE
{
    internal static class StringMarshal
    {
        internal static Span<char> CreateSpan(string s) => MemoryMarshal.CreateSpan(ref GetReference(s), s.Length);

        internal static ref char GetReference(string s) => ref Unsafe.AsRef(s.GetPinnableReference());
    }
}
