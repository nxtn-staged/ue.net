// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

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
