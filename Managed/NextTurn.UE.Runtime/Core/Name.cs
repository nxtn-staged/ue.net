// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public readonly struct Name : IComparable, IComparable<Name>, IEquatable<Name>
    {
        public static Name None { get; }

        public Name(string? name, Mode mode = Mode.Add) : this(name, 0, mode) { }

        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is not 0 and its corresponding external number is less than 0.</exception>
        public Name(string? name, int number, Mode mode = Mode.Add)
        {
            if (string.IsNullOrEmpty(name))
            {
                this = None;
                return;
            }

            if (number < 0 && number != int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            if (IsWideName(name))
            {
                NativeMethods.InitializeWide(out this, name.Length, name, number, mode);
            }
            else
            {
                NativeMethods.InitializeAnsi(out this, name.Length, name, number, mode);
            }
        }

        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is not 0 and its corresponding external number is less than 0.</exception>
        private Name(Name name, int number)
        {
            if (number < 0 && number != int.MinValue)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }

            this.ComparisonIndex = name.ComparisonIndex;
            this.DisplayIndex = name.DisplayIndex;
            this.Number = number;
        }

        public int ComparisonIndex { get; }

        public int DisplayIndex { get; }

        public int Number { get; }

        public string PlainNameString => NameEntry.UnsafeMethods.GetPlainNameString(NativeMethods.GetDisplayNameEntry(this));

        public bool IsNone => this.ComparisonIndex == 0 && this.Number == 0;

        public bool IsValid => NativeMethods.IsValid(this);

        /// <summary>
        /// Converts the specified internal number to an external number.
        /// </summary>
        /// <param name="internalNumber">The internal number to convert.</param>
        /// <returns>The converted external number.</returns>
        /// <remarks><see cref="Number"/> is an internal number.</remarks>
        /// <seealso cref="ConvertToInternalNumber"/>
        public static int ConvertToExternalNumber(int internalNumber) => internalNumber - 1;

        /// <summary>
        /// Converts the specified external number to an internal number.
        /// </summary>
        /// <param name="externalNumber">The external number to convert.</param>
        /// <returns>The converted internal number.</returns>
        /// <remarks><see cref="Number"/> is an internal number.</remarks>
        /// <seealso cref="ConvertToExternalNumber"/>
        public static int ConvertToInternalNumber(int externalNumber) => externalNumber + 1;

        internal static bool IsWideName(string name)
        {
            foreach (char c in name)
            {
                if (c > 0x7F)
                {
                    return true;
                }
            }

            return false;
        }

        /// <exception cref="ArgumentException"><paramref name="value"/> is not a <see cref="Name"/>.</exception>
        public int CompareTo(object? value) =>
            value is null ? 1 :
            value is Name other ? this.CompareTo(other) :
            throw new ArgumentException();

        public int CompareTo(Name other) =>
            this.ComparisonIndex != other.ComparisonIndex ? NativeMethods.CompareTo(this, other) :
            this.Number != other.Number ? (this.Number > other.Number ? 1 : -1) :
            0;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int comparisonIndex, out int number)
        {
            comparisonIndex = this.ComparisonIndex;
            number = this.Number;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int comparisonIndex, out int displayIndex, out int number)
        {
            comparisonIndex = this.ComparisonIndex;
            displayIndex = this.DisplayIndex;
            number = this.Number;
        }

        public override bool Equals(object? value) => value is Name other && this.Equals(other);

        public bool Equals(Name other) => this.ComparisonIndex == other.ComparisonIndex && this.Number == other.Number;

        public override int GetHashCode() => NativeMethods.GetHashCode(this);

        /// <exception cref="InvalidOperationException"><see cref="Number"/> is not 0 and its corresponding external number is less than 0.</exception>
        public override string ToString()
        {
            if (this.Number == 0)
            {
                return this.PlainNameString;
            }

            if (this.Number < 0 && this.Number != int.MinValue)
            {
                throw new InvalidOperationException();
            }

            Span<char> numberBuffer = stackalloc char[10];
            _ = ConvertToExternalNumber(this.Number).TryFormat(numberBuffer, out int numberLength);
            string result = NameEntry.UnsafeMethods.GetPlainNameString(NativeMethods.GetDisplayNameEntry(this), numberLength + 1);
            ref char destination = ref Unsafe.Add(ref StringMarshal.GetReference(result), result.Length - numberLength - 2);
            destination = '_';
            numberBuffer.Slice(0, numberLength).CopyTo(
                MemoryMarshal.CreateSpan(ref Unsafe.Add(ref destination, 1), numberLength));
            return result;
        }

        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is not 0 and its corresponding external number is less than 0.</exception>
        public Name WithNumber(int number) => new Name(this, number);

        public static bool operator ==(Name left, Name right) => left.Equals(right);

        public static bool operator !=(Name left, Name right) => !(left == right);

        public enum Mode
        {
            Find,
            Add,
            Replace,
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        private static class NativeMethods
        {
            [Calli]
            public static extern void InitializeAnsi(out Name name, int length, string s, int number, Mode mode);

            [Calli]
            public static extern void InitializeWide(out Name name, int length, string s, int number, Mode mode);

            [Calli]
            public static extern int CompareTo(Name name, Name other);

            [Calli]
            public static extern IntPtr GetDisplayNameEntry(Name name);

            [Calli]
            public static extern int GetHashCode(Name name);

            [Calli]
            public static extern bool IsValid(Name name);
        }
    }
}
