// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Leftice
{
    internal readonly ref struct ImportTable
    {
        private static readonly Dictionary<string, Dictionary<string, IntPtr>> Pairs;

        private readonly Dictionary<string, IntPtr> pairs;

        internal static ImportTable Get(string typeName) => throw new NotImplementedException();

        public void Dispose() => Debug.Assert(this.pairs.Count == 0);

        internal IntPtr GetField(string key) => this.GetPointer(key);

        internal IntPtr GetMethod(string key) => this.GetPointer(key);

        internal int GetOffset(string key) => this.GetPointer(key).ToInt32();

        internal IntPtr GetPointer(string key) => this.pairs.Remove(key, out IntPtr value) ? value : throw new KeyNotFoundException();
    }
}
