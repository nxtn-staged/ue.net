// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Collections.Immutable;
using NextTurn.UE.Processors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NextTurn.UE.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal sealed class Analyzer : DiagnosticAnalyzer
    {
        internal static readonly DiagnosticDescriptor MethodShouldNotBeAppliedWithMultipleAttributes =
            Descriptor.Create("Method should not be applied with multiple processor attributes");

        internal static readonly DiagnosticDescriptor MethodShouldStartWithGet =
            Descriptor.Create("Method should start with 'Get'");

        internal static readonly DiagnosticDescriptor MethodShouldTakeOneIntPtrParameter =
            Descriptor.Create($"Method should take one {typeof(System.IntPtr).FullName} parameter");

        internal static readonly DiagnosticDescriptor MethodShouldTakeOneParameter =
            Descriptor.Create("Method should take one parameter");

        internal static readonly DiagnosticDescriptor NativeMethodsClassShouldBeNested =
            Descriptor.Create("NativeMethods classes should be nested");

        internal static readonly DiagnosticDescriptor NativeMethodsClassShouldBeStatic =
            Descriptor.Create("NativeMethods classes should be static");

        internal static readonly DiagnosticDescriptor NativeMethodsClassShouldNotContainConstructors =
            Descriptor.Create("NativeMethods classes should not contain constructors");

        internal static readonly DiagnosticDescriptor NativeMethodsClassShouldNotContainNestedTypes =
            Descriptor.Create("NativeMethods classes should not contain nested types");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(
                MethodShouldNotBeAppliedWithMultipleAttributes,
                MethodShouldStartWithGet,
                MethodShouldTakeOneIntPtrParameter,
                MethodShouldTakeOneParameter,
                NativeMethodsClassShouldBeNested,
                NativeMethodsClassShouldBeStatic,
                NativeMethodsClassShouldNotContainConstructors,
                NativeMethodsClassShouldNotContainNestedTypes);

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
                context.ReportDiagnostic(Diagnostic.Create(NativeMethodsClassShouldBeStatic, classSyntax.Identifier.GetLocation()));
            }

            if (!IsNestedClass(classSyntax))
            {
                context.ReportDiagnostic(Diagnostic.Create(NativeMethodsClassShouldBeNested, classSyntax.Identifier.GetLocation()));
            }

            foreach (MemberDeclarationSyntax memberSyntax in classSyntax.Members)
            {
                if (memberSyntax.IsKind(SyntaxKind.ClassDeclaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        NativeMethodsClassShouldNotContainNestedTypes,
                        (memberSyntax as ClassDeclarationSyntax).Identifier.GetLocation()));
                }

                if (memberSyntax.IsKind(SyntaxKind.ConstructorDeclaration))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        NativeMethodsClassShouldNotContainConstructors,
                        (memberSyntax as ConstructorDeclarationSyntax).Identifier.GetLocation()));
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
                if (!IsContainedInNamespace(attributeTypeSymbol, nameof(NextTurn), nameof(UE), nameof(Processors)))
                {
                    continue;
                }

                if (processorApplied)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        MethodShouldNotBeAppliedWithMultipleAttributes, methodSyntax.Identifier.GetLocation()));
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
                            context.ReportDiagnostic(Diagnostic.Create(
                                MethodShouldTakeOneParameter, methodSyntax.Identifier.GetLocation()));
                        }
                        else if (methodSymbol.Parameters[0].Type.SpecialType != SpecialType.System_IntPtr)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                MethodShouldTakeOneIntPtrParameter, methodSyntax.Identifier.GetLocation()));
                        }

                        if (!methodSyntax.Identifier.ValueText.StartsWith("Get"))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                MethodShouldStartWithGet, methodSyntax.Identifier.GetLocation()));
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
