// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class GridPanelSlot : Slot
    {
        private unsafe NativeGridPanelSlot* Target => (NativeGridPanelSlot*)this.pointer;

        public unsafe int Column
        {
            get => this.Target->Column;
            set => NativeMethods.SetColumn(this.Target, value);
        }

        public unsafe int ColumnSpan
        {
            get => this.Target->ColumnSpan;
            set => NativeMethods.SetColumnSpan(this.Target, value);
        }

        public unsafe int Row
        {
            get => this.Target->Row;
            set => NativeMethods.SetRow(this.Target, value);
        }

        public unsafe int RowSpan
        {
            get => this.Target->RowSpan;
            set => NativeMethods.SetRowSpan(this.Target, value);
        }

        private struct NativeGridPanelSlot
        {
            public int Column { get; }

            public int ColumnSpan { get; }

            public int Row { get; }

            public int RowSpan { get; }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void SetColumn(NativeGridPanelSlot* slot, int column);

            [Calli]
            public static extern unsafe void SetColumnSpan(NativeGridPanelSlot* slot, int columnSpan);

            [Calli]
            public static extern unsafe void SetRow(NativeGridPanelSlot* slot, int row);

            [Calli]
            public static extern unsafe void SetRowSpan(NativeGridPanelSlot* slot, int rowSpan);
        }
    }
}
