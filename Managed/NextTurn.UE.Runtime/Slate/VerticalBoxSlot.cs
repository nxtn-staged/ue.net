// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class VerticalBoxSlot : Slot
    {
        private unsafe NativeVerticalBoxSlot* Target => (NativeVerticalBoxSlot*)this.pointer;

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

        private struct NativeVerticalBoxSlot
        {
            public int HorizontalAlignment { get; }

            public int VerticalAlignment { get; }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void SetHorizontalAlignment(NativeVerticalBoxSlot* slot, int horizontalAlignment);

            [Calli]
            public static extern unsafe void SetVerticalAlignment(NativeVerticalBoxSlot* slot, int verticalAlignment);
        }
    }
}
