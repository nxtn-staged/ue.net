// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public struct ScriptMap
    {
        private ScriptSet entries;

        /// <summary>
        /// Gets the number of elements contained in this <see cref="ScriptMap"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in this <see cref="ScriptMap"/>.
        /// </value>
        public int Count => this.entries.Count;

        internal unsafe void Add(
            IntPtr key,
            in Layout layout,
            Func<IntPtr, int> getHashCode,
            Func<IntPtr, IntPtr, bool> equals,
            Action<IntPtr> constructKey,
            Action<IntPtr> constructValue,
            Action<IntPtr> destructKey,
            Action<IntPtr> destructValue)
        {
            int valueOffset = layout.ValueOffset;
            this.entries.Add(
                key,
                layout.SetLayout,
                getHashCode,
                equals,
                pair =>
                {
                    constructKey(pair);
                    constructValue((IntPtr)((byte*)pair + valueOffset));
                },
                pair =>
                {
                    destructKey(pair);
                    destructValue((IntPtr)((byte*)pair + valueOffset));
                });
        }

        internal void Clear(in Layout layout)
        {
            this.entries.Clear(layout.SetLayout);
        }

        internal IntPtr FindValuePtr(IntPtr key, in Layout layout, Func<IntPtr, int> getHashCode, Func<IntPtr, IntPtr, bool> equals)
        {
            int index = this.IndexOf(key, layout, getHashCode, equals);
            return index >= 0 ? this.GetEntryPtr(index, layout) + layout.ValueOffset : IntPtr.Zero;
        }

        private IntPtr GetEntryPtr(int index, in Layout layout)
        {
            return this.entries.GetEntryPtr(index, layout.SetLayout);
        }

        internal IntPtr GetValuePtr(int index, in Layout layout)
        {
            return this.GetEntryPtr(index, layout) + layout.ValueOffset;
        }

        internal int IndexOf(IntPtr key, in Layout layout, Func<IntPtr, int> getHashCode, Func<IntPtr, IntPtr, bool> equals)
        {
            return this.entries.Count != 0 ? this.entries.IndexOf(key, layout.SetLayout, getHashCode, equals) : -1;
        }

        internal void RemoveAt(int index, in Layout layout)
        {
            this.entries.RemoveAt(index, layout.SetLayout);
        }

        internal readonly struct Layout
        {
            internal int ValueOffset { get; }

            internal ScriptSet.Layout SetLayout { get; }
        }
    }
}
