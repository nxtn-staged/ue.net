// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public sealed class UICommandList : IDisposable
    {
        internal readonly SharedReference Reference;

        private bool disposed;

        public UICommandList() => NativeMethods.Initialize(out this.Reference);

        ~UICommandList() => this.DisposeImpl();

        public void Dispose()
        {
            this.DisposeImpl();
            GC.SuppressFinalize(this);
        }

        private void DisposeImpl()
        {
            if (!this.disposed)
            {
                this.Reference.ReleaseReference();

                this.disposed = true;
            }
        }

        public void MapAction(UICommandInfo commandInfo, Action execute) =>
            NativeMethods.MapAction(
                this.Reference,
                commandInfo.Reference,
                Marshal.GetFunctionPointerForDelegate(execute),
                GCHandle.ToIntPtr(GCHandle.Alloc(execute)));

        [CLSCompliant(false)]
        public unsafe void MapAction(UICommandInfo commandInfo, delegate* unmanaged<void> execute) =>
            NativeMethods.MapAction_(
                this.Reference,
                commandInfo.Reference,
                execute);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out SharedReference commandList);

            [Calli]
            public static extern void MapAction(
                in SharedReference commandList,
                in SharedReference commandInfo,
                IntPtr execute,
                IntPtr executeHandle);

            [Calli]
            public static extern unsafe void MapAction_(
                in SharedReference commandList,
                in SharedReference commandInfo,
                delegate* unmanaged<void> execute);
        }
    }
}
