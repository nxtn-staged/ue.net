// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal.Slate
{
    public sealed class Extension : IDisposable
    {
        internal SharedReference Reference;

        private bool disposed;

        internal Extension()
        {
        }

        ~Extension() => this.DisposeImpl();

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
    }
}
