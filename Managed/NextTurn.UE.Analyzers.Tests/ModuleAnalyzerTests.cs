// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Xunit;
using Verifier = NextTurn.UE.Analyzers.Testing.AnalyzerVerifier<NextTurn.UE.Analyzers.ModuleAnalyzer>;

namespace NextTurn.UE.Analyzers.Tests
{
    public static class ModuleAnalyzerTests
    {
        [Fact]
        public static async void NoConstructor() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
class NativeModule
{
}

class Module : global::NativeModule
{
}
",
                Verifier.Diagnostic().WithLocation(6, 7));
    }
}
