// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

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
