// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    internal static class NameEntry
    {
        internal static class UnsafeMethods
        {
            internal static unsafe string GetPlainNameString(IntPtr entry, int additionalLength = 0)
            {
                void* namePtr = NativeMethods.GetName(entry);
                ushort header = NativeMethods.GetHeader(entry);
                int length = header & NameEntryHeader.NativeMethods.Length_Mask;
                string result = new string('\0', length + additionalLength);
                if ((header & NameEntryHeader.NativeMethods.IsWide_Mask) != 0)
                {
                    new ReadOnlySpan<char>(namePtr, length).CopyTo(
                        MemoryMarshal.CreateSpan(ref StringMarshal.GetReference(result), length));
                    return result;
                }
                else
                {
                    ref char first = ref StringMarshal.GetReference(result);
                    for (int i = 0; i < length; i++)
                    {
                        Unsafe.Add(ref first, i) = (char)((byte*)namePtr)[i];
                    }

                    return result;
                }
            }
        }

        private static class NativeMethods
        {
            [ReadOffset]
            public static extern ushort GetHeader(IntPtr entry);

            [PointerOffset]
            public static extern unsafe void* GetName(IntPtr entry);
        }

        private static class NameEntryHeader
        {
            internal static class NativeMethods
            {
                public static readonly ushort IsWide_Mask;
                public static readonly ushort Length_Mask;
            }
        }
    }
}
