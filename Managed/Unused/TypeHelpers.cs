// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace NextTurn.UE.Runtime
{
    internal static class TypeHelpers
    {
        internal static GCHandle GetType(IntPtr typeName) => GCHandle.Alloc(
            Type.GetType(Marshal.PtrToStringUni(typeName)));
    }
}
