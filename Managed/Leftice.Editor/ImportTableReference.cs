// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;

namespace Leftice
{
    internal static class ImportTableReference
    {
        internal static ImportTable Get(string typeName) => ImportTable.Get(typeName);

        internal static void Dispose(this ImportTable @this) => @this.Dispose();

        internal static IntPtr GetField(this ImportTable @this, string key) => @this.GetField(key);

        internal static IntPtr GetMethod(this ImportTable @this, string key) => @this.GetMethod(key);

        internal static int GetOffset(this ImportTable @this, string key) => @this.GetOffset(key);

        internal static IntPtr GetPointer(this ImportTable @this, string key) => @this.GetPointer(key);
    }
}
