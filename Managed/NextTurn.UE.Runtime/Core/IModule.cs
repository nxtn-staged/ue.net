// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

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
