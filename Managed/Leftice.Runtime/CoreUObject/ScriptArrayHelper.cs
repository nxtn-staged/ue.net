// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct ScriptArrayHelper
    {
        private readonly IntPtr elementProperty;
        private readonly unsafe ScriptArray* array;
        private readonly int elementSize;

        internal unsafe ScriptArrayHelper(ArrayProperty property, IntPtr array)
        {
            this.elementProperty = ArrayProperty.NativeMethods.GetElementProperty(property.pointer);
            this.array = (ScriptArray*)array;
            this.elementSize = Property.NativeMethods.GetElementSize(this.elementProperty);
        }

        internal unsafe int Count => this.array->Count;

        private void Clear(int index, int count)
        {
        }

        private void Construct(int index, int count)
        {
        }

        private void Destruct(int index, int count)
        {
        }
    }
}
