// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public class NumericProperty : Property
    {
        internal NumericProperty(IntPtr pointer) : base(pointer) { }

        public virtual bool IsFloatingPoint => false;

        public virtual bool IsIntegral => true;
    }
}
