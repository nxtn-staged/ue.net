// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Runtime.CompilerServices;
using Leftice.Processors;
using Unreal;

namespace Leftice
{
    internal static class BooleanMarshaler
    {
        internal static unsafe bool ToManaged(IntPtr source, int arrayIndex) =>
            TrivialMarshaler<NativeBoolean>.ToManaged(source, arrayIndex);

        internal static unsafe void ToNative(IntPtr destination, int arrayIndex, bool value) =>
            TrivialMarshaler<NativeBoolean>.ToNative(destination, arrayIndex, value);
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
                return new string((char*)source->Items, 0, source->Count);
            }

            string result = new string('\0', source->Count);
            ref char first = ref StringMarshal.GetReference(result);
            for (int i = 0; i < source->Count; i++)
            {
                Unsafe.Add(ref first, i) = (char)source->GetItem<int>(i);
            }

            return result;
        }

        internal static unsafe string ToManagedFinally(ScriptArray* source)
        {
            string result = ToManaged(source);
            NativeMethods.Finalize(source);
            return result;
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern unsafe void Finalize(ScriptArray* array);
        }
    }

    internal static class TextMarshaler
    {
        internal static unsafe Text ToManaged(IntPtr source, int arrayIndex)
        {
            return new Text(TrivialMarshaler<Text.NativeText>.ToManaged(source, arrayIndex));
        }

        internal static void ToNative(IntPtr destination, int arrayIndex, Text value)
        {
            value.text.TextData.AddReference();
            TrivialMarshaler<Text.NativeText>.ToNative(destination, arrayIndex, value.text);
        }
    }

    internal static class TrivialMarshaler<T> where T : unmanaged
    {
        internal static unsafe T ToManaged(IntPtr source, int arrayIndex) =>
            ((T*)source)[arrayIndex];

        internal static unsafe void ToNative(IntPtr destination, int arrayIndex, T value) =>
            ((T*)destination)[arrayIndex] = value;
    }
}
