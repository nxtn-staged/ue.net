// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

using System;
using Leftice.Processors;

namespace Unreal
{
    public abstract class ConsoleObject
    {
        private readonly IntPtr pointer;

        internal ConsoleObject(IntPtr pointer) => this.pointer = pointer;

        public string HelpMessage
        {
            get => Leftice.CharArrayMarshaler.ToManaged(NativeMethods.GetHelpMessage(this.pointer))!;

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

        private static class NativeMethods
        {
            [Calli]
            public static extern IntPtr GetHelpMessage(IntPtr @object);

            [Calli]
            public static extern void SetHelpMessage(IntPtr @object, string helpMessage);
        }
    }
}
