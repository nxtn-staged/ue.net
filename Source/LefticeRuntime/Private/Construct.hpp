// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

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
