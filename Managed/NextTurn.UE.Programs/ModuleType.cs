// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

namespace NextTurn.UE.Programs
{
    // NOTE: Keep in sync with ENextTurnModuleType.
    internal enum ModuleType
    {
        Program,
        EngineRuntime,
        EngineUncooked,
        EngineDeveloper,
        EngineEditor,
        EngineThirdParty,
        ProjectRuntime,
        ProjectUncooked,
        ProjectDeveloper,
        ProjectEditor,
        ProjectThirdParty,
    }
}
