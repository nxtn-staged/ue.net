// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;

namespace NextTurn.UE.Sdk.Tests
{
    public static class ProcessorTests
    {
        private const string DefinitionAssemblyName = "NextTurn.UE.Runtime";
        private const string ReferenceAssemblyName = "NextTurn.UE.Editor";

        private const string TestNamespace = "NextTurn.UE";
        private const string TestTypeName = "Enclosing";

        private static readonly SyntaxTree ImportTableSyntaxTree =
            CSharpSyntaxTree.ParseText(@"
using System;

namespace NextTurn.UE
{
    public readonly ref struct ImportTable
    {
        public static ImportTable Get(string typeName) => throw new NotImplementedException();

        public void Dispose() => throw new NotImplementedException();

        public IntPtr GetField(string key) => throw new NotImplementedException();

        public int GetInt32(string key) => throw new NotImplementedException();

        public IntPtr GetMethod(string key) => throw new NotImplementedException();

        public int GetOffset(string key) => throw new NotImplementedException();

        public IntPtr GetPointer(string key) => throw new NotImplementedException();

        public ushort GetUInt16(string key) => throw new NotImplementedException();
    }
}
");

        private static readonly SyntaxTree ImportTableReferenceSyntaxTree =
            CSharpSyntaxTree.ParseText(@"
using System;

namespace NextTurn.UE
{
    internal static class ImportTableReference
    {
        internal static ImportTable Get(string typeName) => ImportTable.Get(typeName);

        internal static void Dispose(this ImportTable @this) => @this.Dispose();

        internal static IntPtr GetField(this ImportTable @this, string key) => @this.GetField(key);

        internal static int GetInt32(this ImportTable @this, string key) => @this.GetInt32(key);

        internal static IntPtr GetMethod(this ImportTable @this, string key) => @this.GetMethod(key);

        internal static int GetOffset(this ImportTable @this, string key) => @this.GetOffset(key);

        internal static IntPtr GetPointer(this ImportTable @this, string key) => @this.GetPointer(key);

        internal static ushort GetUInt16(this ImportTable @this, string key) => @this.GetUInt16(key);
    }
}
");

        private static readonly MetadataReference CoreLibReference =
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

        private static readonly MetadataReference RuntimeReference =
            MetadataReference.CreateFromFile(System.Reflection.Assembly.Load("System.Runtime").Location);

        private static readonly MetadataReference StandardReference =
            MetadataReference.CreateFromFile(System.Reflection.Assembly.Load("netstandard").Location);

        private static readonly MetadataReference SdkReference =
            MetadataReference.CreateFromFile(typeof(Processors.CalliAttribute).Assembly.Location);

        private static void CompileAssembly(string source, Action<string> verification)
        {
            string definitionPath = Path.GetTempFileName();
            string referencePath = Path.GetTempFileName();

            VerifyEmitSucceeded(CSharpCompilation.Create(
                DefinitionAssemblyName,
                new[] { CSharpSyntaxTree.ParseText(source), ImportTableSyntaxTree },
                new[] { CoreLibReference, SdkReference, RuntimeReference, StandardReference },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .Emit(definitionPath));

            verification(definitionPath);

            VerifyEmitSucceeded(CSharpCompilation.Create(
                ReferenceAssemblyName,
                new[] { CSharpSyntaxTree.ParseText(source), ImportTableReferenceSyntaxTree },
                new[] { CoreLibReference, SdkReference, RuntimeReference, StandardReference, MetadataReference.CreateFromFile(definitionPath) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .Emit(referencePath));

            verification(referencePath);

            File.Delete(definitionPath);
            File.Delete(referencePath);

            static void VerifyEmitSucceeded(Microsoft.CodeAnalysis.Emit.EmitResult result) =>
                Assert.True(result.Diagnostics.IsEmpty, string.Join(Environment.NewLine, result.Diagnostics));
        }

        private static void VerifyProcessAssembly(string source, Action<ModuleDefinition> assertion) =>
            CompileAssembly(source, path =>
            {
                Assert.True(new Processor { AssemblyFile = new AssemblyFileTaskItem(path) }.Execute());

                using var module = ModuleDefinition.ReadModule(path);
                assertion(module);
            });

        private static Instruction GetInstruction(OpCode opcode, MethodDefinition method) =>
            method.Body.Instructions.Single(inst => inst.OpCode == opcode);

        private static TypeDefinition GetNativeMethodsType(ModuleDefinition module) =>
            module.GetType(TestNamespace, TestTypeName).NestedTypes.Single();

        private static MethodDefinition GetTestMethod(ModuleDefinition module) =>
            GetNativeMethodsType(module).Methods.Where(method => !method.IsConstructor).Single();

        private static void VerifyContainsOpCode(OpCode opcode, MethodDefinition method) =>
            Assert.Single(method.Body.Instructions, inst => inst.OpCode == opcode);

        private static void VerifyDoesNotContainOpCode(OpCode opcode, MethodDefinition method) =>
            Assert.DoesNotContain(method.Body.Instructions, inst => inst.OpCode == opcode);

        [Fact]
        public static void ImplementCalli() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.Calli]
            static extern void Invoke(System.IntPtr source);
        }}
    }}
}}
",
                module => VerifyContainsOpCode(OpCodes.Calli, GetTestMethod(module)));

        [Fact]
        public static void ImplementCalli_BooleanParam() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.Calli]
            static extern void Invoke(bool argument);
        }}
    }}
}}
",
                module => Assert.Equal(
                    typeof(byte).FullName!,
                    (GetInstruction(OpCodes.Calli, GetTestMethod(module)).Operand as CallSite)!.Parameters.Single().ParameterType.FullName));

        [Fact]
        public static void ImplementCalli_BooleanReturnValue() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.Calli]
            static extern bool Invoke();
        }}
    }}
}}
",
                module => Assert.Equal(
                    typeof(byte).FullName!,
                    (GetInstruction(OpCodes.Calli, GetTestMethod(module)).Operand as CallSite)!.ReturnType.FullName));

        [Fact]
        public static void ImplementField() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
