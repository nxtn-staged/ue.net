// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "Type.hpp"

//#include "CoreClr.hpp"
#include "NextTurnNames.hpp"

#define TYPE_TYPE_NAME "TypeHelper"

namespace
{
	using GetTypePtrType = LType (*)(const char* TypeName);

	static GetTypePtrType GetTypePtr = nullptr;
}

void LType::Initialize()
{
	//GetTypePtr = CoreClr::GetFunctionPointer<GetTypePtrType>(
	//	LEFTICE_RUNTIME_ASSEMBLY_NAME, LEFTICE_RUNTIME_NAMESPACE NAME_SEPARATOR TYPE_TYPE_NAME, "GetType");
}

LType LType::GetType(const char* TypeName)
{
	return GetTypePtr(TypeName);
}
