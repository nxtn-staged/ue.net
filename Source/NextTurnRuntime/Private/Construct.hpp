// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

template<typename T>
inline void Construct(T& Destination, typename TMoveSupportTraits<T>::Copy Source)
{
	new(&Destination) T{ Source };
}

template<typename T>
inline void Construct(T& Destination, typename TMoveSupportTraits<T>::Move Source)
{
	new(&Destination) T{ Source };
}

RESTORE_WARNINGS;
