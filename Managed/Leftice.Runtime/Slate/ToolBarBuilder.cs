// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal.Slate
{
    public readonly ref struct ToolBarBuilder
    {
        private readonly IntPtr pointer;

        public void AddSeparator(Name extensionPoint = default) =>
            NativeMethods.AddSeparator(this.pointer, extensionPoint);

        public void BeginSection(Name extensionPoint = default) =>
            NativeMethods.BeginSection(this.pointer, extensionPoint);

        public void EndSection() =>
            NativeMethods.EndSection(this.pointer);

        private static class NativeMethods
        {
            [Calli]
            public static extern void AddSeparator(IntPtr builder, Name extensionPoint);

            [Calli]
            public static extern void BeginSection(IntPtr builder, Name extensionPoint);

            [Calli]
            public static extern void EndSection(IntPtr builder);
        }
    }
}
