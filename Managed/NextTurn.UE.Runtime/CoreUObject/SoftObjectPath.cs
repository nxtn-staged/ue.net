// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SoftObjectPath : IEquatable<SoftObjectPath>
    {
        private readonly Name unused1;
        private readonly ScriptArray unused2;

        public unsafe bool Equals(SoftObjectPath other) => NativeMethods.Equals(this, &other);

        public override int GetHashCode() => NativeMethods.GetHashCode(this);

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe bool Equals(in SoftObjectPath path, SoftObjectPath* other);

            [Calli]
            public static extern int GetHashCode(in SoftObjectPath path);
        }
    }
}
