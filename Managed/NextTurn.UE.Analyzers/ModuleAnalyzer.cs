// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NextTurn.UE.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class ModuleAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor CtorDescriptor =
            Descriptor.Create("Class should declare one default constructor");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(CtorDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            INamedTypeSymbol symbol = context.Symbol as INamedTypeSymbol;
            if (symbol.BaseType is not null &&
                symbol.BaseType.Name == "NativeModule" &&
                !(symbol.InstanceConstructors.Length == 1 &&
                symbol.InstanceConstructors[0].DeclaredAccessibility == Accessibility.Internal &&
                symbol.InstanceConstructors[0].Parameters.IsEmpty))
            {
                context.ReportDiagnostic(Diagnostic.Create(CtorDescriptor, context.Symbol.Locations[0]));
            }
        }
    }
}
