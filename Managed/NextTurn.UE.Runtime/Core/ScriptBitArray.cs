// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public struct ScriptBitArray
    {
        private InlineAllocator4 allocator;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="ScriptBitArray"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="ScriptBitArray"/>.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of elements that this <see cref="ScriptBitArray"/> can contain.
        /// </summary>
        /// <value>
        /// The number of elements that this <see cref="ScriptBitArray"/> can contain.
        /// </value>
        public int Capacity { get; private set; }

        /// <summary>
        /// Gets or sets the value of the bit at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the value to get or set.
        /// </param>
        /// <value>
        /// The value of the bit at the specified index.
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
                    Throw.IndexArgumentOutOfRangeException();
                }

                return (this.GetSegment(index >> 5) & (1 << index)) != 0;
            }

            set
            {
                if ((uint)index >= (uint)this.Count)
                {
                    Throw.IndexArgumentOutOfRangeException();
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

        private void Resize(int oldCount) => NativeMethods.Resize(ref this, oldCount);

        private void ResizeGrow(int oldCount) => NativeMethods.ResizeGrow(ref this, oldCount);

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

        internal struct IndexEnumerator
        {
            private readonly unsafe ScriptBitArray* array;
            private int segmentIndex;
            private int unvisitedMask;
            private int current;

            internal unsafe IndexEnumerator(ScriptBitArray* array)
            {
                this.array = array;
                this.segmentIndex = 0;
                this.unvisitedMask = ~0;
                this.current = 0;
            }

            internal int CurrentIndex => this.current;

            private static int CountTrailingZeros(int value)
            {
                if (value == 0)
                {
                    return 32;
                }

                int count = 0;
                if ((value & 0b1111_1111_1111_1111) == 0)
                {
                    value >>= 16;
                    count += 16;
                }

                if ((value & 0b1111_1111) == 0)
                {
                    value >>= 8;
                    count += 8;
                }

                if ((value & 0b1111) == 0)
                {
                    value >>= 4;
                    count += 4;
                }

                if ((value & 0b11) == 0)
                {
                    value >>= 2;
                    count += 2;
                }

                if ((value & 0b1) == 0)
                {
                    count += 1;
                }

                return count;
            }

            internal unsafe bool MoveNext()
            {
                int remainingMask = this.array->GetSegment(this.segmentIndex) & this.unvisitedMask;
                while (remainingMask == 0)
                {
                    this.segmentIndex++;
                    if (this.segmentIndex > (this.array->Count - 1) >> 5)
                    {
                        return false;
                    }

                    remainingMask = this.array->GetSegment(this.segmentIndex);
                    this.unvisitedMask = ~0;
                }

                this.current = CountTrailingZeros(remainingMask & -remainingMask);

                return this.current < this.array->Count;
            }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern void Resize(ref ScriptBitArray array, int oldCount);

            [Calli]
            public static extern void ResizeGrow(ref ScriptBitArray array, int oldCount);
        }
    }
}
