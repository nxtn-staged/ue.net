// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

namespace Unreal
{
    public struct ScriptSparseArray
    {
        private ScriptArray items;
        private ScriptBitArray elementAllocated;
        private int freeList;
        private int freeCount;

        public int Count => this.items.Count - this.freeCount;

        internal unsafe void Add(in Layout layout)
        {
            int index;
            if (this.freeCount != 0)
            {
                index = this.freeList;
                this.freeList = this.GetFreeListNode(this.freeList, layout)->Next;
                this.freeCount--;
                if (this.freeCount != 0)
                {
                    this.GetFreeListNode(this.freeList, layout)->Prev = -1;
                }
            }
            else
            {
                index = this.items.Add(1, layout.Size);
                this.elementAllocated.Add(false);
            }

            this.elementAllocated[index] = true;
        }

        internal void Clear(in Layout layout)
        {
            this.items.Clear(layout.Size);
            this.freeList = -1;
            this.freeCount = 0;
            this.elementAllocated.Clear();
        }

        private unsafe FreeListNode* GetFreeListNode(int index, in Layout layout) => (FreeListNode*)this.GetItem(index, layout);

        internal unsafe void* GetItem(int index, in Layout layout) => (byte*)this.items.Items + index * layout.Size;

        internal unsafe void RemoveRange(int index, int count, in Layout layout)
        {
            for (int i = 0; i < count; i++)
            {
                if (this.freeCount != 0)
                {
                    this.GetFreeListNode(this.freeList, layout)->Prev = index;
                }

                FreeListNode* node = this.GetFreeListNode(index, layout);
                node->Prev = -1;
                node->Next = this.freeCount != 0 ? this.freeList : -1;
                this.freeList = index;
                this.freeCount++;
                this.elementAllocated[index] = false;

                index++;
            }
        }

        private struct FreeListNode
        {
            public int Prev;
            public int Next;
        }

        internal readonly struct Layout
        {
            internal int Alignment { get; }

            internal int Size { get; }
        }

        internal struct IndexEnumerator
        {
            private readonly unsafe ScriptSparseArray* array;
            private readonly int count;
            private ScriptBitArray.IndexEnumerator bitEnumerator;

            internal unsafe IndexEnumerator(ScriptSparseArray* array)
            {
                this.array = array;
                this.count = array->Count;
                this.bitEnumerator = new ScriptBitArray.IndexEnumerator(&array->elementAllocated);
            }

            internal int CurrentIndex => this.bitEnumerator.CurrentIndex;

            internal unsafe void* GetCurrent(in Layout layout) => this.array->GetItem(this.CurrentIndex, layout);

            internal unsafe bool MoveNext()
            {
                if (this.count != this.array->Count)
                {
                    Throw.InvalidOperationException();
                }

                return this.bitEnumerator.MoveNext();
            }
        }
    }
}
