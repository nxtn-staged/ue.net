// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Unreal
{
    public struct ScriptMap
    {
        private ScriptSet pairs;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ScriptMap"/>.
        /// </summary>
        /// <value>
        /// The number of elements contained in the <see cref="ScriptMap"/>.
        /// </value>
        public int Count => this.pairs.Count;

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
            this.pairs.Add(
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

        internal IntPtr GetValue(IntPtr key, in Layout layout, Func<IntPtr, int> getHashCode, Func<IntPtr, IntPtr, bool> equals)
        {
            int index = this.IndexOf(key, layout, getHashCode, equals);
            if (index >= 0)
            {
                return default;
            }

            return IntPtr.Zero;
        }

        internal int IndexOf(IntPtr key, in Layout layout, Func<IntPtr, int> getHashCode, Func<IntPtr, IntPtr, bool> equals)
        {
            return this.pairs.Count != 0 ? this.pairs.IndexOf(key, layout.SetLayout, getHashCode, equals) : -1;
        }

        internal readonly struct Layout
        {
            internal int ValueOffset { get; }

            internal ScriptSet.Layout SetLayout { get; }
        }
    }
}
