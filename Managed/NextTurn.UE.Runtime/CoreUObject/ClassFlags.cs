// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    public enum ClassFlags
    {
        None = 0x00000000,

        Abstract = 0x00000001,
        Interface = 0x00004000,
        Deprecated = 0x02000000,
    }
}
