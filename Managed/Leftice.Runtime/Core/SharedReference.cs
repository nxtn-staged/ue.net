// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using Leftice;
using Leftice.Processors;

namespace Unreal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SharedReference
    {
        private readonly IntPtr target;
        private readonly SharedReferencer sharedReferenceCount;

        internal void AddReference() => this.sharedReferenceCount.AddReference();

        internal void ReleaseReference() => this.sharedReferenceCount.ReleaseReference();

        private readonly struct SharedReferencer
        {
            private readonly unsafe SharedReferenceController* referenceController;

            public unsafe void AddReference()
            {
                if (this.referenceController != null)
                {
                    SharedReferenceController.AddSharedReference(this.referenceController);
                }
            }

            public unsafe void ReleaseReference()
            {
                if (this.referenceController != null)
                {
                    SharedReferenceController.ReleaseSharedReference(this.referenceController);
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct SharedReferenceController
            {
                private readonly IntPtr vtablePtr;
                private int sharedReferenceCount;
                private int weakReferenceCount;

                public static unsafe void AddSharedReference(SharedReferenceController* referenceController) =>
                    Interlocked.Increment(ref referenceController->sharedReferenceCount);

                public static unsafe void ReleaseSharedReference(SharedReferenceController* referenceController)
                {
                    if (Interlocked.Decrement(ref referenceController->sharedReferenceCount) == 0)
                    {
                        NativeMethods.DestructTarget(referenceController);
                        ReleaseWeakReference(referenceController);
                    }
                }

                public static unsafe void ReleaseWeakReference(SharedReferenceController* referenceController)
                {
                    if (Interlocked.Decrement(ref referenceController->weakReferenceCount) == 0)
                    {
                        NativeMethods.DeleteReferenceController(referenceController);
                    }
                }

                private static class NativeMethods
                {
                    [Calli]
                    public static extern unsafe void DeleteReferenceController(SharedReferenceController* referenceController);

                    [Calli]
                    public static extern unsafe void DestructTarget(SharedReferenceController* referenceController);
                }
            }
        }
    }
}
