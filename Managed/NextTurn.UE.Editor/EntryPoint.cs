// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Runtime.InteropServices;
using Unreal;
using Unreal.Editor;
using Unreal.Slate;

namespace NextTurn.UE.Editor
{
    internal static class EntryPoint
    {
        private static readonly ScriptArray classes;

        private static readonly Extender.ExtendMenuCallback ExtendMenu = menu =>
        {
            menu.BeginSection();
            {
                menu.AddMenuEntry(Commands.Build);
            }

            menu.EndSection();
        };

        private static BindingContext context = null!;
        private static Extender extender = null!;

        private static ConsoleCommand command = null!;
        private static ConsoleCommand command2 = null!;

        private static readonly Action load = () =>
        {
            _ = System.Diagnostics.Debugger.Launch();
            try
            {
                var alc = System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(System.Reflection.Assembly.GetExecutingAssembly())!;
                var proj = alc.LoadFromAssemblyPath(@"C:\Users\Liim\Documents\Unreal Projects\MyProject\Binaries\UE.NET\NextTurn.UE.Project.dll");
                var ep = proj.GetType("EntryPoint", true)!;
                var load = ep.GetMethod("Load", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
                var unload = ep.GetMethod("Unload", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
                _ = load.Invoke(null, null);
                var a = alc.LoadFromAssemblyPath(@"C:\Users\Liim\Documents\Unreal Projects\MyProject\Plugins\UE.NET\Managed\NextTurn.UE.Tests\bin\Debug\net5.0\NextTurn.UE.Tests.dll");
                var t = a.GetType("NextTurn.UE.Tests.Program", true)!;
                var m = t.GetMethod("Main", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)!;
                _ = m.Invoke(null, null);
                _ = unload.Invoke(null, null);
            }
            catch (Exception e)
            {
            }
        };

        [UnmanagedCallersOnly]
        internal static void Start()
        {
            Log.Warning("Welcome to UE.NET!");

            ModuleManager.KnownModules.Add(typeof(LevelEditor), new Name("LevelEditor"));
            try
            {
                command = Unreal.Console.RegisterCommand("build", "Hello message", /*Callbacks.Build*/load);
                command2 = Unreal.Console.RegisterCommand("projtest", "Hello message", load);

                context = new BindingContext(
                    new Name("NextTurnEditor"),
                    Text.FromName(new Name("NextTurn Editor")),
                    Name.None,
                    new Name("EditorStyle"));

                Commands.Build = UICommandInfo.Create(context, "Build", "Build", "Build");

                UICommandList list = new UICommandList();
                list.MapAction(Commands.Build, Callbacks.Build);

                extender = new Extender();
                _ = extender.AddMenuExtension(new Name("UENETLoad"), ExtensionPosition.After, list, ExtendMenu);

                LevelEditor editor = ModuleManager.LoadModule<LevelEditor>();
                editor.MenuExtensibilityManager.AddExtender(extender);
                editor.RegenerateMenu();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        [UnmanagedCallersOnly]
        internal static void Stop()
        {
            try
            {
                command.Dispose();
                command2.Dispose();

                UICommandInfo.Unregister(context, Commands.Build);

                LevelEditor editor = ModuleManager.LoadModule<LevelEditor>();
                editor.MenuExtensibilityManager.RemoveExtender(extender);
                editor.RegenerateMenu();

                Commands.Build.Dispose();
                extender.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        private static class Commands
        {
            public static UICommandInfo Build { get; set; } = null!;
        }

        private static class Callbacks
        {
            public static Action Build { get; } = () =>
            {
                try
                {
                    if (classes.Count == 0)
                    {
                        unsafe
                        {
                            fixed (ScriptArray* result = &classes)
                            {
                                Class.NativeMethods.FindObjects(Class.GetClass<Class>().pointer, result);
                            }
                        }
                    }

                    //using SlowTask task = new SlowTask(2);

                    Type codegen = Type.GetType("NextTurn.UE.Programs.CodeGenerator, NextTurn.UE.Programs")!;
                    _ = codegen.InvokeMember(
                        "Initialize",
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        null,
                        new object[] { });

                    for (int i = 0; i < classes.Count; i++)
                    {
                        _ = codegen.InvokeMember(
                            "AddClass",
                            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod,
                            null,
                            null,
                            new object[] { classes.GetItem<IntPtr>(i) });
                    }

                    _ = codegen.InvokeMember(
                        "FinishExport",
                        System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.InvokeMethod,
                        null,
                        null,
                        new object[] { });
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            };
        }
    }
}
