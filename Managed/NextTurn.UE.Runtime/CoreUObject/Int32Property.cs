// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Int32Property : NumericProperty<int>
    {
        internal Int32Property(IntPtr pointer) : base(pointer) { }
    }
}
