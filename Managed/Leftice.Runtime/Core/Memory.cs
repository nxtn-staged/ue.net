// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    internal static class Memory
    {
        internal static unsafe T* Alloc<T>() where T : unmanaged => (T*)NativeMethods.Alloc(sizeof(T));

        internal static unsafe void Free(void* ptr) => NativeMethods.Free(ptr);

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr Alloc(int size);

            [Calli]
            public static extern unsafe void Free(void* ptr);
        }
    }
}
