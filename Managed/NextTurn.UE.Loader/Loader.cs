// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Diagnostics.Runtime;

namespace NextTurn.UE
{
    internal static class Loader
    {
        private static string componentAssemblyPath = null!;
        private static CollectibleContext context = null!;
        private static WeakReference contextReference = null!;

        private static Dictionary<string, (IntPtr MembersPtr, int MembersLength)> symbols = null!;

        [UnmanagedCallersOnly]
        internal static void FreeGCHandle(IntPtr handle) => GCHandle.FromIntPtr(handle).Free();

        [UnmanagedCallersOnly]
        internal static IntPtr GetFunctionPointer(
            IntPtr assemblyNamePtr,
            int assemblyNameLength,
            IntPtr typeNamePtr,
            int typeNameLength,
            IntPtr methodNamePtr,
            int methodNameLength)
        {
            string assemblyName = Marshal.PtrToStringUni(assemblyNamePtr, assemblyNameLength)!;
            string typeName = Marshal.PtrToStringUni(typeNamePtr, typeNameLength)!;
            string methodName = Marshal.PtrToStringUni(methodNamePtr, methodNameLength)!;

            Assembly assembly = context.LoadFromAssemblyName(new AssemblyName(assemblyName));
            Type type = assembly.GetType(typeName)!;
            MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)!;
            return method.MethodHandle.GetFunctionPointer();
        }

        [UnmanagedCallersOnly]
        internal static unsafe void Initialize(
            IntPtr componentAssemblyPathPtr,
            int componentAssemblyPathLength,
            TypeSymbol* typesPtr,
            int typesLength)
        {
            componentAssemblyPath = Marshal.PtrToStringUni(componentAssemblyPathPtr, componentAssemblyPathLength)!;

            symbols = new Dictionary<string, (IntPtr MembersPtr, int MembersLength)>();
            foreach (var type in new ReadOnlySpan<TypeSymbol>(typesPtr, typesLength))
            {
                symbols.Add(type.Name, (type.MembersPtr, type.MembersLength));
            }
        }

        [UnmanagedCallersOnly]
        internal static void Load()
        {
            context = new CollectibleContext(componentAssemblyPath);

            Assembly runtime = context.LoadFromAssemblyName(new AssemblyName("NextTurn.UE.Runtime"));
            Type importTable = runtime.GetType("NextTurn.UE.ImportTable")!;
            FieldInfo tables = importTable.GetField("Tables", BindingFlags.NonPublic | BindingFlags.Static)!;
            tables.SetValue(null, symbols);
        }

        [UnmanagedCallersOnly]
        internal static unsafe void RegisterSymbols(TypeSymbol* typesPtr, int typesLength)
        {
            foreach (var type in new ReadOnlySpan<TypeSymbol>(typesPtr, typesLength))
            {
                symbols.Add(type.Name, (type.MembersPtr, type.MembersLength));
            }
        }

        [UnmanagedCallersOnly]
        internal static int Unload()
        {
            if (context is not null)
            {
                contextReference = new WeakReference(context);

                context.Unload();
                context = null!;
            }

            const int MaxRetries = 100;

            int retries;
            for (retries = 0; retries < MaxRetries && contextReference.IsAlive; retries++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            if (retries == MaxRetries)
            {
                Dump();
            }

            return retries;

            static void Dump()
            {
                using var dataTarget = DataTarget.AttachToProcess(Process.GetCurrentProcess().Id, false);
                using var runtime = dataTarget.ClrVersions.Single().CreateRuntime();
                var heap = runtime.Heap;

                foreach (var path in
                    from obj in heap.EnumerateObjects()
                    where obj.Type?.Name == "System.Reflection.LoaderAllocator"
                    from path in new GCRoot(heap).EnumerateGCRoots(obj.Address, false)
                    select path)
                {
                    Console.WriteLine(path.ToString());
                }

                int currentId = Environment.CurrentManagedThreadId;
                foreach (var frame in
                    from thread in runtime.Threads
                    where thread.ManagedThreadId != currentId
                    from frame in thread.EnumerateStackTrace()
                    where frame.Kind == ClrStackFrameKind.ManagedMethod
                    select frame)
                {
                    Console.WriteLine(frame.Method?.Signature);
                }
            }
        }

        [UnmanagedCallersOnly]
        internal static unsafe void UnregisterSymbols(TypeSymbol* typesPtr, int typesLength)
        {
            foreach (var type in new ReadOnlySpan<TypeSymbol>(typesPtr, typesLength))
            {
                _ = symbols.Remove(type.Name);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct TypeSymbol
        {
            internal readonly IntPtr NamePtr;
            internal readonly int NameLength;
            internal readonly IntPtr MembersPtr;
            internal readonly int MembersLength;

            internal string Name => Marshal.PtrToStringUni(this.NamePtr, this.NameLength);
        }
    }
}
