// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using NextTurn.UE.Annotations;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NextTurn.UE.Sdk
{
    [Generator]
    public sealed class Generator : ISourceGenerator
    {
        private const string ImportTableTypeName = "ImportTable";
        private const string GetMethodName = "Get";
        private const string GetFieldMethodName = "GetField";
        private const string GetMethodMethodName = "GetMethod";
        private const string GetOffsetMethodName = "GetOffset";

        private const string NativeMethodsTypeName = "NativeMethods";

        private SyntaxNode intPtrType = null!;

        private SyntaxNode importTableType = null!;

        private SyntaxNode importTableGetMethod = null!;

        private EmptyStatementSyntax placeholder = null!;

        private SyntaxGenerator sg = null!;

        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            foreach (var syntaxTree in context.Compilation.SyntaxTrees)
            {
                if (syntaxTree.GetRoot(context.CancellationToken) is CompilationUnitSyntax compilationUnit)
                {
                    foreach (var memberDeclaration in compilationUnit.Members)
                    {
                        if (memberDeclaration is NamespaceDeclarationSyntax namespaceDeclaration)
                        {
                            foreach (var memberDeclaration1 in namespaceDeclaration.Members)
                            {
                                if (memberDeclaration1 is ClassDeclarationSyntax classDeclaration)
                                {
                                    this.ProcessType(classDeclaration);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ImportCalli()
        {

        }

        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
            using var workspace = new AdhocWorkspace();
            var sg = this.sg = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);

            var globalNamespace = SF.IdentifierName(SF.Token(SyntaxKind.GlobalKeyword));
            this.intPtrType = sg.QualifiedName(SF.AliasQualifiedName(globalNamespace, (IdentifierNameSyntax)this.sg.IdentifierName(nameof(System))), this.sg.IdentifierName(nameof(IntPtr)));

            var internalNamespace = sg.QualifiedName(SF.AliasQualifiedName(globalNamespace, (IdentifierNameSyntax)sg.IdentifierName(nameof(NextTurn))), sg.IdentifierName(nameof(UE)));
            var importTableType = this.importTableType = sg.QualifiedName(internalNamespace, sg.IdentifierName(ImportTableTypeName));
            this.importTableGetMethod = sg.QualifiedName(importTableType, sg.IdentifierName(GetMethodName));

            this.placeholder = SF.EmptyStatement();
        }

        private void ProcessMethod(MethodDeclarationSyntax methodDeclaration)
        {
            foreach (var attributeList in methodDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (null == nameof(CalliAttribute))
                    {

                    }

                    if (null == nameof(ReadOffsetAttribute))
                    {

                    }

                    if (null == nameof(PointerOffsetAttribute))
                    {

                    }
                }
            }
        }

        private void ProcessType(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration.Identifier.ValueText != NativeMethodsTypeName)
            {
                foreach (var memberDeclaration in classDeclaration.Members)
                {
                    if (memberDeclaration is ClassDeclarationSyntax classDeclaration1)
                    {
                        this.ProcessType(classDeclaration1);
                    }
                }

                return;
            }

            var members = new List<SyntaxNode>();

            var statements = new List<SyntaxNode>();

            const string tableLocalName = "__table";
            var tableLocalExpression = this.sg.IdentifierName(tableLocalName);

            statements.Add(this.placeholder);

            int index = 0;

            foreach (var memberDeclaration1 in classDeclaration.Members)
            {
                switch (memberDeclaration1)
                {
                    case FieldDeclarationSyntax:
                        statements.Add(
                            this.sg.InvocationExpression(
                                this.sg.MemberAccessExpression(
                                    tableLocalExpression,
                                    GetFieldMethodName),
                                this.sg.LiteralExpression(index++)));
                        break;

                    case MethodDeclarationSyntax methodDeclaration:
                        this.ProcessMethod(methodDeclaration);
                        break;

                    default:
                        break;
                }
            }

            _ = this.sg.LocalDeclarationStatement(
                this.importTableType,
                tableLocalName,
                this.sg.InvocationExpression(
                    this.importTableGetMethod,
                    this.sg.LiteralExpression("0"),
                    this.sg.LiteralExpression(0)));

            _ = this.sg.ConstructorDeclaration(
                NativeMethodsTypeName,
                null,
                Accessibility.NotApplicable,
                DeclarationModifiers.Static,
                null,
                null);

            _ = this.sg.ClassDeclaration(
                NativeMethodsTypeName,
                null,
                Accessibility.NotApplicable,
                DeclarationModifiers.Partial,
                null,
                null,
                null);
        }
    }
}
