// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LObject.hpp"

class LType : public LObject
{
public:
	static void Initialize();
	static LType GetType(const char* TypeName);
};
