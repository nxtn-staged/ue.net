// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Unreal.Slate
{
    internal struct Binding<T>
        where T : unmanaged
    {
        internal T Value;
        internal bool IsSet;
        internal IntPtr Getter;

        public Binding(T value)
        {
            this.Value = value;
            this.IsSet = true;
            this.Getter = IntPtr.Zero;
        }

        public Binding(Func<T> getter)
        {
            this.Value = default;
            this.IsSet = true;
            this.Getter = Marshal.GetFunctionPointerForDelegate(getter);
        }
    }
}
