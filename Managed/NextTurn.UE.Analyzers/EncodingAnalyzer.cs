// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace NextTurn.UE.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class EncodingAnalyzer : DiagnosticAnalyzer
    {
        internal static readonly DiagnosticDescriptor EncodingDescriptor =
            Descriptor.Create("Encoding");

        private static readonly int UTF8CodePage = Encoding.UTF8.CodePage;

        private static readonly byte[] UTF8Preamble = Encoding.UTF8.GetPreamble();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(EncodingDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(Analyze);
        }

        private static void Analyze(SyntaxTreeAnalysisContext context)
        {
            Encoding encoding = context.Tree.Encoding;
            if (encoding.CodePage != UTF8CodePage || encoding.GetPreamble().Length == UTF8Preamble.Length)
            {
                context.ReportDiagnostic(Diagnostic.Create(EncodingDescriptor, Location.Create(context.Tree, TextSpan.FromBounds(0, 0))));
            }
        }
    }
}
