// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class HorizontalBoxSlot : Slot
    {
        private unsafe NativeHorizontalBoxSlot* Target => (NativeHorizontalBoxSlot*)this.pointer;

        public unsafe int HorizontalAlignment
        {
            get => this.Target->HorizontalAlignment;
            set => NativeMethods.SetHorizontalAlignment(this.Target, value);
        }

        public unsafe int VerticalAlignment
        {
            get => this.Target->VerticalAlignment;
            set => NativeMethods.SetVerticalAlignment(this.Target, value);
        }

        private struct NativeHorizontalBoxSlot
        {
            public int HorizontalAlignment { get; }

            public int VerticalAlignment { get; }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void SetHorizontalAlignment(NativeHorizontalBoxSlot* slot, int horizontalAlignment);

            [Calli]
            public static extern unsafe void SetVerticalAlignment(NativeHorizontalBoxSlot* slot, int verticalAlignment);
        }
    }
}
