// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Leftice.Analyzers.Testing
{
    public sealed class AnalyzerTest<TAnalyzer, TVerifier> : CSharpAnalyzerTest<TAnalyzer, TVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
    {
        private static readonly MetadataReference RuntimeReference =
            MetadataReference.CreateFromFile(System.Reflection.Assembly.Load("System.Runtime").Location);

        private static readonly MetadataReference StandardReference =
            MetadataReference.CreateFromFile(System.Reflection.Assembly.Load("netstandard").Location);

        private static readonly MetadataReference SdkReference =
            MetadataReference.CreateFromFile(typeof(Processors.CalliAttribute).Assembly.Location);

        public AnalyzerTest() => this.Exclusions &= ~AnalysisExclusions.GeneratedCode;

        protected override Solution CreateSolution(ProjectId projectId, string language) =>
            base.CreateSolution(projectId, language)
                .AddMetadataReference(projectId, RuntimeReference)
                .AddMetadataReference(projectId, SdkReference)
                .AddMetadataReference(projectId, StandardReference);
    }

    internal sealed class AnalyzerVerifier<TAnalyzer> : AnalyzerVerifier<TAnalyzer, XUnitVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
    }

    internal class AnalyzerVerifier<TAnalyzer, TVerifier> : AnalyzerVerifier<TAnalyzer, AnalyzerTest<TAnalyzer, TVerifier>, TVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
    {
    }
}
