// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public static class App
    {
        public static string BranchName =>
            CharArrayMarshaler.ToManaged(NativeMethods.BranchName_Field);

        public static string BuildDateString =>
            CharArrayMarshaler.ToManaged(NativeMethods.BuildDateString_Field);

        public static string BuildVersionString =>
            CharArrayMarshaler.ToManaged(NativeMethods.BuildVersionString_Field);

        public static unsafe double CurrentTime
        {
            get => *(double*)NativeMethods.CurrentTime_Field;
            set => *(double*)NativeMethods.CurrentTime_Field = value;
        }

        public static unsafe double DeltaTime
        {
            get => *(double*)NativeMethods.DeltaTime_Field;
            set => *(double*)NativeMethods.DeltaTime_Field = value;
        }

        public static unsafe double FixedDeltaTime
        {
            get => *(double*)NativeMethods.FixedDeltaTime_Field;
            set => *(double*)NativeMethods.FixedDeltaTime_Field = value;
        }

        public static unsafe double IdleTime
        {
            get => *(double*)NativeMethods.IdleTime_Field;
            set => *(double*)NativeMethods.IdleTime_Field = value;
        }

        public static unsafe string Name
        {
            get
            {
                ScriptArray nativeResult;
                NativeMethods.GetName(&nativeResult);
                return StringMarshaler.ToManagedFinally(&nativeResult);
            }
        }

        public static string ProjectName
        {
            get => CharArrayMarshaler.ToManaged(NativeMethods.ProjectName_Field);

            set
            {
                if (value.Length + 1 >= NativeMethods.ProjectName_Capacity)
                {
                    throw new ArgumentException(nameof(value));
                }

                CharArrayMarshaler.ToNative(NativeMethods.ProjectName_Field, value);
            }
        }

        private static class NativeMethods
        {
            public static readonly int ProjectName_Capacity;

            public static readonly IntPtr BranchName_Field;
            public static readonly IntPtr BuildDateString_Field;
            public static readonly IntPtr BuildVersionString_Field;
            public static readonly IntPtr CurrentTime_Field;
            public static readonly IntPtr DeltaTime_Field;
            public static readonly IntPtr FixedDeltaTime_Field;
            public static readonly IntPtr IdleTime_Field;
            public static readonly IntPtr ProjectName_Field;

            [Calli]
            public static extern unsafe void GetName(ScriptArray* nativeResult);
        }
    }
}
