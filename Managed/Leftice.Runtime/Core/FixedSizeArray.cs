// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public struct FixedSizeArray<T> where T : unmanaged
    {
        private readonly IntPtr pointer;
        private readonly int length;

        public FixedSizeArray(IntPtr pointer, int length)
        {
            this.pointer = pointer;
            this.length = length;
        }

        public unsafe T this[int index]
        {
            get => *(T*)this.pointer;
            set => *(T*)this.pointer = value;
        }
    }
}
