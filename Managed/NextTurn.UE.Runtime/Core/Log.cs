// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class Log
    {
        /// <summary>
        /// Writes an informational message to the console and log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void Display(string message) => NativeMethods.Display(message);

        /// <summary>
        /// Writes an error message to the console and log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void Error(string message) => NativeMethods.Error(message);

        /// <summary>
        /// Writes a fatal message to the console and log file and crashes.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        [DoesNotReturn]
        public static void Fatal(string message) => NativeMethods.Fatal(message);

        /// <summary>
        /// Writes an informational message to the log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void Information(string message) => NativeMethods.Information(message);

        /// <summary>
        /// Writes a verbose message to the log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void Verbose(string message) => NativeMethods.Verbose(message);

        /// <summary>
        /// Writes a very verbose message to the log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void VeryVerbose(string message) => NativeMethods.VeryVerbose(message);

        /// <summary>
        /// Writes a warning message to the console and log file.
        /// </summary>
        /// <param name="message">
        /// The message to write.
        /// </param>
        public static void Warning(string message) => NativeMethods.Warning(message);

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        private static class NativeMethods
        {
            [Calli]
            public static extern void Display(string message);

            [Calli]
            public static extern void Error(string message);

            [Calli]
            public static extern void Fatal(string message);

            [Calli]
            public static extern void Information(string message);

            [Calli]
            public static extern void Verbose(string message);

            [Calli]
            public static extern void VeryVerbose(string message);

            [Calli]
            public static extern void Warning(string message);
        }
    }
}
