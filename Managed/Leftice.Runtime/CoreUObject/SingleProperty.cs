// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class SingleProperty : NumericProperty<float>
    {
        internal SingleProperty(IntPtr pointer) : base(pointer) { }

        public override bool IsFloatingPoint => true;

        public override bool IsIntegral => false;
    }
}
