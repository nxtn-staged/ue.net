// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    internal readonly struct ScriptSetHelper
    {
        private readonly IntPtr elementProperty;
        private readonly unsafe ScriptSet* set;
        private readonly ScriptSet.Layout layout;

        internal unsafe ScriptSetHelper(SetProperty property, IntPtr set)
        {
            this.elementProperty = SetProperty.UnsafeMethods.GetElementProperty(property.pointer);
            this.set = (ScriptSet*)set;
            this.layout = SetProperty.UnsafeMethods.GetLayout(property.pointer);
        }

        internal unsafe int Count => this.set->Count;

        private void Construct(int index)
        {
        }

        private void Destruct(int index, int count)
        {
        }
    }
}
