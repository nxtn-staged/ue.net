// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using NextTurn.UE.Annotations;

namespace Unreal
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct SharedReference
    {
        private readonly IntPtr target;
        private readonly SharedReferencer sharedReferenceCount;

        internal void AddReference() => this.sharedReferenceCount.AddReference();

        internal unsafe ref readonly T GetTarget<T>() where T : unmanaged => ref Unsafe.AsRef<T>((T*)this.target);

        internal void ReleaseReference() => this.sharedReferenceCount.ReleaseReference();

        private readonly struct SharedReferencer
        {
            private readonly unsafe ReferenceController* referenceController;

            public unsafe void AddReference()
            {
                if (this.referenceController != null)
                {
                    ReferenceController.AddSharedReference(this.referenceController);
                }
            }

            public unsafe void ReleaseReference()
            {
                if (this.referenceController != null)
                {
                    ReferenceController.ReleaseSharedReference(this.referenceController);
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct ReferenceController
            {
                private readonly IntPtr vtablePtr;
                private int sharedReferenceCount;
                private int weakReferenceCount;

                public static unsafe void AddSharedReference(ReferenceController* referenceController) =>
                    Interlocked.Increment(ref referenceController->sharedReferenceCount);

                public static unsafe void ReleaseSharedReference(ReferenceController* referenceController)
                {
                    if (Interlocked.Decrement(ref referenceController->sharedReferenceCount) == 0)
                    {
                        NativeMethods.FinalizeTarget(referenceController);
                        ReleaseWeakReference(referenceController);
                    }
                }

                public static unsafe void ReleaseWeakReference(ReferenceController* referenceController)
                {
                    if (Interlocked.Decrement(ref referenceController->weakReferenceCount) == 0)
                    {
                        NativeMethods.DeleteReferenceController(referenceController);
                    }
                }

                private static class NativeMethods
                {
                    [Calli]
                    public static extern unsafe void DeleteReferenceController(ReferenceController* referenceController);

                    [Calli]
                    public static extern unsafe void FinalizeTarget(ReferenceController* referenceController);
                }
            }
        }
    }
}
