// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

using Xunit;
using Verifier = Leftice.Analyzers.Testing.AnalyzerVerifier<Leftice.Analyzers.Analyzer>;

namespace Leftice.Analyzers.Tests
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
                Verifier.Diagnostic().WithLocation(4, 11).WithArguments("Class should be static"));

        [Fact]
        public static async void NotNested() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class NativeMethods
{
}
",
                Verifier.Diagnostic().WithLocation(2, 14).WithArguments("Class should be nested"));

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
                Verifier.Diagnostic().WithLocation(6, 22).WithArguments("Class should not contain nested types"));

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
                Verifier.Diagnostic().WithLocation(6, 16).WithArguments("Class should not contain constructors"));

        [Fact]
        public static async void NotContainedInNativeMethodsClass() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
struct NativeMethods
{
    [Leftice.Processors.Calli]
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
        [Leftice.Processors.Calli]
        [Leftice.Processors.ReadOffset]
        static extern void GetValue(System.IntPtr source);
    }
}
",
                Verifier.Diagnostic().WithLocation(8, 28).WithArguments("Method should not be applied with multiple processor attributes"));

        [Fact]
        public static async void ReadOffset_TooFewParameters() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [Leftice.Processors.ReadOffset]
        static extern void GetValue();
    }
}
",
                Verifier.Diagnostic().WithLocation(7, 28).WithArguments("Method should take one parameter"));

        [Fact]
        public static async void ReadOffset_TooManyParameters() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [Leftice.Processors.ReadOffset]
        static extern void GetValue(System.IntPtr source, int byteOffset);
    }
    }
",
                Verifier.Diagnostic().WithLocation(7, 28).WithArguments("Method should take one parameter"));

        [Fact]
        public static async void ReadOffset_NotIntPtrParameter() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [Leftice.Processors.ReadOffset]
        static extern void GetValue(int source);
    }
}
",
                Verifier.Diagnostic().WithLocation(7, 28).WithArguments($"Method should take one '{typeof(System.IntPtr).FullName}' parameter"));

        [Fact]
        public static async void ReadOffset_NoPrefix() =>
            await Verifier.VerifyAnalyzerAsync(
                @"
static class Enclosing
{
    static class NativeMethods
    {
        [Leftice.Processors.ReadOffset]
        static extern void Value(System.IntPtr source);
    }
}
",
                Verifier.Diagnostic().WithLocation(7, 28).WithArguments("Method should start with 'Get'"));
    }
}
