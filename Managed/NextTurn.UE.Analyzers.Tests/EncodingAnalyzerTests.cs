// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using System.Collections.Generic;
using System.Text;
using Xunit;
using Verifier = NextTurn.UE.Analyzers.Testing.AnalyzerVerifier<NextTurn.UE.Analyzers.EncodingAnalyzer>;

namespace NextTurn.UE.Analyzers.Tests
{
    public static class EncodingAnalyzerTests
    {
        public static IEnumerable<object[]> NonUTF8Encodings
        {
            get
            {
                yield return new[] { Encoding.ASCII };
                yield return new[] { Encoding.BigEndianUnicode };
                yield return new[] { Encoding.Unicode };
                yield return new[] { Encoding.UTF32 };
                yield return new[] { Encoding.UTF7 };
            }
        }

        [Theory]
        [MemberData(nameof(NonUTF8Encodings))]
        public static async void NotUTF8(Encoding encoding) =>
            await Verifier.VerifyAnalyzerAsync(
                string.Empty,
                encoding,
                Verifier.Diagnostic().WithLocation(1, 1));

        [Fact]
        public static async void UTF8WithBOM() =>
            await Verifier.VerifyAnalyzerAsync(
                string.Empty,
                Encoding.UTF8,
                Verifier.Diagnostic().WithLocation(1, 1));

        [Fact]
        public static async void UTF8WithoutBOM() =>
            await Verifier.VerifyAnalyzerAsync(
                string.Empty,
                new UTF8Encoding());
    }
}
