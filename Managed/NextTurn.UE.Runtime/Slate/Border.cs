// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class Border : CompoundWidget
    {
        private ref readonly NativeBorder Target => ref this.Reference.GetTarget<NativeBorder>();

        public int HorizontalAlignment
        {
            get => this.Target.HorizontalAlignment;
            set => NativeMethods.SetHorizontalAlignment(this.Reference, value);
        }

        public int VerticalAlignment
        {
            get => this.Target.VerticalAlignment;
            set => NativeMethods.SetVerticalAlignment(this.Reference, value);
        }

        private struct NativeBorder
        {
            public int HorizontalAlignment { get; }

            public int VerticalAlignment { get; }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern void SetHorizontalAlignment(in SharedReference border, int horizontalAlignment);

            [Calli]
            public static extern void SetVerticalAlignment(in SharedReference border, int verticalAlignment);
        }
    }
}
