// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;

namespace NextTurn.UE.Analyzers.Testing
{
    public sealed class AnalyzerTest<TAnalyzer, TVerifier> : CSharpAnalyzerTest<TAnalyzer, TVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
    {
        public AnalyzerTest()
        {
            this.TestBehaviors = TestBehaviors.SkipGeneratedCodeCheck | TestBehaviors.SkipSuppressionCheck;

            var references = this.TestState.AdditionalReferences;

            references.Add(Assembly.Load("System.Runtime"));
            references.Add(Assembly.Load("netstandard"));
            references.Add(typeof(Processors.CalliAttribute).Assembly);
        }
    }

    internal sealed class AnalyzerVerifier<TAnalyzer> : AnalyzerVerifier<TAnalyzer, XUnitVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
    }

    internal class AnalyzerVerifier<TAnalyzer, TVerifier> : AnalyzerVerifier<TAnalyzer, AnalyzerTest<TAnalyzer, TVerifier>, TVerifier>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TVerifier : IVerifier, new()
    {
        internal static Task VerifyAnalyzerAsync(string source, Encoding encoding, params DiagnosticResult[] expected)
        {
            var test = new AnalyzerTest<TAnalyzer, TVerifier>();
            test.TestState.Sources.Add(SourceText.From(source, encoding));
            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync();
        }
    }
}
