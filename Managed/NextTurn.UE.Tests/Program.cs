using Unreal;

namespace NextTurn.UE.Tests
{
    internal class Program
    {
        public static void Main()
        {
            Log.Display("Hello");
            try
            {
                var c = Unreal.Object.CreateNew<NextTurnTestClass>();
                Log.Display(c.ToString()!);
                // _ = System.Diagnostics.Debugger.Launch();
            }
            catch (System.Exception e)
            {
                Log.Warning(e.ToString());
                // throw;
            }
        }
    }
}
