// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

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
