// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

class LObject
{
public:
	static void Initialize();

	void Free();

private:
	LObject() = delete;

	void* Handle;
};
