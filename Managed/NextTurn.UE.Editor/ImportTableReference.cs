// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace NextTurn.UE
{
    internal static class ImportTableReference
    {
        internal static ImportTable Get(string typeName, int count) => ImportTable.Get(typeName, count);

        internal static IntPtr GetField(this ImportTable @this, int index) => @this.GetField(index);

        internal static IntPtr GetMethod(this ImportTable @this, int index) => @this.GetMethod(index);

        internal static int GetOffset(this ImportTable @this, int index) => @this.GetOffset(index);

        internal static IntPtr GetPointer(this ImportTable @this, int index) => @this.GetPointer(index);
    }
}
