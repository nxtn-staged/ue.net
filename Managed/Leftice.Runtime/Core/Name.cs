// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    public readonly struct Name : IComparable, IComparable<Name>, IEquatable<Name>
    {
        public static readonly Name None;

        public Name(string? name, Mode mode = Mode.Add, bool splitName = true) : this(name, 0, mode, splitName) { }

        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is not 0 and its corresponding external number is less than 0.</exception>
        public unsafe Name(string? name, int number = 0, Mode mode = Mode.Add, bool splitName = true)
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

            fixed (Name* thisPtr = &this)
            {
                if (number == 0 && splitName && TrySplitName(name, out string? newName, out int newNumber))
                {
                    NativeMethods.Initialize(thisPtr, newName!, newNumber, mode);
                }
                else
                {
                    NativeMethods.Initialize(thisPtr, name, number, mode);
                }
            }

            static bool TrySplitName(string name, out string? newName, out int newNumber)
            {
                ref char first = ref Leftice.StringMarshal.GetReference(name);
                ref char last = ref Unsafe.Add(ref first, name.Length - 1);
                ref char current = ref last;
                if (IsDigit(current) && Unsafe.IsAddressGreaterThan(ref current, ref first))
                {
                    current = ref Unsafe.Subtract(ref current, 1);
                    while (IsDigit(current) && Unsafe.IsAddressGreaterThan(ref current, ref first))
                    {
                        current = ref Unsafe.Subtract(ref current, 1);
                    }

                    if (current == '_')
                    {
                        ref char next = ref Unsafe.Add(ref current, 1);
                        if ((next != '0' || Unsafe.AreSame(ref next, ref last)) && int.TryParse(name, out newNumber))
                        {
                            newName = name.Substring(0, (int)Unsafe.ByteOffset(ref first, ref current) / sizeof(char));
                            return true;
                        }
                    }
                }

                newName = default;
                newNumber = default;
                return false;

                static bool IsDigit(char c) => (uint)(c - '0') <= '9' - '0';
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

        public override int GetHashCode() => this.ComparisonIndex + this.Number;

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
            ref char destination = ref Unsafe.Add(ref Leftice.StringMarshal.GetReference(result), result.Length - numberLength - 2);
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
            Replace
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Initialize(Name* name, string s, int number, Mode mode);

            [Calli]
            public static extern int CompareTo(Name name, Name other);

            [Calli]
            public static extern IntPtr GetDisplayNameEntry(Name name);

            [Calli]
            public static extern NativeBoolean IsValid(Name name);
        }
    }
}
