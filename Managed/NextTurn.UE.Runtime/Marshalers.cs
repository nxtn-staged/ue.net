// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Runtime.CompilerServices;
using Unreal;
using Object = Unreal.Object;

namespace NextTurn.UE
{
    /*
    internal static class Marshaler
    {
        internal static object? ToManaged(Type type, IntPtr source, int arrayIndex) =>
            throw new NotImplementedException();

        internal static void ToNative(Type type, IntPtr destination, int arrayIndex, object? value) =>
            throw new NotImplementedException();
    }
    */

    internal static class Marshaler<T>
    {
        private static readonly unsafe delegate* managed<IntPtr, int, T> ToManagedMethod;
        private static readonly unsafe delegate* managed<IntPtr, int, T, void> ToNativeMethod;

        static unsafe Marshaler()
        {
            if (IsNumeric)
            {
                Type marshaler = typeof(TrivialMarshaler<>).MakeGenericType(typeof(T));
                ToManagedMethod = (delegate* managed<IntPtr, int, T>)marshaler.GetMethod(nameof(ToManaged))!.MethodHandle.GetFunctionPointer();
                ToNativeMethod = (delegate* managed<IntPtr, int, T, void>)marshaler.GetMethod(nameof(ToNative))!.MethodHandle.GetFunctionPointer();
            }
        }

        private static bool IsNumeric =>
            typeof(T) == typeof(byte) ||
            typeof(T) == typeof(sbyte) ||
            typeof(T) == typeof(short) ||
            typeof(T) == typeof(ushort) ||
            typeof(T) == typeof(int) ||
            typeof(T) == typeof(uint) ||
            typeof(T) == typeof(long) ||
            typeof(T) == typeof(ulong) ||
            typeof(T) == typeof(float) ||
            typeof(T) == typeof(double);

        internal static bool IsRequired => !IsNumeric;

        internal static bool IsSupported =>
            (default(T) is not null && IsNumeric) ||
            (default(T) is null && typeof(Object).IsAssignableFrom(typeof(T)));

        internal static unsafe T ToManaged(IntPtr source, int arrayIndex) =>
            ToManagedMethod(source, arrayIndex);

        internal static unsafe void ToNative(IntPtr destination, int arrayIndex, T value) =>
            ToNativeMethod(destination, arrayIndex, value);
    }

    internal static class BooleanMarshaler
    {
        internal static unsafe bool ToManaged(IntPtr source, int arrayIndex) =>
            TrivialMarshaler<bool>.ToManaged(source, arrayIndex);

        internal static unsafe void ToNative(IntPtr destination, int arrayIndex, bool value) =>
            TrivialMarshaler<bool>.ToNative(destination, arrayIndex, value);
    }

    internal static class CharArrayMarshaler
    {
        private static readonly unsafe int NativeCharSize = NativeChar.Size;

        internal static unsafe string? ToManaged(IntPtr source)
        {
            void* sourcePtr = source.ToPointer();
            if (sourcePtr is null)
            {
                return null;
            }

            switch (NativeCharSize)
            {
                case sizeof(char):
                    {
                        int length = new ReadOnlySpan<char>(sourcePtr, int.MaxValue).IndexOf('\0');
                        return new string(new ReadOnlySpan<char>(sourcePtr, length));
                    }

                case sizeof(uint):
                    {
                        int length = new ReadOnlySpan<uint>(sourcePtr, int.MaxValue).IndexOf('\0');
                        string result = new string('\0', length);
                        ref char first = ref StringMarshal.GetReference(result);
                        for (int i = 0; i < length; i++)
                        {
                            Unsafe.Add(ref first, i) = (char)((uint*)sourcePtr)[i];
                        }

                        return result;
                    }

                default:
                    throw new NotSupportedException();
            }
        }

        internal static unsafe void ToNative(IntPtr destination, string? value)
        {

        }
    }

    internal static class ObjectMarshaler
    {
        internal static unsafe Object? ToManaged(IntPtr source)
        {
            return Object.CreateOrNull<Object>(TrivialMarshaler<IntPtr>.ToManaged(source, 0));
        }

        internal static unsafe void ToNative(IntPtr destination, Object? value)
        {
            TrivialMarshaler<IntPtr>.ToNative(destination, 0, Object.GetPointerOrZero(value));
        }
    }

    internal static class StringMarshaler
    {
        private static readonly unsafe int NativeCharSize = NativeChar.Size;

        internal static unsafe string ToManaged(ScriptArray* source)
        {
            if (source->Count == 0)
            {
                return string.Empty;
            }

            if (NativeCharSize == sizeof(char))
            {
                return new ReadOnlySpan<char>(source->Items, source->Count).TrimEnd('\0').ToString();
            }

            ReadOnlySpan<uint> sourceSpan = new ReadOnlySpan<uint>(source->Items, source->Count).TrimEnd('\0');
            string result = new string('\0', sourceSpan.Length);
            ref char first = ref StringMarshal.GetReference(result);
            for (int i = 0; i < sourceSpan.Length; i++)
            {
                Unsafe.Add(ref first, i) = (char)sourceSpan[i];
            }

            return result;
        }

        internal static unsafe string ToManagedFinally(ScriptArray* source)
        {
            string result = ToManaged(source);
            ScriptArray.NativeMethods.Finalize(source);
            return result;
        }

        internal static unsafe void ToNative(ScriptArray* destination, string value)
        {

        }
    }

    internal static class TextMarshaler
    {
        internal static unsafe Text ToManaged(IntPtr source, int arrayIndex)
        {
            return new Text(TrivialMarshaler<NativeText>.ToManaged(source, arrayIndex));
        }

        internal static void ToNative(IntPtr destination, int arrayIndex, Text value)
        {
            value.text.TextData.AddReference();
            TrivialMarshaler<NativeText>.ToNative(destination, arrayIndex, value.text);
        }
    }

    internal static class TrivialMarshaler<T>
        where T : unmanaged
    {
        internal static unsafe T ToManaged(IntPtr source, int arrayIndex) =>
            ((T*)source)[arrayIndex];

        internal static unsafe void ToNative(IntPtr destination, int arrayIndex, T value) =>
            ((T*)destination)[arrayIndex] = value;
    }
}
