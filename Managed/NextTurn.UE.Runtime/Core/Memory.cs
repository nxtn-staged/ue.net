// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class Memory
    {
        /// <summary>
        /// Allocates a memory block from the engine memory allocator.
        /// </summary>
        /// <param name="size">
        /// The size in bytes of the memory block to be allocated.
        /// </param>
        /// <param name="alignment">
        /// The alignment in bytes of the memory block to be allocated.
        /// </param>
        /// <returns>
        /// An integer representing the address of the memory block.
        /// This memory block must be released with <see cref="Free"/>.
        /// </returns>
        public static unsafe IntPtr Alloc(int size, int alignment = 0) =>
            new IntPtr(NativeMethods.Alloc(size, alignment));

        internal static unsafe T* Alloc<T>()
            where T : unmanaged =>
            (T*)NativeMethods.Alloc(sizeof(T), 0);

        /// <summary>
        /// Releases a memory block allocated with <see cref="Alloc"/>.
        /// </summary>
        /// <param name="ptr">
        /// An integer representing the address of the memory block allocated with <see cref="Alloc"/>.
        /// </param>
        public static unsafe void Free(IntPtr ptr) =>
            NativeMethods.Free(ptr.ToPointer());

        internal static unsafe void Free(void* ptr) =>
            NativeMethods.Free(ptr);

        /// <summary>
        /// Reallocates a memory block allocated with <see cref="Alloc"/>.
        /// </summary>
        /// <param name="ptr">
        /// An integer representing the address of the memory block allocated with <see cref="Alloc"/>.
        /// </param>
        /// <param name="size">
        /// The new size in bytes of the memory block to be reallocated.
        /// </param>
        /// <param name="alignment">
        /// The new alignment in bytes of the memory block to be reallocated.
        /// </param>
        /// <returns>
        /// An integer representing the address of the reallocated memory block.
        /// This memory block must be released with <see cref="Free"/>.
        /// </returns>
        public static unsafe IntPtr ReAlloc(IntPtr ptr, int size, int alignment = 0) =>
            new IntPtr(NativeMethods.ReAlloc(ptr.ToPointer(), size, alignment));

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void* Alloc(int size, int alignment);

            [Calli]
            public static extern unsafe void Free(void* ptr);

            [Calli]
            public static extern unsafe void* ReAlloc(void* ptr, int size, int alignment);
        }
    }
}
