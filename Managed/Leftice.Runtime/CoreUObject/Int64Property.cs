// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Int64Property : NumericProperty<long>
    {
        internal Int64Property(IntPtr pointer) : base(pointer) { }
    }
}
