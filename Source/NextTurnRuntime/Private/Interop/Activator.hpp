// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "Type.hpp"

class LActivator
{
public:
	static void Initialize();

	template <typename T>
	static T CreateInstance(LType Type);
};
