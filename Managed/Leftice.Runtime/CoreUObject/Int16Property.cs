// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Int16Property : NumericProperty<short>
    {
        internal Int16Property(IntPtr pointer) : base(pointer) { }
    }
}
