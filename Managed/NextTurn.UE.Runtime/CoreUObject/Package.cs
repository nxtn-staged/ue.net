// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class Package : Object
    {
        public static Package Any { get; } = default!;

        internal Package(IntPtr pointer) : base(pointer) { }

        public long FileLength => NativeMethods.GetFileLength(this.pointer);

        public Name FileName => NativeMethods.GetFileName(this.pointer);

        public Guid Guid => NativeMethods.GetGuid(this.pointer);

        private static new class NativeMethods
        {
            [ReadOffset]
            public static extern long GetFileLength(IntPtr package);

            [ReadOffset]
            public static extern Name GetFileName(IntPtr package);

            [ReadOffset]
            public static extern Guid GetGuid(IntPtr package);
        }
    }
}
