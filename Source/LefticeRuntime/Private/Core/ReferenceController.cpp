// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Private/Accessor.hpp"
#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

using SharedPointerInternals::FReferenceControllerBase;
using SharedPointerInternals::FSharedReferencer;

struct FLefticeReferenceController
{
	void* VTablePtr;
	int32_t SharedReferenceCount;
	int32_t WeakReferenceCount;
};

static_assert(sizeof(FLefticeReferenceController) == sizeof(FReferenceControllerBase), "");

static_assert(sizeof(FLefticeReferenceController::SharedReferenceCount) == sizeof(FReferenceControllerBase::SharedReferenceCount), "");
static_assert(sizeof(FLefticeReferenceController::WeakReferenceCount) == sizeof(FReferenceControllerBase::WeakReferenceCount), "");

struct FLefticeSharedReferencer
{
	FLefticeReferenceController* ReferenceController;
};

using FThreadSafeSharedReferencer = FSharedReferencer<ESPMode::ThreadSafe>;
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReferencer, ReferenceController);

static_assert(sizeof(FLefticeSharedReferencer) == sizeof(FThreadSafeSharedReferencer), "");

static_assert(sizeof(FLefticeSharedReferencer::ReferenceController) == SIZEOF(FThreadSafeSharedReferencer, ReferenceController), "");

struct FLefticeSharedReference
{
	void* Target;
	FLefticeSharedReferencer SharedReferenceCount;
};

using FThreadSafeSharedReference = TSharedRef<void*, ESPMode::ThreadSafe>;
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReference, Object);
DEFINE_SIZE_ACCESSOR(FThreadSafeSharedReference, SharedReferenceCount);

static_assert(sizeof(FLefticeSharedReference) == sizeof(FThreadSafeSharedReference), "");

static_assert(sizeof(FLefticeSharedReference::Target) == SIZEOF(FThreadSafeSharedReference, Object), "");
static_assert(sizeof(FLefticeSharedReference::SharedReferenceCount) == SIZEOF(FThreadSafeSharedReference, SharedReferenceCount), "");

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
