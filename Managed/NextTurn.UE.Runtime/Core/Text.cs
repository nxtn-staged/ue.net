// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class Text : IDisposable, IComparable, IComparable<Text?>,
#nullable disable // to enable use with both T and T? for reference types due to IEquatable<T> being invariant
        IEquatable<Text>
#nullable restore
    {
        internal NativeText text;

        private bool disposed;

        internal Text() { }

        internal Text(NativeText text)
        {
            text.TextData.AddReference();

            this.text = text;
        }

        ~Text() => this.Dispose(false);

        public static Text Empty { get; } = GetEmpty();

        /// <summary>
        /// Gets a value indicating whether this <see cref="Text"/> is empty.
        /// </summary>
        /// <value><see langword="true"/> if this <see cref="Text"/> is empty; otherwise, <see langword="false"/>.</value>
        /// <seealso cref="IsNullOrEmpty"/>
        public bool IsEmpty => NativeMethods.IsEmpty(this.text);

        public bool IsNumeric => NativeMethods.IsNumeric(this.text);

        /// <summary>
        /// Gets a value indicating whether this <see cref="Text"/> is empty or consists only of white-space characters.
        /// </summary>
        /// <value><see langword="true"/> if this <see cref="Text"/> is empty or consists only of white-space characters; otherwise, <see langword="false"/>.</value>
        /// <seealso cref="IsNullOrWhiteSpace"/>
        public bool IsWhiteSpace => NativeMethods.IsWhiteSpace(this.text);

        public static Text FromName(Name value)
        {
            Text result = new Text();
            NativeMethods.InitializeFromName(out result.text, value);
            return result;
        }

        //public static Text FromString(string value)
        //{
        //    Text result = new Text();
        //    fixed (char* pointer = value)
        //    {
        //        NextTurn.UE.Runtime.ScriptArray nativeValue = new NextTurn.UE.Runtime.ScriptArray { pointer = new IntPtr(pointer), _count = value.Length + 1, _capacity = value.Length + 1 };
        //        fixed (NativeText* textPtr = &result.text)
        //        {
        //            NativeMethods.InitializeFromString(textPtr, nativeValue);
        //        }
        //    }

        //    return result;
        //}

        private static Text GetEmpty()
        {
            Text result = new Text();
            NativeMethods.GetEmpty(out result.text);
            return result;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Text"/> is <see langword="null"/> or empty.
        /// </summary>
        /// <seealso cref="IsEmpty"/>
        /// <seealso cref="IsNullOrWhiteSpace"/>
        public static bool IsNullOrEmpty([NotNullWhen(false)] Text? text) => text is null || text.IsEmpty;

        /// <summary>
        /// Indicates whether the specified <see cref="Text"/> is <see langword="null"/>, empty, or consists only of white-space characters.
        /// </summary>
        /// <seealso cref="IsWhiteSpace"/>
        /// <seealso cref="IsNullOrEmpty"/>
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] Text? text) => text is null || text.IsWhiteSpace;

        /// <exception cref="ArgumentException"><paramref name="value"/> is not a <see cref="Text"/></exception>
        public int CompareTo(object? value) =>
            value is null ? 1 :
            value is Text other ? this.CompareTo(other) :
            throw new ArgumentException();

        public int CompareTo(Text? other) => this.CompareTo(other, ComparisonLevel.Default);

        public int CompareTo(Text? other, ComparisonLevel level = ComparisonLevel.Default) =>
            other is null ? 1 :
            ReferenceEquals(this, other) ? 0 :
            NativeMethods.CompareTo(this.text, other.text, level);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.text.TextData.ReleaseReference();

                this.disposed = false;
            }
        }

        public bool Equals(Text? other) => this.Equals(other, ComparisonLevel.Default);

        public bool Equals(Text? other, ComparisonLevel level = ComparisonLevel.Default) =>
            other is null ? false :
            ReferenceEquals(this, other) ? true :
            NativeMethods.CompareTo(this.text, other.text, level) == 0;

        public char[] ToCharArray() => throw new NotImplementedException();

        /// <summary>
        /// Returns a copy of this <see cref="Text"/> converted to lowercase.
        /// </summary>
        /// <seealso cref="ToUpper"/>
        public Text ToLower()
        {
            Text result = new Text();
            NativeMethods.ToLower(this.text, out result.text);
            return result;
        }

        public override unsafe string ToString()
        {
            NativeMethods.ToString(this.text, out ScriptArray nativeResult);
            return StringMarshaler.ToManaged(&nativeResult);
        }

        /// <summary>
        /// Returns a copy of this <see cref="Text"/> converted to uppercase.
        /// </summary>
        /// <seealso cref="ToLower"/>
        public Text ToUpper()
        {
            Text result = new Text();
            NativeMethods.ToUpper(this.text, out result.text);
            return result;
        }

        /// <summary>
        /// Removes all leading and trailing white-space characters from the string.
        /// </summary>
        /// <seealso cref="TrimEnd"/>
        /// <seealso cref="TrimStart"/>
        public Text Trim()
        {
            Text result = new Text();
            NativeMethods.Trim(this.text, out result.text);
            return result;
        }

        /// <summary>
        /// Removes all trailing white-space characters from the string.
        /// </summary>
        /// <seealso cref="Trim"/>
        /// <seealso cref="TrimStart"/>
        public Text TrimEnd()
        {
            Text result = new Text();
            NativeMethods.TrimEnd(this.text, out result.text);
            return result;
        }

        /// <summary>
        /// Removes all leading white-space characters from the string.
        /// </summary>
        /// <seealso cref="Trim"/>
        /// <seealso cref="TrimEnd"/>
        public Text TrimStart()
        {
            Text result = new Text();
            NativeMethods.TrimStart(this.text, out result.text);
            return result;
        }

        public enum ComparisonLevel
        {
            Default,
            Primary,
            Secondary,
            Tertiary,
            Quaternary,
            Quinary,
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern void InitializeFromName(out NativeText text, Name value);

            [Calli]
            public static extern void InitializeFromString(out NativeText text, ScriptArray nativeValue);

            [Calli]
            public static extern int CompareTo(in NativeText text, in NativeText other, ComparisonLevel level);

            [Calli]
            public static extern void GetEmpty(out NativeText text);

            [Calli]
            public static extern bool IsEmpty(in NativeText text);

            [Calli]
            public static extern bool IsNumeric(in NativeText text);

            [Calli]
            public static extern bool IsWhiteSpace(in NativeText text);

            [Calli]
            public static extern void ToLower(in NativeText text, out NativeText result);

            [Calli]
            public static extern void ToString(in NativeText text, out ScriptArray nativeResult);

            [Calli]
            public static extern void ToUpper(in NativeText text, out NativeText result);

            [Calli]
            public static extern void Trim(in NativeText text, out NativeText result);

            [Calli]
            public static extern void TrimEnd(in NativeText text, out NativeText result);

            [Calli]
            public static extern void TrimStart(in NativeText text, out NativeText result);
        }
    }
}
