// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct ScriptArrayHelper
    {
        private readonly IntPtr itemProperty;
        private readonly unsafe ScriptArray* array;
        private readonly int itemSize;

        internal unsafe ScriptArrayHelper(ArrayProperty property, IntPtr array)
        {
            this.itemProperty = ArrayProperty.NativeMethods.GetItemProperty(property.pointer);
            this.array = (ScriptArray*)array;
            this.itemSize = Property.NativeMethods.GetElementSize(this.itemProperty);
        }

        internal unsafe int Count => this.array->Count;

        internal unsafe int Add(int count = 1)
        {
            int oldCount = this.array->Add(count, this.itemSize);
            this.Initialize(oldCount, count);
            return oldCount;
        }

        private unsafe void Clear(int index, int count)
        {
            if (Property.UnsafeMethods.HasAllPropertyFlags(this.itemProperty, PropertyFlags.TriviallyDefaultConstructible | PropertyFlags.TriviallyDestructible))
            {
                new Span<byte>(this.GetItemPtr(index), count * this.itemSize).Clear();
            }
            else
            {
            }
        }

        internal unsafe void Insert(int index, int count = 1)
        {
            this.array->Insert(index, count, this.itemSize);
            this.Initialize(index, count);
        }

        private void Finalize(int index, int count)
        {
            if (!Property.UnsafeMethods.HasAnyPropertyFlags(this.itemProperty, PropertyFlags.TriviallyDestructible))
            {
            }
        }

        private unsafe void* GetItemPtr(int index)
        {
            return (byte*)this.array->Items + index * this.itemSize;
        }

        private unsafe void Initialize(int index, int count)
        {
            if (Property.UnsafeMethods.HasAnyPropertyFlags(this.itemProperty, PropertyFlags.TriviallyDefaultConstructible))
            {
                new Span<byte>(this.GetItemPtr(index), count * this.itemSize).Clear();
            }
            else
            {
            }
        }

        internal unsafe void RemoveRange(int index, int count = 1)
        {
            this.Finalize(index, count);
            this.array->RemoveRange(index, count, this.itemSize);
        }
    }
}
