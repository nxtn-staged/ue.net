// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "LObject.hpp"

//#include "CoreClr.hpp"
#include "NextTurnNames.hpp"

#define GCHANDLE_TYPE_NAME "GCHandleHelper"

namespace
{
	using FreePtrType = void (*)(LObject Value);

	static FreePtrType FreePtr = nullptr;
}

void LObject::Initialize()
{
	//FreePtr = CoreClr::GetFunctionPointer<FreePtrType>(
	//	LEFTICE_RUNTIME_ASSEMBLY_NAME, LEFTICE_RUNTIME_NAMESPACE NAME_SEPARATOR GCHANDLE_TYPE_NAME, "Free");
}

void LObject::Free()
{
	FreePtr(*this);
	Handle = nullptr;
}
