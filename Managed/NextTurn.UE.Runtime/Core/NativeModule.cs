// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class NativeModule : IModule
    {
        internal IntPtr Pointer;

        internal NativeModule() { }

        public bool SupportsAutomaticShutdown => NativeMethods.SupportsAutomaticShutdown(this.Pointer);

        public bool SupportsDynamicReloading => NativeMethods.SupportsDynamicReloading(this.Pointer);

        public void OnPostLoad() => NativeMethods.OnPostLoad(this.Pointer);

        public void OnPreUnload() => NativeMethods.OnPreUnload(this.Pointer);

        public void ShutDown() => NativeMethods.ShutDown(this.Pointer);

        public void StartUp() => NativeMethods.StartUp(this.Pointer);

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
            public static extern bool SupportsAutomaticShutdown(IntPtr module);

            [Calli]
            public static extern bool SupportsDynamicReloading(IntPtr module);
        }
    }
}
