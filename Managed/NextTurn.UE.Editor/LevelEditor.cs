// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using NextTurn.UE.Annotations;

namespace Unreal.Editor
{
    public sealed class LevelEditor : NativeModule, IMenuExtensible, IToolBarExtensible
    {
        internal LevelEditor() { }

        public ExtensibilityManager MenuExtensibilityManager
        {
            get
            {
                ExtensibilityManager result = new ExtensibilityManager();
                NativeMethods.GetMenuExtensibilityManager(this.Pointer, out result.Reference);
                return result;
            }
        }

        public ExtensibilityManager ModeBarExtensibilityManager
        {
            get
            {
                ExtensibilityManager result = new ExtensibilityManager();
                NativeMethods.GetModeBarExtensibilityManager(this.Pointer, out result.Reference);
                return result;
            }
        }

        public ExtensibilityManager NotificationBarExtensibilityManager
        {
            get
            {
                ExtensibilityManager result = new ExtensibilityManager();
                NativeMethods.GetNotificationBarExtensibilityManager(this.Pointer, out result.Reference);
                return result;
            }
        }

        public ExtensibilityManager ToolBarExtensibilityManager
        {
            get
            {
                ExtensibilityManager result = new ExtensibilityManager();
                NativeMethods.GetToolBarExtensibilityManager(this.Pointer, out result.Reference);
                return result;
            }
        }

        internal void RegenerateMenu() => NativeMethods.RegenerateMenu(this.Pointer);

        private static class NativeMethods
        {
            [Calli]
            public static extern void GetMenuExtensibilityManager(IntPtr editor, out SharedReference manager);

            [Calli]
            public static extern void GetModeBarExtensibilityManager(IntPtr editor, out SharedReference manager);

            [Calli]
            public static extern void GetNotificationBarExtensibilityManager(IntPtr editor, out SharedReference manager);

            [Calli]
            public static extern void GetToolBarExtensibilityManager(IntPtr editor, out SharedReference manager);

            [Calli]
            public static extern void RegenerateMenu(IntPtr editor);
        }
    }
}
