// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    public struct ScriptBitArray
    {
        private InlineAllocator4 allocator;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ScriptBitArray"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="ScriptBitArray"/>.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of elements that the <see cref="ScriptBitArray"/> can contain.
        /// </summary>
        /// <value>
        /// The number of elements that the <see cref="ScriptBitArray"/> can contain.
        /// </value>
        public int Capacity { get; private set; }

        /// <summary>
        /// Gets or sets the value of the bit at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the value to get or set.
        /// </param>
        /// <value>
        /// The value of the bit at the specified <paramref name="index"/>.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.
        /// -or-
        /// <paramref name="index"/> is greater than or equal to <see cref="Count"/>.
        /// </exception>
        public unsafe bool this[int index]
        {
            get
            {
                if ((uint)index >= (uint)this.Count)
                {
                    this.ThrowIndexOutOfRangeException();
                }

                return (this.GetSegment(index >> 5) & (1 << index)) != 0;
            }

            set
            {
                if ((uint)index >= (uint)this.Count)
                {
                    this.ThrowIndexOutOfRangeException();
                }

                int mask = 1 << index;
                ref int segment = ref this.GetSegment(index >> 5);

                if (value)
                {
                    segment |= mask;
                }
                else
                {
                    segment &= ~mask;
                }
            }
        }

        public void Add(bool value)
        {
            int count = this.Count;
            this.Count++;
            if (this.Count > this.Capacity)
            {
                this.ResizeGrow(count);
            }

            this[count] = value;
        }

        public void Clear()
        {
            this.Count = 0;

            if (this.Capacity != 0)
            {
                this.Capacity = 0;
                this.Resize(0);
            }
        }

        private ref int GetSegment(int index) => ref Unsafe.Add(ref this.allocator.Allocation, index);

        private unsafe void Resize(int oldCount)
        {
            fixed (ScriptBitArray* thisPtr = &this)
            {
                NativeMethods.Resize(thisPtr, oldCount);
            }
        }

        private unsafe void ResizeGrow(int oldCount)
        {
            fixed (ScriptBitArray* thisPtr = &this)
            {
                NativeMethods.ResizeGrow(thisPtr, oldCount);
            }
        }

        [DoesNotReturn]
        private void ThrowIndexOutOfRangeException() => throw new ArgumentOutOfRangeException("index");

        private struct InlineAllocator4
        {
            private unsafe fixed int inlineAllocation[4];
            private HeapAllocator secondaryAllocator;

            internal unsafe ref int Allocation =>
                ref this.secondaryAllocator.Allocation != null
                    ? ref Unsafe.AsRef<int>(this.secondaryAllocator.Allocation)
                    : ref this.inlineAllocation[0];
        }

        private struct HeapAllocator
        {
            internal unsafe void* Allocation;
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Resize(ScriptBitArray* array, int oldCount);

            [Calli]
            public static extern unsafe void ResizeGrow(ScriptBitArray* array, int oldCount);
        }
    }
}
