// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public sealed class DoubleProperty : NumericProperty<double>
    {
        internal DoubleProperty(IntPtr pointer) : base(pointer) { }

        public override bool IsFloatingPoint => true;

        public override bool IsIntegral => false;
    }
}
