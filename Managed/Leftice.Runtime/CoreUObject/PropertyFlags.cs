// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    [Flags]
    public enum PropertyFlags : long
    {
        None = 0x0000000000000001,

        ReadOnlyParameter = 0x0000000000000002,
        Parameter = 0x0000000000000080,
        OutParameter = 0x0000000000000100,
        TriviallyDefaultConstructible = 0x0000000000000200,
        ReturnParameter = 0x0000000000000400,
        Deprecated = 0x0000000020000000,
        ByReferenceParameter = 0x0000000008000000,
        Trivial = 0x0000000040000000,
        TriviallyDestructible = 0x0000001000000000,
    }
}
