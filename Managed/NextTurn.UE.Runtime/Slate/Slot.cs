// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class Slot
    {
        private protected readonly IntPtr pointer;

        public void Attach(Widget widget) => NativeMethods.Attach(this.pointer, widget.Reference);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Attach(IntPtr slot, in SharedReference widget);
        }
    }
}
