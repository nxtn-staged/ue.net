// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using NextTurn.UE.Annotations;

namespace Unreal.Slate
{
    public sealed class Extender : IDisposable
    {
        internal readonly SharedReference Reference;

        private bool disposed;

        public Extender() => NativeMethods.Initialize(out this.Reference);

        ~Extender() => this.DisposeImpl();

        public Extension AddMenuBarExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            ExtendMenuBarCallback extendMenuBar)
        {
            Extension result = new Extension();
            NativeMethods.AddMenuBarExtension(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                Marshal.GetFunctionPointerForDelegate(extendMenuBar),
                GCHandle.ToIntPtr(GCHandle.Alloc(extendMenuBar)),
                out result.Reference);

            return result;
        }

        [CLSCompliant(false)]
        public unsafe Extension AddMenuBarExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            delegate* unmanaged<MenuBarBuilder, void> extendMenuBar)
        {
            Extension result = new Extension();
            NativeMethods.AddMenuBarExtension_(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                extendMenuBar,
                out result.Reference);

            return result;
        }

        public Extension AddMenuExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            ExtendMenuCallback extendMenu)
        {
            Extension result = new Extension();
            NativeMethods.AddMenuExtension(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                Marshal.GetFunctionPointerForDelegate(extendMenu),
                GCHandle.ToIntPtr(GCHandle.Alloc(extendMenu)),
                out result.Reference);

            return result;
        }

        [CLSCompliant(false)]
        public unsafe Extension AddMenuExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            delegate* unmanaged<MenuBuilder, void> extendMenu)
        {
            Extension result = new Extension();
            NativeMethods.AddMenuExtension_(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                extendMenu,
                out result.Reference);

            return result;
        }

        public Extension AddToolBarExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            ExtendToolBarCallback extendToolBar)
        {
            Extension result = new Extension();
            NativeMethods.AddToolBarExtension(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                Marshal.GetFunctionPointerForDelegate(extendToolBar),
                GCHandle.ToIntPtr(GCHandle.Alloc(extendToolBar)),
                out result.Reference);

            return result;
        }

        [CLSCompliant(false)]
        public unsafe Extension AddToolBarExtension(
            Name extensionPoint,
            ExtensionPosition position,
            UICommandList commandList,
            delegate* unmanaged<ToolBarBuilder, void> extendToolBar)
        {
            Extension result = new Extension();
            NativeMethods.AddToolBarExtension_(
                this.Reference,
                extensionPoint,
                position,
                commandList.Reference,
                extendToolBar,
                out result.Reference);

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

        public delegate void ExtendMenuBarCallback(MenuBarBuilder menuBar);

        public delegate void ExtendMenuCallback(MenuBuilder menu);

        public delegate void ExtendToolBarCallback(ToolBarBuilder toolBar);

        private static class NativeMethods
        {
            [Calli]
            public static extern void Initialize(out SharedReference extender);

            [Calli]
            public static extern void AddMenuBarExtension(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                IntPtr extendMenuBar,
                IntPtr extendMenuBarHandle,
                out SharedReference extension);

            [Calli]
            public static extern unsafe void AddMenuBarExtension_(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                delegate* unmanaged<MenuBarBuilder, void> extendMenuBar,
                out SharedReference extension);

            [Calli]
            public static extern void AddMenuExtension(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                IntPtr extendMenu,
                IntPtr extendMenuHandle,
                out SharedReference extension);

            [Calli]
            public static extern unsafe void AddMenuExtension_(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                delegate* unmanaged<MenuBuilder, void> extendMenu,
                out SharedReference extension);

            [Calli]
            public static extern void AddToolBarExtension(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                IntPtr extendToolBar,
                IntPtr extendToolBarHandle,
                out SharedReference extension);

            [Calli]
            public static extern unsafe void AddToolBarExtension_(
                in SharedReference extender,
                Name extensionPoint,
                ExtensionPosition position,
                in SharedReference commandList,
                delegate* unmanaged<ToolBarBuilder, void> extendToolBar,
                out SharedReference extension);
        }
    }
}
