// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [CLSCompliant(false)]
    public sealed class UInt32Property : NumericProperty<uint>
    {
        internal UInt32Property(IntPtr pointer) : base(pointer) { }
    }
}
