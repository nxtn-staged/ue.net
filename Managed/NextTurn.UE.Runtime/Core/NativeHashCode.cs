// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class NativeHashCode
    {
        public static int Combine(int hashCode1, int hashCode2) => NativeMethods.Combine(hashCode1, hashCode2);

        public static int GetHashCode(byte value) => value;

        public static int GetHashCode(double value) => GetHashCode(Unsafe.As<double, long>(ref value));

        public static int GetHashCode(float value) => Unsafe.As<float, int>(ref value);

        public static int GetHashCode(Guid value) => NativeMethods.ForGuid(value);

        public static int GetHashCode(int value) => value;

        public static unsafe int GetHashCode(IntPtr value) => NativeMethods.ForPointer(value.ToPointer());

        public static int GetHashCode(long value) => (int)value + ((int)(value >> 32) * 23);

        public static int GetHashCode(Name value) => value.GetHashCode();

        public static unsafe int GetHashCode(ReadOnlySpan<char> value)
        {
            fixed (char* valuePtr = value)
            {
                return NativeMethods.ForCharArray(valuePtr);
            }
        }

        [CLSCompliant(false)]
        public static int GetHashCode(sbyte value) => value;

        public static int GetHashCode(short value) => value;

        [CLSCompliant(false)]
        public static int GetHashCode(uint value) => (int)value;

        [CLSCompliant(false)]
        public static unsafe int GetHashCode(UIntPtr value) => NativeMethods.ForPointer(value.ToPointer());

        [CLSCompliant(false)]
        public static int GetHashCode(ulong value) => (int)value + ((int)(value >> 32) * 23);

        [CLSCompliant(false)]
        public static int GetHashCode(ushort value) => value;

        public static int GetHashCode<T>(T value)
            where T : INativeHashable =>
            value.GetNativeHashCode();

        private static class NativeMethods
        {
            [Calli]
            public static extern int Combine(int hashCode1, int hashCode2);

            [Calli]
            public static extern unsafe int ForCharArray(char* value);

            [Calli]
            public static extern int ForGuid(in Guid value);

            [Calli]
            public static extern unsafe int ForPointer(void* value);
        }
    }
}
