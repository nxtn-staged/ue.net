// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

/*
using System;
using System.IO;
using System.Text;
using Unreal;
using Object = Unreal.Object;

namespace NextTurn.UE.Programs
{
    internal static class CppCodeGenerator
    {
        private static string outputDirectory = null!;
        private static StringBuilder classesIncludesText = null!;
        private static StringBuilder classesText = null!;

        internal static unsafe void AddClass(IntPtr classPtr, ScriptArray* headerFileNative)
        {
            string headerFile = StringMarshaler.ToManaged(headerFileNative);
            if (!string.IsNullOrEmpty(headerFile))
            {
                _ = classesIncludesText.AppendLine($"#include \"{headerFile}\"");
            }

            Class unrealClass = Object.Create<Class>(classPtr);
            string unrealClassName = unrealClass.Name.ToString();
            _ = classesText.AppendLine($"EXPORT_CLASS({unrealClassName}, {unrealClass.CppPrefix}{unrealClassName}),");
        }

        internal static void FinishExport()
        {
            File.WriteAllText(Path.Combine(outputDirectory, "ClassesIncludes.inl"), classesIncludesText.ToString());
            File.WriteAllText(Path.Combine(outputDirectory, "Classes.inl"), classesText.ToString());
        }

        internal static unsafe void Initialize(ScriptArray* outputDirectoryNative)
        {
            outputDirectory = StringMarshaler.ToManaged(outputDirectoryNative);
            classesIncludesText = new StringBuilder();
            classesText = new StringBuilder();
        }

        internal unsafe delegate void AddClass_Delegate(IntPtr classPtr, ScriptArray* headerFileNative);

        internal delegate void FinishExport_Delegate();

        internal unsafe delegate void Initialize_Delegate(ScriptArray* outputDirectoryNative);
    }
}
*/
