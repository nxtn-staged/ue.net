// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class Member : Object
    {
        internal Member(IntPtr pointer) : base(pointer) { }

        public Member? Next => CreateOrNull<Member>(NativeMethods.GetNext(this.pointer));

        internal static new class NativeMethods
        {
            [ReadOffset]
            public static extern IntPtr GetNext(IntPtr member);
        }
    }
}
