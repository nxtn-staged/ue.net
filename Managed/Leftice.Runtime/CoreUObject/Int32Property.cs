// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Int32Property : NumericProperty<int>
    {
        internal Int32Property(IntPtr pointer) : base(pointer) { }
    }
}
