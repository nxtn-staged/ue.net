// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [CLSCompliant(false)]
    public sealed class UInt16Property : NumericProperty<ushort>
    {
        internal UInt16Property(IntPtr pointer) : base(pointer) { }
    }
}
