using System.Threading;
using Xunit.Runners;

namespace NextTurn.UE.Testing
{
    internal class Program
    {
        private static readonly ManualResetEvent complete = new ManualResetEvent(false);

        internal static void Main(string[] args)
        {
            _ = System.Reflection.Assembly.LoadFrom(@"C:\Users\Liim\Documents\Unreal Projects\MyProject\Plugins\UE.NET\Managed\NextTurn.UE.Runtime.Tests\bin\Debug\net5.0\NextTurn.UE.Runtime.Tests.dll");

            using var runner = AssemblyRunner.WithoutAppDomain(@"C:\Users\Liim\Documents\Unreal Projects\MyProject\Plugins\UE.NET\Managed\NextTurn.UE.Runtime.Tests\bin\Debug\net5.0\NextTurn.UE.Runtime.Tests.dll");

            runner.OnDiscoveryComplete = OnDiscoveryComplete;
            runner.OnExecutionComplete = OnExecutionComplete;
            runner.OnTestFailed = OnTestFailed;
            runner.OnTestPassed = OnTestPassed;
            runner.OnTestSkipped = OnTestSkipped;
            runner.OnTestStarting = OnTestStarting;

            runner.Start();

            _ = complete.WaitOne();
            complete.Dispose();
        }

        private static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
        {
        }

        private static void OnExecutionComplete(ExecutionCompleteInfo info)
        {
            _ = complete.Set();
        }

        private static void OnTestFailed(TestFailedInfo info)
        {
        }

        private static void OnTestPassed(TestPassedInfo info)
        {
        }

        private static void OnTestSkipped(TestSkippedInfo info)
        {
        }

        private static void OnTestStarting(TestStartingInfo info)
        {
        }
    }
}
