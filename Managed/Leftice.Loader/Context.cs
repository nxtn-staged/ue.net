// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System.Reflection;
using System.Runtime.Loader;

namespace Leftice
{
    internal sealed class Context : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        internal Context(string componentAssemblyPath) : base(true) =>
            this.resolver = new AssemblyDependencyResolver(componentAssemblyPath);

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = this.resolver.ResolveAssemblyToPath(assemblyName);
            return assemblyPath != null ? this.LoadFromAssemblyPath(assemblyPath) : null;
        }
    }
}
