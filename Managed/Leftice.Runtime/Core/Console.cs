// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using Leftice.Processors;

namespace Unreal
{
    public static class Console
    {
        public static ConsoleCommand RegisterCommand(string name, string helpMessage, Action command)
        {
            if (name is null)
            {
                Throw.NameArgumentNullException();
            }

            if (helpMessage is null)
            {
                Throw.HelpMessageArgumentNullException();
            }

            return new ConsoleCommand(NativeMethods.RegisterCommand(
                name, helpMessage, Marshal.GetFunctionPointerForDelegate(command), GCHandle.ToIntPtr(GCHandle.Alloc(command))));
        }

        public static ConsoleVariable RegisterVariable(string name, string helpMessage, float defaultValue)
        {
            if (name is null)
            {
                Throw.NameArgumentNullException();
            }

            if (helpMessage is null)
            {
                Throw.HelpMessageArgumentNullException();
            }

            return new ConsoleVariable(NativeMethods.RegisterSingleVariable(name, helpMessage, defaultValue));
        }

        public static ConsoleVariable RegisterVariable(string name, string helpMessage, int defaultValue)
        {
            if (name is null)
            {
                Throw.NameArgumentNullException();
            }

            if (helpMessage is null)
            {
                Throw.HelpMessageArgumentNullException();
            }

            return new ConsoleVariable(NativeMethods.RegisterInt32Variable(name, helpMessage, defaultValue));
        }

        public static ConsoleVariable RegisterVariable(string name, string helpMessage, ReadOnlySpan<char> defaultValue)
        {
            if (name is null)
            {
                Throw.NameArgumentNullException();
            }

            if (helpMessage is null)
            {
                Throw.HelpMessageArgumentNullException();
            }

            return new ConsoleVariable(NativeMethods.RegisterStringVariable(name, helpMessage, MemoryMarshal.GetReference(defaultValue), defaultValue.Length));
        }

        public static void Unregister(ConsoleObject? @object) => NativeMethods.Unregister(@object?.Pointer ?? IntPtr.Zero);

        public static void Unregister(string name) => NativeMethods.UnregisterWithName(name);

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr RegisterCommand(string name, string helpMessage, IntPtr command, IntPtr commandHandle);

            [Calli]
            public static extern IntPtr RegisterInt32Variable(string name, string helpMessage, int defaultValue);

            [Calli]
            public static extern IntPtr RegisterSingleVariable(string name, string helpMessage, float defaultValue);

            [Calli]
            public static extern IntPtr RegisterStringVariable(string name, string helpMessage, in char defaultValue, int defaultValueLength);

            [Calli]
            public static extern void Unregister(IntPtr @object);

            [Calli]
            public static extern void UnregisterWithName(string name);
        }
    }
}
