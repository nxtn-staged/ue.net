// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [CLSCompliant(false)]
    public sealed class SByteProperty : NumericProperty<sbyte>
    {
        internal SByteProperty(IntPtr pointer) : base(pointer) { }
    }
}
