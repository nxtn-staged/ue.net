// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NextTurn.UE.Runtime
{
    internal static class CalliHelpers
    {
        internal static void BindFunctionPointer(IntPtr typeName, IntPtr fieldName, IntPtr ptr) => _ =
            Type.GetType(Marshal.PtrToStringUni(typeName)).InvokeMember(
                Marshal.PtrToStringUni(fieldName),
                BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                null, null, new object[] { ptr });
    }
}
