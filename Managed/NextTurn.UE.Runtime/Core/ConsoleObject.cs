// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE;
using NextTurn.UE.Annotations;

namespace Unreal
{
    public abstract class ConsoleObject : IDisposable
    {
        private readonly IntPtr pointer;

        private bool disposed;

        internal ConsoleObject(IntPtr pointer) => this.pointer = pointer;

        ~ConsoleObject() => this.DisposeImpl();

        public string HelpMessage
        {
            get => CharArrayMarshaler.ToManaged(NativeMethods.GetHelpMessage(this.pointer))!;

            set
            {
                if (value is null)
                {
                    Throw.HelpMessageArgumentNullException();
                }

                NativeMethods.SetHelpMessage(this.pointer, value);
            }
        }

        internal IntPtr Pointer => this.pointer;

        public void Dispose()
        {
            this.DisposeImpl();
            GC.SuppressFinalize(this);
        }

        private void DisposeImpl()
        {
            if (!this.disposed)
            {
                Console.Unregister(this);

                this.disposed = true;
            }
        }

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr GetHelpMessage(IntPtr @object);

            [Calli]
            public static extern void SetHelpMessage(IntPtr @object, string helpMessage);
        }
    }
}
