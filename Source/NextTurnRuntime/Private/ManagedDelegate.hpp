// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

class FGCHandle
{
public:
	FGCHandle(intptr_t InHandle);
	~FGCHandle();

private:
	intptr_t Handle;
};

template<typename WrappedResultType, typename... ParameterTypes>
class TManagedDelegateInstance : public IBaseDelegateInstance<typename TUnwrapType<WrappedResultType>::Type (ParameterTypes...)>
{
public:
	using ResultType = typename TUnwrapType<WrappedResultType>::Type;

public:
	using UnwrappedThisType = TManagedDelegateInstance<ResultType, ParameterTypes...>;
	using FFunctionPointer = ResultType (*)(ParameterTypes...);

	TManagedDelegateInstance(FFunctionPointer InFunctionPointer, intptr_t InGCHandle);

	// IDelegateInstance

#if USE_DELEGATE_TRYGETBOUNDFUNCTIONNAME
	virtual FName TryGetBoundFunctionName() const override final;
#endif
	virtual UObject* GetUObject() const override final;
	virtual const void* GetObjectForTimerManager() const override final;
	virtual uint64 GetBoundProgramCounterForTimerManager() const override final;
	virtual bool HasSameObject(const void*) const override final;
	virtual bool IsSafeToExecute() const override final;
	virtual FDelegateHandle GetHandle() const override final;

	// IBaseDelegateInstanceCommon

	virtual void CreateCopy(FDelegateBase& Base) override final;
	inline virtual ResultType Execute(ParameterTypes... Arguments) const override final;

private:
	FFunctionPointer FunctionPointer;
	TSharedRef<FGCHandle> GCHandle;
	FDelegateHandle Handle;
};

template<typename... ParameterTypes>
class TManagedDelegateInstance<void, ParameterTypes...> final : public TManagedDelegateInstance<TTypeWrapper<void>, ParameterTypes...>
{
	using SuperType = TManagedDelegateInstance<TTypeWrapper<void>, ParameterTypes...>;

public:
	TManagedDelegateInstance(typename SuperType::FFunctionPointer InFunctionPointer, intptr_t InGCHandle);

	// IBaseDelegateInstanceCommon

	virtual bool ExecuteIfSafe(ParameterTypes... Arguments) const override;
};

template<typename DelegateType>
struct TManagedDelegate;

template<typename ResultType, typename... ParameterTypes>
struct TManagedDelegate<TBaseDelegate<ResultType, ParameterTypes...>>
{
	static TBaseDelegate<ResultType, ParameterTypes...> Create(
		typename TBaseDelegate<ResultType, ParameterTypes...>::FStaticDelegate::FFuncPtr FunctionPointer, intptr_t GCHandle);
};

RESTORE_WARNINGS;
