// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public sealed class ConsoleVariable : ConsoleObject
    {
        internal ConsoleVariable(IntPtr pointer) : base(pointer) { }

        public int ValueAsInt32
        {
            get => NativeMethods.GetInt32(this.Pointer);
            set => NativeMethods.SetInt32(this.Pointer, value);
        }

        public float ValueAsSingle
        {
            get => NativeMethods.GetSingle(this.Pointer);
            set => NativeMethods.SetSingle(this.Pointer, value);
        }

        public unsafe string ValueAsString
        {
            get
            {
                NativeMethods.GetString(this.Pointer, out ScriptArray nativeResult);
                return Leftice.StringMarshaler.ToManagedFinally(&nativeResult);
            }

            set => NativeMethods.SetString(this.Pointer, value);
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern int GetInt32(IntPtr variable);

            [Calli]
            public static extern float GetSingle(IntPtr variable);

            [Calli]
            public static extern void GetString(IntPtr variable, out ScriptArray nativeResult);

            [Calli]
            public static extern void SetInt32(IntPtr variable, int value);

            [Calli]
            public static extern void SetSingle(IntPtr variable, float value);

            [Calli]
            public static extern void SetString(IntPtr variable, string value);
        }
    }
}
