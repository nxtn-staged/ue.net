// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NextTurn.UE.Programs
{
    internal static class CSharpSyntaxGenerator
    {
        internal static StackAllocArrayCreationExpressionSyntax StackAllocArrayCreationExpression(TypeSyntax type, ExpressionSyntax size) =>
            SF.StackAllocArrayCreationExpression(ArrayType(type, SingletonList(ArrayRankSpecifier(SingletonSeparatedList(size)))));
    }
}
