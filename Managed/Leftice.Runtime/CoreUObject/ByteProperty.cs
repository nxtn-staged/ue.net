// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class ByteProperty : NumericProperty<byte>
    {
        internal ByteProperty(IntPtr pointer) : base(pointer) { }
    }
}
