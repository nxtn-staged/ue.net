// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Private/Accessor.hpp"
#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

using SharedPointerInternals::FReferenceControllerBase;
using SharedPointerInternals::FSharedReferencer;

struct FNextTurnReferenceController
{
	void* VTablePtr;
	int32_t SharedReferenceCount;
	int32_t WeakReferenceCount;
};

static_assert(sizeof(FNextTurnReferenceController) == sizeof(FReferenceControllerBase), "");

static_assert(sizeof(FNextTurnReferenceController::SharedReferenceCount) == sizeof(FReferenceControllerBase::SharedReferenceCount), "");
static_assert(sizeof(FNextTurnReferenceController::WeakReferenceCount) == sizeof(FReferenceControllerBase::WeakReferenceCount), "");

struct FNextTurnSharedReferencer
{
	FNextTurnReferenceController* ReferenceController;
};

using FThreadSafeSharedReferencer = FSharedReferencer<ESPMode::ThreadSafe>;
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReferencer, ReferenceController);

static_assert(sizeof(FNextTurnSharedReferencer) == sizeof(FThreadSafeSharedReferencer), "");

static_assert(sizeof(FNextTurnSharedReferencer::ReferenceController) == SIZEOF(FThreadSafeSharedReferencer, ReferenceController), "");

struct FNextTurnSharedReference
{
	void* Target;
	FNextTurnSharedReferencer SharedReferenceCount;
};

using FThreadSafeSharedReference = TSharedRef<void*, ESPMode::ThreadSafe>;
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReference, Object);
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReference, SharedReferenceCount);

static_assert(sizeof(FNextTurnSharedReference) == sizeof(FThreadSafeSharedReference), "");

static_assert(sizeof(FNextTurnSharedReference::Target) == SIZEOF(FThreadSafeSharedReference, Object), "");
static_assert(sizeof(FNextTurnSharedReference::SharedReferenceCount) == SIZEOF(FThreadSafeSharedReference, SharedReferenceCount), "");

#define TYPE_EXPORT_MANAGED_TYPE_NAME ReferenceController

TYPE_EXPORT_START

void EXPORT_CALL_CONV DeleteReferenceController(FReferenceControllerBase* ReferenceController)
{
	delete ReferenceController;
}

void EXPORT_CALL_CONV FinalizeTarget(FReferenceControllerBase* ReferenceController)
{
	ReferenceController->DestroyObject();
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(DeleteReferenceController),
	EXPORT_METHOD(FinalizeTarget),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
