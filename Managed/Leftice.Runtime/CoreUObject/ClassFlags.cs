// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    public enum ClassFlags
    {
        None = 0x00000000,

        Abstract = 0x00000001,
        Deprecated = 0x02000000
    }
}
