// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Leftice;
using Leftice.Processors;

namespace Unreal.Editor
{
    public static class MemberExtensions
    {
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static Text GetDisplayNameText(this Member member)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            Text result = new Text();
            NativeMethods.GetDisplayNameText(member.pointer, out result.text);
            return result;
        }

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static Text GetToolTipText(this Member member, bool shortToolTip = false)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            Text result = new Text();
            NativeMethods.GetToolTipText(member.pointer, shortToolTip, out result.text);

            return result;
        }

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public static string GetMetaData(this Member member, string? key) =>
            TryGetMetaData(member, key, out string? value) ? value :
            throw new KeyNotFoundException();

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public static string GetMetaData(this Member member, Name key) =>
            TryGetMetaData(member, key, out string? value) ? value :
            throw new KeyNotFoundException();

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static bool HasMetaData(this Member member, string? key) =>
            member is null ? throw new ArgumentNullException(nameof(member)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.HasMetaData(member.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static bool HasMetaData(this Member member, Name key) =>
            member is null ? throw new ArgumentNullException(nameof(member)) :
            !key.IsNone &&
            NativeMethods.HasMetaDataWithName(member.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static bool RemoveMetaData(this Member member, string? key) =>
            member is null ? throw new ArgumentNullException(nameof(member)) :
            !string.IsNullOrEmpty(key) &&
            NativeMethods.RemoveMetaData(member.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static bool RemoveMetaData(this Member member, Name key) =>
            member is null ? throw new ArgumentNullException(nameof(member)) :
            !key.IsNone &&
            NativeMethods.RemoveMetaDataWithName(member.pointer, key);

        /// <exception cref="ArgumentNullException"><paramref name="member"/> or <paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is a zero-length string.</exception>
        public static void SetMetaData(this Member member, string key, string value)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key.Length == 0)
            {
                throw new ArgumentException();
            }

            NativeMethods.SetMetaData(member.pointer, key, value);
        }

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> is <see cref="Name.None"/>.</exception>
        public static void SetMetaData(this Member member, Name key, string value)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (key.IsNone)
            {
                throw new ArgumentException();
            }

            NativeMethods.SetMetaDataWithName(member.pointer, key, value);
        }

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static unsafe bool TryGetMetaData(this Member member, string? key, [NotNullWhen(true)] out string? value)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (!string.IsNullOrEmpty(key) &&
                NativeMethods.TryGetMetaData(member.pointer, key, out ScriptArray nativeValue))
            {
                value = StringMarshaler.ToManaged(&nativeValue);
                return true;
            }

            value = default;
            return false;
        }

        /// <exception cref="ArgumentNullException"><paramref name="member"/> is <see langword="null"/>.</exception>
        public static unsafe bool TryGetMetaData(this Member member, in Name key, [NotNullWhen(true)] out string? value)
        {
            if (member is null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (!key.IsNone &&
                NativeMethods.TryGetMetaDataWithName(member.pointer, key, out ScriptArray nativeValue))
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
            public static extern void GetDisplayNameText(IntPtr member, out NativeText text);

            [Calli]
            public static extern void GetToolTipText(IntPtr member, bool shortToolTip, out NativeText text);

            [Calli]
            public static extern bool HasMetaData(IntPtr member, string key);

            [Calli]
            public static extern bool HasMetaDataWithName(IntPtr member, Name key);

            [Calli]
            public static extern bool RemoveMetaData(IntPtr member, string key);

            [Calli]
            public static extern bool RemoveMetaDataWithName(IntPtr member, Name key);

            [Calli]
            public static extern void SetMetaData(IntPtr member, string key, string value);

            [Calli]
            public static extern void SetMetaDataWithName(IntPtr member, Name key, string value);

            [Calli]
            public static extern bool TryGetMetaData(IntPtr member, string key, out ScriptArray nativeValue);

            [Calli]
            public static extern bool TryGetMetaDataWithName(IntPtr member, Name key, out ScriptArray nativeValue);
        }
    }
}
