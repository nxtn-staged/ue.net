// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NextTurn.UE.Annotations;
using Unreal.Slate;

namespace Unreal.Editor
{
    public static class LevelEditorExtensions
    {
        private static readonly HashSet<MulticastDelegate> Delegates = new HashSet<MulticastDelegate>();

        public static void AddMenuBarExtension(this LevelEditor editor, Name extensionPoint, ExtensionPosition position, ExtendMenuBarCallback extendMenuBar)
        {
            _ = Delegates.Add(extendMenuBar);
            NativeMethods.AddMenuBarExtension(default, extensionPoint, position, Marshal.GetFunctionPointerForDelegate(extendMenuBar));
        }

        public static void AddMenuExtension(this LevelEditor editor, Name extensionPoint, ExtensionPosition position, Action execute)
        {
            _ = Delegates.Add(execute);
            NativeMethods.AddMenuExtension(default, extensionPoint, position, Marshal.GetFunctionPointerForDelegate(execute));
        }

        public static void AddToolBarExtension(this LevelEditor editor, Name extensionPoint, ExtensionPosition position, ExtendToolBarCallback extendToolBar)
        {
            _ = Delegates.Add(extendToolBar);
            NativeMethods.AddToolBarExtension(default, extensionPoint, position, Marshal.GetFunctionPointerForDelegate(extendToolBar));
        }

        public delegate void ExtendMenuBarCallback(MenuBuilder menu);

        public delegate void ExtendToolBarCallback(ToolBarBuilder toolBar);

        private static class NativeMethods
        {
            [Calli]
            public static extern void AddMenuBarExtension(
                IntPtr editor,
                Name extensionPoint,
                ExtensionPosition position,
                IntPtr extendMenuBar);

            [Calli]
            public static extern void AddMenuExtension(
                IntPtr editor,
                Name extensionPoint,
                ExtensionPosition position,
                IntPtr execute);

            [Calli]
            public static extern void AddToolBarExtension(
                IntPtr editor,
                Name extensionPoint,
                ExtensionPosition position,
                IntPtr extendToolBar);
        }
    }
}
