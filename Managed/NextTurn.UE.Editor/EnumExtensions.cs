// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal.Editor
{
    public static class EnumExtensions
    {
        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        public static Text GetDisplayNameText(this Enum @enum, int index)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            Text result = new Text();
            NativeMethods.GetDisplayNameTextByIndex(@enum.pointer, index, out result.text);
            return result;
        }

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        public static Text GetToolTipText(this Enum @enum, int index)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            Text result = new Text();
            NativeMethods.GetToolTipTextByIndex(@enum.pointer, index, out result.text);
            return result;
        }

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public static string GetMetaData(this Enum @enum, string? key, int index) =>
            TryGetMetaData(@enum, key, index, out string? value) ? value :
            throw new KeyNotFoundException();

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        public static bool HasMetaData(this Enum @enum, string? key, int index) =>
            @enum is null ? throw new ArgumentNullException(nameof(@enum)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.HasMetaData(@enum.pointer, key, index);

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        public static bool RemoveMetaData(this Enum @enum, string? key, int index) =>
            @enum is null ? throw new ArgumentNullException(nameof(@enum)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.RemoveMetaData(@enum.pointer, key, index);

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is a zero length string.</exception>
        public static void SetMetaData(this Enum @enum, string key, string value, int index)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length == 0)
            {
                throw new ArgumentException();
            }

            NativeMethods.SetMetaData(@enum.pointer, key, value, index);
        }

        /// <exception cref="ArgumentNullException"><paramref name="enum"/> is <see langword="null"/>.</exception>
        public static unsafe bool TryGetMetaData(this Enum @enum, string? key, int index, [NotNullWhen(true)] out string? value)
        {
            if (@enum is null)
            {
                throw new ArgumentNullException(nameof(@enum));
            }

            if (!string.IsNullOrEmpty(key) &&
                NativeMethods.TryGetMetaData(@enum.pointer, key, index, out ScriptArray nativeValue))
            {
                value = StringMarshaler.ToManaged(&nativeValue);
                return true;
            }

            value = default;
            return false;
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        private static class NativeMethods
        {
            [Calli]
            public static extern void GetDisplayNameTextByIndex(IntPtr @enum, int index, out NativeText text);

            [Calli]
            public static extern void GetToolTipTextByIndex(IntPtr @enum, int index, out NativeText text);

            [Calli]
            public static extern bool HasMetaData(IntPtr @enum, string key, int index);

            [Calli]
            public static extern bool RemoveMetaData(IntPtr @enum, string key, int index);

            [Calli]
            public static extern void SetMetaData(IntPtr @enum, string key, string value, int index);

            [Calli]
            public static extern bool TryGetMetaData(IntPtr @enum, string key, int index, out ScriptArray nativeValue);
        }
    }
}
