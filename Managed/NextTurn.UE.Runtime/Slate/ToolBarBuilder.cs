// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

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
