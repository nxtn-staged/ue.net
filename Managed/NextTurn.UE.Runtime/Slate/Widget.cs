// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public class Widget : IDisposable
    {
        private readonly SharedReference reference;

        private bool disposed;

        public Widget() => this.Initialize(out this.reference);

        ~Widget() => this.DisposeImpl();

        internal ref readonly SharedReference Reference => ref this.reference;

        public void AddMetaData(MetaData metaData) => NativeMethods.AddMetaData(this.reference, metaData.Reference);

        public void Dispose()
        {
            this.DisposeImpl();
            GC.SuppressFinalize(this);
        }

        private void DisposeImpl()
        {
            if (!this.disposed)
            {
                this.reference.ReleaseReference();

                this.disposed = true;
            }
        }

        private protected virtual unsafe void Initialize(out SharedReference reference) =>
            NativeMethods.Initialize(out reference);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out SharedReference widget);

            [Calli]
            public static extern void AddMetaData(in SharedReference widget, in SharedReference metaData);
        }
    }
}
