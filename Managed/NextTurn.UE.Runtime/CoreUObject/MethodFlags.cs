// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    [CLSCompliant(false)]
    public enum MethodFlags : uint
    {
        None = 0x00000000,

        Final = 0x00000001,
        Static = 0x00002000,
        Public = 0x00020000,
        Private = 0x00040000,
        Family = 0x00080000,

        HasOutParameters = 0x00400000,
    }
}
