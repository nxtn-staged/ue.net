// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct ScriptMapHelper
    {
        private readonly IntPtr keyProperty;
        private readonly IntPtr valueProperty;
        private readonly unsafe ScriptMap* map;
        private readonly ScriptMap.Layout layout;

        internal unsafe ScriptMapHelper(MapProperty property, IntPtr map)
        {
            this.keyProperty = MapProperty.UnsafeMethods.GetKeyProperty(property.pointer);
            this.valueProperty = MapProperty.UnsafeMethods.GetValueProperty(property.pointer);
            this.map = (ScriptMap*)map;
            this.layout = MapProperty.UnsafeMethods.GetLayout(property.pointer);
        }

        internal unsafe int Count => this.map->Count;

        private void Construct(int index)
        {
        }

        private void Destruct(int index, int count)
        {
        }
    }
}
