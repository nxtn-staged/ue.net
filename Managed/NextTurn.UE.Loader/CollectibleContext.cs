// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.Loader;

namespace NextTurn.UE
{
    internal sealed class CollectibleContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        internal CollectibleContext(string componentAssemblyPath) : base(true)
        {
            this.resolver = new AssemblyDependencyResolver(componentAssemblyPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = this.resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath is null ? null : this.LoadFromAssemblyPath(assemblyPath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string? libraryPath = this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return libraryPath is null ? IntPtr.Zero : this.LoadUnmanagedDllFromPath(libraryPath);
        }
    }
}
