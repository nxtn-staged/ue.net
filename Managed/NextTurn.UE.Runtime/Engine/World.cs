// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class World
    {
        internal Actor SpawnActor(Class @class) => new Actor(NativeMethods.SpawnActor(@class.pointer));

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr SpawnActor(IntPtr @class);
        }
    }
}
