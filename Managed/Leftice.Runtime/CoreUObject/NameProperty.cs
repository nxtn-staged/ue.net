// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public sealed class NameProperty : Property, IProperty<Name>
    {
        internal NameProperty(IntPtr pointer) : base(pointer) { }

        public Name GetValue(Object @object, int index = 0) => this.GetValue<Name>(@object, index);

        public void SetValue(Object @object, Name value, int index = 0) => this.SetValue<Name>(@object, value, index);
    }
}
