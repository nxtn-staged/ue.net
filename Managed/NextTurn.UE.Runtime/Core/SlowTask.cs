// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public class SlowTask : IDisposable
    {
        private readonly IntPtr pointer;

        private bool disposed = false;

        public SlowTask(float totalAmount, Text? defaultMessage = null)
        {
            defaultMessage ??= Text.Empty;
            this.pointer = NativeMethods.New(totalAmount, defaultMessage.text);
        }

        ~SlowTask() => this.Dispose(false);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                NativeMethods.Delete(this.pointer);

                this.disposed = true;
            }
        }

        private static class NativeMethods
        {
            [ReadOffset]
            public static extern float GetCompletedAmount(IntPtr task);

            [ReadOffset]
            public static extern NativeText GetDefaultMessage(IntPtr task);

            [ReadOffset]
            public static extern NativeText GetFrameMessage(IntPtr task);

            [ReadOffset]
            public static extern float GetTotalAmount(IntPtr task);

            [Calli]
            public static extern IntPtr New(float totalAmount, in NativeText defaultMessage);

            [Calli]
            public static extern void Delete(IntPtr task);
        }
    }
}
