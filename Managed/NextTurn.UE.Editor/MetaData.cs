// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public sealed class MetaData : Object
    {
        internal MetaData(IntPtr pointer) : base(pointer) { }

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public string GetValue(Object @object, string? key) =>
            this.TryGetValue(@object, key, out string? value) ? value :
            throw new KeyNotFoundException();

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public string GetValue(Object @object, Name key) =>
            this.TryGetValue(@object, key, out string? value) ? value :
            throw new KeyNotFoundException();

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public bool HasValue(Object @object, string? key) =>
            @object is null ? throw new ArgumentNullException(nameof(@object)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.HasValue(@object.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public bool HasValue(Object @object, Name key) =>
            @object is null ? throw new ArgumentNullException(nameof(@object)) :
            !key.IsNone &&
            NativeMethods.HasValueWithName(@object.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public bool RemoveValue(Object @object, string? key) =>
            @object is null ? throw new ArgumentNullException(nameof(@object)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.RemoveValue(@object.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public bool RemoveValue(Object @object, Name key) =>
            @object is null ? throw new ArgumentNullException(nameof(@object)) :
            !key.IsNone &&
            NativeMethods.RemoveValueWithName(@object.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="object"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is a zero-length string.</exception>
        public void SetValue(Object @object, string key, string value)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length == 0)
            {
                throw new ArgumentException();
            }

            NativeMethods.SetValue(@object.pointer, key, value);
        }

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is <see cref="Name.None"/>.</exception>
        public void SetValue(Object @object, Name key, string value)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (key.IsNone)
            {
                throw new ArgumentException();
            }

            NativeMethods.SetValueWithName(@object.pointer, key, value);
        }

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public unsafe bool TryGetValue(Object @object, string? key, [NotNullWhen(true)] out string? value)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (!string.IsNullOrEmpty(key) &&
                NativeMethods.TryGetValue(@object.pointer, key, out ScriptArray nativeValue))
            {
                value = StringMarshaler.ToManaged(&nativeValue);
                return true;
            }

            value = default;
            return false;
        }

        /// <exception cref="ArgumentNullException"><paramref name="object"/> is <see langword="null"/>.</exception>
        public unsafe bool TryGetValue(Object @object, Name key, [NotNullWhen(true)] out string? value)
        {
            if (@object is null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            if (!key.IsNone &&
                NativeMethods.TryGetValueWithName(@object.pointer, key, out ScriptArray nativeValue))
            {
                value = StringMarshaler.ToManaged(&nativeValue);
                return true;
            }

            value = default;
            return false;
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        private static new class NativeMethods
        {
            [Calli]
            public static extern bool HasValue(IntPtr @object, string key);

            [Calli]
            public static extern bool HasValueWithName(IntPtr @object, Name key);

            [Calli]
            public static extern bool RemoveValue(IntPtr @object, string key);

            [Calli]
            public static extern bool RemoveValueWithName(IntPtr @object, Name key);

            [Calli]
            public static extern void SetValue(IntPtr @object, string key, string value);

            [Calli]
            public static extern void SetValueWithName(IntPtr @object, Name key, string value);

            [Calli]
            public static extern bool TryGetValue(IntPtr @object, string key, out ScriptArray nativeValue);

            [Calli]
            public static extern bool TryGetValueWithName(IntPtr @object, Name key, out ScriptArray nativeValue);
        }
    }
}
