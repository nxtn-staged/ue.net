// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Microsoft.CodeAnalysis;

namespace NextTurn.UE.Analyzers
{
    internal static class Descriptor
    {
        internal static DiagnosticDescriptor Create(string title) =>
            new DiagnosticDescriptor(
                id: "UENET",
                title: title,
                messageFormat: title,
                category: string.Empty,
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);
    }
}
