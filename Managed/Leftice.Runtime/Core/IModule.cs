// Copyright (c) NextTurn.
// See the LICENSE.TXT file in the project root for more information.

namespace Unreal
{
    public interface IModule
    {
        bool SupportsAutomaticShutdown => true;

        bool SupportsDynamicReloading => true;

        void OnPostLoad()
        {
        }

        void OnPreUnload()
        {
        }

        void ShutDown()
        {
        }

        void StartUp()
        {
        }
    }
}
