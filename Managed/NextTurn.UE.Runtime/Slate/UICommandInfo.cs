// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public sealed class UICommandInfo : IDisposable
    {
        internal SharedReference Reference;

        private bool disposed;

        ~UICommandInfo() => this.DisposeImpl();

        public static UICommandInfo Create(BindingContext context, string name, string friendlyName, string description)
        {
            UICommandInfo result = new UICommandInfo();
            NativeMethods.Initialize(
                out result.Reference,
                context.reference,
                name,
                name + "_ToolTip",
                "." + name,
                friendlyName,
                description);

            return result;
        }

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

        public static void Unregister(BindingContext context, UICommandInfo commandInfo) =>
            NativeMethods.Unregister(context.reference, commandInfo.Reference);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(
                out SharedReference commandInfo,
                in SharedReference context,
                string name,
                string underscoreTooltipName,
                string dotName,
                string friendlyName,
                string description);

            [Calli]
            public static extern void Unregister(in SharedReference context, in SharedReference commandInfo);
        }
    }
}
