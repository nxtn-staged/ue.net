// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.Loader;
using Unreal;

namespace NextTurn.UE.Programs
{
    internal static class Generator
    {
        internal static void Export(string path)
        {
            AssemblyLoadContext context = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly())!;
            Assembly assembly = context.LoadFromAssemblyPath(path);
            Export(assembly);
        }

        private static void Export(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes)
            {
                if (ShouldExport(type))
                {
                    Export(type);
                }
            }
        }

        private static bool ShouldExport(Type type)
        {
            if (type.IsDefined(typeof(ClassAttribute)))
            {
                return true;
            }

            if (type.IsDefined(typeof(StructAttribute)))
            {
                return true;
            }

            if (type.IsDefined(typeof(EnumAttribute)))
            {
                return true;
            }

            return false;
        }

        private static void ExportMethods(Type type)
        {

        }

        private static void ExportProperties(Type type)
        {

        }

        private static void Export(Type type)
        {
            ExportProperties(type);
            ExportMethods(type);
        }
    }
}
