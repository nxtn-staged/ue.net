// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Leftice
{
    internal static class StringMarshal
    {
        internal static Span<char> CreateSpan(string s) => MemoryMarshal.CreateSpan(ref GetReference(s), s.Length);

        internal static ref char GetReference(string s) => ref Unsafe.AsRef(s.GetPinnableReference());
    }
}
