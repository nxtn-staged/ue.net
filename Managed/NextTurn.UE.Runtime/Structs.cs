// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;

namespace Unreal
{
    internal static class Structs
    {
        private static readonly Dictionary<IntPtr, Type> TypeByPtr = new Dictionary<IntPtr, Type>();
        private static readonly Dictionary<Type, IntPtr> PtrByType = new Dictionary<Type, IntPtr>();

        internal static IntPtr GetStruct(Type type)
        {
            if (PtrByType.TryGetValue(type, out IntPtr @struct))
            {
                return @struct;
            }

            Throw.NotSupportedException();
            return default;
        }

        internal static void Register(IntPtr @struct, Type type)
        {
            TypeByPtr.Add(@struct, type);
            PtrByType.Add(type, @struct);
        }

        internal static void Unregister(IntPtr @struct, Type type)
        {
            _ = TypeByPtr.Remove(@struct);
            _ = PtrByType.Remove(type);
        }
    }
}
