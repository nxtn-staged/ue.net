// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public struct ScriptArray
    {
        private Allocator allocator;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="ScriptArray"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="ScriptArray"/>.
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of elements that this <see cref="ScriptArray"/> can contain.
        /// </summary>
        /// <value>
        /// The number of elements that this <see cref="ScriptArray"/> can contain.
        /// </value>
        public int Capacity { get; private set; }

        internal unsafe void* Items => this.allocator.Allocation;

        internal int Add(int count, int itemSize)
        {
            int oldCount = this.Count;
            this.Count += count;
            if (this.Count > this.Capacity)
            {
                this.ResizeGrow(oldCount, itemSize);
            }

            return oldCount;
        }

        internal unsafe ReadOnlySpan<T> AsSpan<T>() => new ReadOnlySpan<T>(this.Items, this.Count);

        internal unsafe bool Any<T>(Predicate<T> predicate)
            where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                T item = ((T*)this.Items)[i];
                if (predicate(item))
                {
                    return true;
                }
            }

            return false;
        }

        internal void Clear(int itemSize)
        {
            this.Count = 0;
            this.Resize(0, itemSize);
        }

        internal unsafe T FirstOrDefault<T>(Predicate<T> match)
            where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                T item = ((T*)this.Items)[i];
                if (match(item))
                {
                    return item;
                }
            }

            return default;
        }

        internal unsafe int FindIndex<T>(Predicate<T> match)
            where T : unmanaged
        {
            for (int i = 0; i < this.Count; i++)
            {
                T item = ((T*)this.Items)[i];
                if (match(item))
                {
                    return i;
                }
            }

            return -1;
        }

        internal unsafe T GetItem<T>(int index)
            where T : unmanaged
        {
            return *this.GetItemPtr<T>(index);
        }

        internal unsafe T GetItemUnsafe<T>(int index)
        {
            return Unsafe.Read<T>(this.GetItemPtrUnsafe<T>(index));
        }

        internal unsafe T* GetItemPtr<T>(int index)
            where T : unmanaged
        {
            return ((T*)this.Items) + index;
        }

        internal unsafe void* GetItemPtrUnsafe<T>(int index)
        {
            return Unsafe.Add<T>(this.Items, index);
        }

        internal void Insert(int index, int count, int itemSize)
        {
            int oldCount = this.Count;
            this.Count += count;
            if (this.Count > this.Capacity)
            {
                this.ResizeGrow(oldCount, itemSize);
            }
        }

        internal void RemoveRange(int index, int count, int itemSize)
        {

        }

        internal void Resize(int newCapacity, int itemSize) =>
            throw new NotImplementedException();

        private void ResizeGrow(int oldCount, int itemSize) => NativeMethods.ResizeGrow(ref this, oldCount, itemSize);

        internal unsafe void SetItemUnsafe<T>(int index, T value)
        {
            Unsafe.Write(this.GetItemPtrUnsafe<T>(index), value);
        }

        private struct Allocator
        {
            internal unsafe void* Allocation;
        }

        internal struct Enumerator<T> : IEnumerable<T>, IEnumerator<T>
            where T : unmanaged
        {
            private unsafe ScriptArray* array;
            private readonly int count;
            private int index;
            private T current;

            internal unsafe Enumerator(ScriptArray* array)
            {
                this.array = array;
                this.count = array->Count;
                this.index = 0;
                this.current = default;
            }

            public T Current => this.current;

            unsafe object? IEnumerator.Current => (uint)this.index > (uint)this.array->Count ? throw new InvalidOperationException() : this.Current;

            public void Dispose() { }

            public Enumerator<T> GetEnumerator()
            {
                Enumerator<T> result = this;
                result.Reset();
                return result;
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

            /// <exception cref="InvalidOperationException"></exception>
            public unsafe bool MoveNext()
            {
                if (this.count != this.array->Count)
                {
                    Throw.InvalidOperationException();
                }

                if ((uint)this.index < (uint)this.array->Count)
                {
                    this.current = this.array->GetItem<T>(this.index);
                    this.index++;
                    return true;
                }

                this.index = this.array->Count + 1;
                this.current = default;
                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default;
            }
        }

        internal static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Finalize(ScriptArray* array);

            [Calli]
            public static extern void ResizeGrow(ref ScriptArray array, int oldCount, int itemSize);
        }
    }
}
