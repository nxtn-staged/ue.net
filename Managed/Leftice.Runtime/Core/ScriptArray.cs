// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Unreal
{
    public struct ScriptArray
    {
        private Allocator allocator;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ScriptArray"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="ScriptArray"/>.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of elements that the <see cref="ScriptArray"/> can contain.
        /// </summary>
        /// <value>
        /// The number of elements that the <see cref="ScriptArray"/> can contain.
        /// </value>
        public int Capacity { get; private set; }

        internal unsafe void* Items => this.allocator.Allocation;

        internal void Add(int count, int elementSize)
        {
            int oldCount = this.Count;
            this.Count += count;
            if (this.Count > this.Capacity)
            {
                this.ResizeGrow(oldCount, elementSize);
            }
        }

        internal unsafe ReadOnlySpan<T> AsSpan<T>() => new ReadOnlySpan<T>(this.Items, this.Count);

        internal unsafe bool Exists<T>(Predicate<T> match) where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (match(((T*)this.Items)[i]))
                {
                    return true;
                }
            }

            return false;
        }

        internal unsafe T Find<T>(Predicate<T> match) where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (match(((T*)this.Items)[i]))
                {
                    return ((T*)this.Items)[i];
                }
            }

            return default;
        }

        internal unsafe int FindIndex<T>(Predicate<T> match) where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (match(((T*)this.Items)[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        internal unsafe T GetItem<T>(int index) where T : unmanaged
        {
            return *this.GetItemPtr<T>(index);
        }

        internal unsafe T GetItemUnsafe<T>(int index)
        {
            return Unsafe.Read<T>(this.GetItemPtrUnsafe<T>(index));
        }

        internal unsafe T* GetItemPtr<T>(int index) where T : unmanaged
        {
            return ((T*)this.Items) + index;
        }

        private unsafe void* GetItemPtrUnsafe<T>(int index)
        {
            return Unsafe.Add<T>(this.Items, index);
        }

        internal void Insert(int index, int count, int elementSize)
        {
            int oldCount = this.Count;
            this.Count += count;
            if (this.Count > this.Capacity)
            {
                this.ResizeGrow(oldCount, elementSize);
            }
        }

        internal void RemoveRange(int index, int count, int elementSize)
        {

        }

        private void ResizeGrow(int oldCount, int elementSize) => throw new NotImplementedException();

        internal unsafe void SetItemUnsafe<T>(int index, T value)
        {
            Unsafe.Write(this.GetItemPtrUnsafe<T>(index), value);
        }

        private struct Allocator
        {
            internal unsafe void* Allocation;
        }
    }
}
