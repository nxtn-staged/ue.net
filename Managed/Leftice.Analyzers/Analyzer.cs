// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using System.Collections.Immutable;
using Leftice.Processors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Leftice.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class Analyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(
                id: "LEFTICE",
                title: string.Empty,
                messageFormat: "{0}",
                category: string.Empty,
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
            context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeClass(SyntaxNodeAnalysisContext context)
        {
            ClassDeclarationSyntax classSyntax = context.Node as ClassDeclarationSyntax;
            if (!IsNativeMethodsClass(classSyntax))
            {
                return;
            }

            if (!classSyntax.Modifiers.Any(SyntaxKind.StaticKeyword))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, classSyntax.Identifier.GetLocation(),
                    "Class should be static"));
            }

            if (!IsNestedClass(classSyntax))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, classSyntax.Identifier.GetLocation(),
                    "Class should be nested"));
            }

            foreach (MemberDeclarationSyntax memberSyntax in classSyntax.Members)
            {
                if (memberSyntax.IsKind(SyntaxKind.ClassDeclaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, (memberSyntax as ClassDeclarationSyntax).Identifier.GetLocation(),
                        "Class should not contain nested types"));
                }

                if (memberSyntax.IsKind(SyntaxKind.ConstructorDeclaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, (memberSyntax as ConstructorDeclarationSyntax).Identifier.GetLocation(),
                        "Class should not contain constructors"));
                }
            }
        }

        private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
        {
            MethodDeclarationSyntax methodSyntax = context.Node as MethodDeclarationSyntax;
            if (!IsContainedInNativeMethodsClass(methodSyntax))
            {
                return;
            }

            IMethodSymbol methodSymbol = context.ContainingSymbol as IMethodSymbol;
            ImmutableArray<AttributeData> attributes = methodSymbol.GetAttributes();
            bool processorApplied = false;
            foreach (AttributeData attributeData in attributes)
            {
                INamedTypeSymbol attributeTypeSymbol = attributeData.AttributeClass;
                if (!IsContainedInNamespace(attributeTypeSymbol, nameof(Leftice), nameof(Processors)))
                {
                    continue;
                }

                if (processorApplied)
                {
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, methodSyntax.Identifier.GetLocation(),
                        "Method should not be applied with multiple processor attributes"));
                }

                processorApplied = true;

                switch (attributeTypeSymbol.Name)
                {
                    case nameof(CalliAttribute):
                        break;

                    case nameof(PointerOffsetAttribute):
                    case nameof(ReadOffsetAttribute):
                        if (methodSymbol.Parameters.Length != 1)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, methodSyntax.Identifier.GetLocation(),
                                "Method should take one parameter"));
                        }
                        else if (methodSymbol.Parameters[0].Type.SpecialType != SpecialType.System_IntPtr)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, methodSyntax.Identifier.GetLocation(),
                                $"Method should take one '{typeof(System.IntPtr).FullName}' parameter"));
                        }

                        if (!methodSyntax.Identifier.ValueText.StartsWith("Get"))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, methodSyntax.Identifier.GetLocation(),
                                "Method should start with 'Get'"));
                        }

                        break;
                }
            }
        }

        private static bool IsContainedInNamespace(INamespaceOrTypeSymbol symbol, params string[] namespaces)
        {
            for (int i = namespaces.Length - 1; i >= 0; i--)
            {
                INamespaceSymbol namespaceSymbol = symbol.ContainingNamespace;
                if (namespaceSymbol.Name != namespaces[i])
                {
                    return false;
                }

                symbol = namespaceSymbol;
            }

            return symbol.ContainingNamespace.IsGlobalNamespace;
        }

        private static bool IsContainedInNativeMethodsClass(MethodDeclarationSyntax syntax) =>
            syntax.Parent is ClassDeclarationSyntax classSyntax && IsNativeMethodsClass(classSyntax);

        private static bool IsNativeMethodsClass(ClassDeclarationSyntax syntax) =>
            syntax.Identifier.ValueText == "NativeMethods";

        private static bool IsNestedClass(ClassDeclarationSyntax syntax) =>
            syntax.Parent is TypeDeclarationSyntax;
    }
}
