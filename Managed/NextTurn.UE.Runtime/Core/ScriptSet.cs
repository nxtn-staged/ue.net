// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public struct ScriptSet
    {
        private ScriptSparseArray entries;
        private Allocator hashAllocator;
        private int bucketSize;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="ScriptSet"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="ScriptSet"/>.
        /// </value>
        public int Count => this.entries.Count;

        internal unsafe void Add(
            IntPtr item,
            in Layout layout,
            Func<IntPtr, int> getHashCode,
            Func<IntPtr, IntPtr, bool> equals,
            Action<IntPtr> initialize,
            Action<IntPtr> finalize)
        {
            int hashCode = getHashCode(item);

            int index = this.IndexOf(item, layout, getHashCode, equals);
            if (index >= 0)
            {
                IntPtr itemPtr = (IntPtr)this.entries.GetItem(index, layout.SparseArrayLayout);

                finalize(itemPtr);
                initialize(itemPtr);
            }
            else
            {
                this.entries.Add(layout.SparseArrayLayout);
                index = this.entries.Count - 1;

                IntPtr itemPtr = (IntPtr)this.entries.GetItem(index, layout.SparseArrayLayout);
                initialize(itemPtr);

                if (this.bucketSize == 0)
                {

                }
                else
                {
                    int bucketIndex = this.GetBucketIndex(hashCode);
                    int* entryIndex = this.GetEntryIndexPtr(bucketIndex);
                    *this.GetBukcetIndexPtr(itemPtr, layout) = bucketIndex;
                    *this.GetNextEntryIndexPtr(itemPtr, layout) = *entryIndex;
                    *entryIndex = index;
                }
            }
        }

        internal void Add(in Layout layout)
        {
            this.entries.Add(layout.SparseArrayLayout);
        }

        internal void Clear(in Layout layout)
        {
            this.entries.Clear(layout.SparseArrayLayout);
            throw new NotImplementedException();
        }

        private int GetBucketIndex(int hashCode) =>
            // hashCode % _bucketSize
            hashCode & (this.bucketSize - 1);

        private unsafe int* GetBukcetIndexPtr(IntPtr item, in Layout layout)
        {
            return (int*)((byte*)item + layout.BucketIndexOffset);
        }

        private unsafe int* GetEntryIndexPtr(int bucketIndex)
        {
            return (int*)this.hashAllocator.Allocation + bucketIndex;
        }

        internal unsafe IntPtr GetEntryPtr(int index, in Layout layout)
        {
            return new IntPtr(this.entries.GetItem(index, layout.SparseArrayLayout));
        }

        private unsafe int* GetNextEntryIndexPtr(IntPtr item, in Layout layout)
        {
            return (int*)((byte*)item + layout.NextEntryIndexOffset);
        }

        internal unsafe int IndexOf(IntPtr item, in Layout layout, Func<IntPtr, int> getHashCode, Func<IntPtr, IntPtr, bool> equals)
        {
            if (this.entries.Count != 0)
            {
                int bucketIndex = this.GetBucketIndex(getHashCode(item));

                IntPtr currentItem = IntPtr.Zero;
                for (int entryIndex = *this.GetEntryIndexPtr(bucketIndex); entryIndex >= 0; entryIndex = *this.GetNextEntryIndexPtr(currentItem, layout))
                {
                    currentItem = (IntPtr)this.entries.GetItem(entryIndex, layout.SparseArrayLayout);
                    if (equals(item, currentItem))
                    {
                        return entryIndex;
                    }
                }
            }

            return -1;
        }

        internal unsafe void Rehash(in Layout layout, Func<IntPtr, int> getHashCode)
        {
            this.hashAllocator.ResizeAllocation();
            this.bucketSize = default;
            if (this.bucketSize != 0)
            {
                for (int bucketIndex = 0; bucketIndex < this.bucketSize; bucketIndex++)
                {
                    *this.GetEntryIndexPtr(bucketIndex) = -1;
                }

                int index = 0;
                int count = this.entries.Count;
                while (count != 0)
                {
                    if (index >= 0)
                    {
                        int elementId = index;

                        IntPtr item = (IntPtr)this.entries.GetItem(index, layout.SparseArrayLayout);

                        int elementHashCode = getHashCode(item);
                        int bucketIndex = this.GetBucketIndex(elementHashCode);
                        *this.GetBukcetIndexPtr(item, layout) = bucketIndex;

                        *this.GetNextEntryIndexPtr(item, layout) = *this.GetEntryIndexPtr(bucketIndex);
                        *this.GetEntryIndexPtr(bucketIndex) = elementId;

                        count--;
                    }

                    index++;
                }
            }
        }

        internal unsafe void RemoveAt(int index, in Layout layout)
        {
            IntPtr element = (IntPtr)this.entries.GetItem(index, layout.SparseArrayLayout);

            for (int* entryIndex = this.GetEntryIndexPtr(*this.GetBukcetIndexPtr(element, layout));
                entryIndex >= null;
                entryIndex = this.GetNextEntryIndexPtr((IntPtr)this.entries.GetItem(*entryIndex, layout.SparseArrayLayout), layout))
            {
                if (*entryIndex == index)
                {
                    *entryIndex = *this.GetNextEntryIndexPtr(element, layout);
                    break;
                }
            }

            this.entries.RemoveRange(index, 1, layout.SparseArrayLayout);
        }

        internal readonly struct Layout
        {
            internal int NextEntryIndexOffset { get; }

            internal int BucketIndexOffset { get; }

            internal int Size { get; }

            internal ScriptSparseArray.Layout SparseArrayLayout { get; }
        }

        private struct Allocator
        {
            internal unsafe void* Allocation => throw new NotImplementedException();

            internal void ResizeAllocation() => throw new NotImplementedException();
        }
    }
}
