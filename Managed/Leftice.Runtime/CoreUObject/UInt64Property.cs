// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [CLSCompliant(false)]
    public sealed class UInt64Property : NumericProperty<ulong>
    {
        internal UInt64Property(IntPtr pointer) : base(pointer) { }
    }
}
