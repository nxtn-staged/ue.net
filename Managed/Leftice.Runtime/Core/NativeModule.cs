// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    internal sealed class NativeModule : IModule
    {
        private readonly IntPtr pointer;

        public NativeModule(IntPtr pointer) => this.pointer = pointer;

        public bool SupportsAutomaticShutdown => NativeMethods.SupportsAutomaticShutdown(this.pointer);

        public bool SupportsDynamicReloading => NativeMethods.SupportsDynamicReloading(this.pointer);

        public void OnPostLoad() => NativeMethods.OnPostLoad(this.pointer);

        public void OnPreUnload() => NativeMethods.OnPreUnload(this.pointer);

        public void ShutDown() => NativeMethods.ShutDown(this.pointer);

        public void StartUp() => NativeMethods.StartUp(this.pointer);

        private static class NativeMethods
        {
            [Calli]
            public static extern void OnPostLoad(IntPtr module);

            [Calli]
            public static extern void OnPreUnload(IntPtr module);

            [Calli]
            public static extern void ShutDown(IntPtr module);

            [Calli]
            public static extern void StartUp(IntPtr module);

            [Calli]
            public static extern NativeBoolean SupportsAutomaticShutdown(IntPtr module);

            [Calli]
            public static extern NativeBoolean SupportsDynamicReloading(IntPtr module);
        }
    }
}
