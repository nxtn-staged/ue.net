// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

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
