// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

using Xunit;
using static NextTurn.UE.Analyzers.Analyzer;
using Verifier = NextTurn.UE.Analyzers.Testing.AnalyzerVerifier<NextTurn.UE.Analyzers.Analyzer>;

namespace NextTurn.UE.Analyzers.Tests
{
    public static class AnalyzerTests
    {
        [Fact]
        public static async void NotStatic() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    class NativeMethods
    {
    }
}
",
                Verifier.Diagnostic(NativeMethodsClassShouldBeStatic).WithLocation(4, 11));

        [Fact]
        public static async void NotNested() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class NativeMethods
{
}
",
                Verifier.Diagnostic(NativeMethodsClassShouldBeNested).WithLocation(2, 14));

        [Fact]
        public static async void NestedType() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        static class Nested
        {
        }
    }
}
",
                Verifier.Diagnostic(NativeMethodsClassShouldNotContainNestedTypes).WithLocation(6, 22));

        [Fact]
        public static async void Constructor() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        static NativeMethods()
        {
        }
    }
}
",
                Verifier.Diagnostic(NativeMethodsClassShouldNotContainConstructors).WithLocation(6, 16));

        [Fact]
        public static async void NotContainedInNativeMethodsClass() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
struct NativeMethods
{
    [NextTurn.UE.Processors.Calli]
    static extern void GetValue(System.IntPtr source);
}
");

        [Fact]
        public static async void NotProcessor() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
class CalliAttribute : System.Attribute
{
}

static class Enclosing
{
    static class NativeMethods
    {
        [global::Calli]
        static extern void GetValue(System.IntPtr source);
    }
    }
");

        [Fact]
        public static async void MultipleAttributes() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [NextTurn.UE.Processors.Calli]
        [NextTurn.UE.Processors.ReadOffset]
        static extern void GetValue(System.IntPtr source);
    }
}
",
                Verifier.Diagnostic(MethodShouldNotBeAppliedWithMultipleAttributes).WithLocation(8, 28));

        [Fact]
        public static async void ReadOffset_TooFewParameters() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [NextTurn.UE.Processors.ReadOffset]
        static extern void GetValue();
    }
}
",
                Verifier.Diagnostic(MethodShouldTakeOneParameter).WithLocation(7, 28));

        [Fact]
        public static async void ReadOffset_TooManyParameters() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [NextTurn.UE.Processors.ReadOffset]
        static extern void GetValue(System.IntPtr source, int byteOffset);
    }
    }
",
                Verifier.Diagnostic(MethodShouldTakeOneParameter).WithLocation(7, 28));

        [Fact]
        public static async void ReadOffset_NotIntPtrParameter() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [NextTurn.UE.Processors.ReadOffset]
        static extern void GetValue(int source);
    }
}
",
                Verifier.Diagnostic(MethodShouldTakeOneIntPtrParameter).WithLocation(7, 28));

        [Fact]
        public static async void ReadOffset_NoPrefix() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [NextTurn.UE.Processors.ReadOffset]
        static extern void Value(System.IntPtr source);
    }
}
",
                Verifier.Diagnostic(MethodShouldStartWithGet).WithLocation(7, 28));
    }
}
