// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class ModuleManager
    {
        internal static readonly Dictionary<Type, Name> KnownModules = new Dictionary<Type, Name>();

        public static T GetModule<T>()
            where T : NativeModule
        {
            IntPtr pointer = NativeMethods.GetModule(KnownModules[typeof(T)]);
            if (pointer == IntPtr.Zero)
            {
                throw new NotImplementedException();
            }

            T result = Unsafe.As<T>(Activator.CreateInstance(typeof(T), nonPublic: true));
            result.Pointer = pointer;
            return result;
        }

        public static bool IsModuleLoaded<T>()
            where T : NativeModule =>
            IsModuleLoaded(KnownModules[typeof(T)]);

        public static bool IsModuleLoaded(Name name) => NativeMethods.IsModuleLoaded(name);

        public static T LoadModule<T>()
            where T : NativeModule
        {
            IntPtr pointer = NativeMethods.LoadModule(KnownModules[typeof(T)]);
            if (pointer == IntPtr.Zero)
            {
                throw new NotImplementedException();
            }

            T result = Unsafe.As<T>(Activator.CreateInstance(typeof(T), nonPublic: true));
            result.Pointer = pointer;
            return result;
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr GetModule(Name name);

            [Calli]
            public static extern bool IsModuleLoaded(Name name);

            [Calli]
            public static extern IntPtr LoadModule(Name name);
        }
    }
}
