// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "Activator.hpp"

//#include "CoreClr.hpp"
#include "NextTurnNames.hpp"

#define ACTIVATOR_TYPE_NAME "ActivatorHelper"

namespace
{
	template <typename T>
	using CreateInstancePtrType = T (*)(LType Type);

	static CreateInstancePtrType<LObject> CreateInstancePtr = nullptr;
}

void LActivator::Initialize()
{
	//CreateInstancePtr = CoreClr::GetFunctionPointer<CreateInstancePtrType<LObject>>(
	//	LEFTICE_RUNTIME_ASSEMBLY_NAME, LEFTICE_RUNTIME_NAMESPACE NAME_SEPARATOR ACTIVATOR_TYPE_NAME, "CreateInstance");
}

template <typename T>
T LActivator::CreateInstance(LType Type)
{
	return ((CreateInstancePtrType<T>)CreateInstancePtr)(Type);
}
