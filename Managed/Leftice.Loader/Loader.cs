// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Leftice
{
    internal static class Loader
    {
        private static string componentAssemblyPath = null!;
        private static AssemblyLoadContext context = null!;

        internal static void Initialize(IntPtr componentAssemblyPath) =>
            Loader.componentAssemblyPath = Marshal.PtrToStringUni(componentAssemblyPath)!;

        internal static void Start() => context = new Context(componentAssemblyPath);

        internal static void Stop()
        {
            context.Unload();
            context = null!;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        internal delegate void Initialize_Delegate(IntPtr componentAssemblyPath);

        internal delegate void Start_Delegate();

        internal delegate void Stop_Delegate();
    }
}
