// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

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
                this.items.Add(1, layout.Size);
                index = this.items.Count - 1;
                this.elementAllocated.Add(false);
            }

            this.elementAllocated[index] = true;
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

        internal readonly struct Layout
        {
            internal int Alignment { get; }

            internal int Size { get; }
        }

        private struct FreeListNode
        {
            public int Prev;
            public int Next;
        }
    }
}
