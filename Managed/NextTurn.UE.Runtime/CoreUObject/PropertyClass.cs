// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    internal static class PropertyClass
    {
        internal static class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetBaseClass(IntPtr @class);

            [ReadOffset]
            public static extern ClassTypeFlags GetClassTypeFlags(IntPtr @class);

            public static bool HasAllTypeFlags(IntPtr @class, ClassTypeFlags flags) =>
                (GetClassTypeFlags(@class) & flags) == flags;
        }
    }
}
