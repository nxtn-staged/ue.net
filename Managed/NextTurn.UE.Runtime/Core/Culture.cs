// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;

namespace Unreal
{
    public sealed class Culture : IDisposable
    {
        private readonly SharedReference reference;

        private bool disposed;

        internal unsafe Culture(SharedReference* reference)
        {
            this.reference = *reference;
            this.reference.AddReference();
        }

        ~Culture() => this.DisposeImpl();

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

                this.disposed = false;
            }
        }
    }
}
