// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NextTurn.UE
{
    internal readonly ref struct ImportTable
    {
        // initialized at NextTurn.UE.Context..ctor
        internal static Dictionary<string, (IntPtr MembersPtr, int MembersLength)> Tables = null!;

        private readonly ReadOnlySpan<IntPtr> symbols;

        private unsafe ImportTable(IntPtr membersPtr, int membersLength) =>
            this.symbols = new ReadOnlySpan<IntPtr>(membersPtr.ToPointer(), membersLength);

        internal static ImportTable Get(string typeName, int count)
        {
            (IntPtr membersPtr, int membersLength) = Tables[typeName];
            Debug.Assert(membersLength == count);
            return new ImportTable(membersPtr, membersLength);
        }

        internal IntPtr GetField(int index) => this.GetPointer(index);

        internal IntPtr GetMethod(int index) => this.GetPointer(index);

        internal int GetOffset(int index) => this.GetPointer(index).ToInt32();

        internal IntPtr GetPointer(int index) => this.symbols[index];
    }
}