#pragma warning disable CS0649 // Field is never assigned to
            public static readonly System.IntPtr Value;
#pragma warning restore CS0649 // Field is never assigned to
        }}
    }}
}}
",
                module => VerifyContainsOpCode(OpCodes.Stsfld, GetNativeMethodsType(module).Methods.Single()));

        [Fact]
        public static void ImplementPointerOffset() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.PointerOffset]
            static extern void GetValuePtr(System.IntPtr source);
        }}
    }}
}}
",
                module =>
                {
                    var method = GetTestMethod(module);

                    VerifyContainsOpCode(OpCodes.Add, method);
                    VerifyDoesNotContainOpCode(OpCodes.Ldobj, method);
                    VerifyDoesNotContainOpCode(OpCodes.Stobj, method);
                });

        [Fact]
        public static void ImplementReadOffset() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.ReadOffset]
            static extern void GetValue(System.IntPtr source);
        }}
    }}
}}
",
                module => VerifyContainsOpCode(OpCodes.Ldobj, GetTestMethod(module)));

        [Fact]
        public static void ProcessNotProcessor() =>
            VerifyProcessAssembly(
                $@"
class CalliAttribute : System.Attribute
{{
}}

namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{

        static class NativeMethods
        {{
            [global::Calli]
            static extern void Invoke();
        }}
    }}
}}
",
                module => VerifyDoesNotContainOpCode(OpCodes.Calli, GetTestMethod(module)));

        [Fact]
        public static void ProcessPinvokeImpl() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [System.Runtime.InteropServices.DllImport(""NextTurn.UE"")]
            static extern void Invoke();
        }}
    }}
}}
",
                module => Assert.False(GetTestMethod(module).HasBody));

        [Fact]
        public static void RemoveCalliAttribute() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.Calli]
            static extern void Invoke();
        }}
    }}
}}
",
                module => Assert.False(GetTestMethod(module).HasCustomAttributes));

        [Fact]
        public static void RemovePointerOffsetAttribute() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.PointerOffset]
            static extern void GetValuePtr(System.IntPtr source);
        }}
    }}
}}
",
                module => Assert.False(GetTestMethod(module).HasCustomAttributes));

        [Fact]
        public static void RemoveReadOffsetAttribute() =>
            VerifyProcessAssembly(
                $@"
namespace {TestNamespace}
{{
    static class {TestTypeName}
    {{
        static class NativeMethods
        {{
            [NextTurn.UE.Processors.ReadOffset]
            static extern void GetValue(System.IntPtr source);
        }}
    }}
}}
",
                module => Assert.False(GetTestMethod(module).HasCustomAttributes));

        private sealed class AssemblyFileTaskItem : ITaskItem
        {
            private readonly string path;

            public AssemblyFileTaskItem(string path) => this.path = path;

            public string ItemSpec
            {
                get => this.path;
                set => throw new NotImplementedException();
            }

            public ICollection MetadataNames =>
                throw new NotImplementedException();

            public int MetadataCount =>
                throw new NotImplementedException();

            public IDictionary CloneCustomMetadata() =>
                throw new NotImplementedException();

            public void CopyMetadataTo(ITaskItem destinationItem) =>
                throw new NotImplementedException();

            public string GetMetadata(string metadataName) =>
                throw new NotImplementedException();

            public void RemoveMetadata(string metadataName) =>
                throw new NotImplementedException();

            public void SetMetadata(string metadataName, string metadataValue) =>
                throw new NotImplementedException();
        }
    }
}
