// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using NextTurn.UE.Annotations;
using Unreal.Slate;

namespace Unreal.Editor
{
    public sealed class ExtensibilityManager
    {
        internal SharedReference Reference;

        internal ExtensibilityManager() { }

        public void AddExtender(Extender extender) => NativeMethods.AddExtender(this.Reference, extender.Reference);

        public void RemoveExtender(Extender extender) => NativeMethods.RemoveExtender(this.Reference, extender.Reference);

        internal static class NativeMethods
        {
            [Calli]
            public static extern void AddExtender(in SharedReference manager, in SharedReference extender);

            [Calli]
            public static extern void RemoveExtender(in SharedReference manager, in SharedReference extender);
        }
    }
}
