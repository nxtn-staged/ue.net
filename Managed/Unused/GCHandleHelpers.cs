// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Runtime.InteropServices;

namespace NextTurn.UE.Runtime
{
    internal static class GCHandleHelpers
    {
        internal static void Free(GCHandle value) => value.Free();
    }
}
