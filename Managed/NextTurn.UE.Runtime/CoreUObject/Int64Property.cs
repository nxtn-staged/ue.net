// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Int64Property : NumericProperty<long>
    {
        internal Int64Property(IntPtr pointer) : base(pointer) { }
    }
}
