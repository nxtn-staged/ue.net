// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "ManagedDelegate.hpp"

#include "NextTurnRuntime/Private/CoreClr.hpp"

ENABLE_WARNINGS;

FGCHandle::FGCHandle(intptr_t InHandle) : Handle{ InHandle }
{
	RESTORE_WARNINGS;
	UE_LOG(LogCore, Error, L"%llx", this->Handle);
	ENABLE_WARNINGS;
}

FGCHandle::~FGCHandle()
{
	RESTORE_WARNINGS;
	UE_LOG(LogCore, Error, L"%llx", this->Handle);
	ENABLE_WARNINGS;
	CoreClr::FreeGCHandle(this->Handle);
}

template<typename WrappedResultType, typename... ParameterTypes>
TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::TManagedDelegateInstance(
	FFunctionPointer InFunctionPointer, intptr_t InGCHandle)
	: FunctionPointer{ InFunctionPointer }
	, GCHandle{ new FGCHandle{ InGCHandle } }
	, Handle{ FDelegateHandle::GenerateNewHandle }
{
}

#if USE_DELEGATE_TRYGETBOUNDFUNCTIONNAME
template<typename WrappedResultType, typename... ParameterTypes>
FName TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::TryGetBoundFunctionName() const
{
	return NAME_None;
}
#endif

template<typename WrappedResultType, typename... ParameterTypes>
UObject* TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::GetUObject() const
{
	return nullptr;
}

template<typename WrappedResultType, typename... ParameterTypes>
const void* TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::GetObjectForTimerManager() const
{
	return nullptr;
}

template<typename WrappedResultType, typename... ParameterTypes>
uint64 TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::GetBoundProgramCounterForTimerManager() const
{
	return reinterpret_cast<const UPTRINT&>(this->FunctionPointer);
}

template<typename WrappedResultType, typename... ParameterTypes>
bool TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::HasSameObject(const void*) const
{
	return false;
}

template<typename WrappedResultType, typename... ParameterTypes>
bool TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::IsSafeToExecute() const
{
	return true;
}

template<typename WrappedResultType, typename... ParameterTypes>
FDelegateHandle TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::GetHandle() const
{
	return this->Handle;
}

template<typename WrappedResultType, typename... ParameterTypes>
void TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::CreateCopy(FDelegateBase& Base)
{
	new(Base) UnwrappedThisType{ *(UnwrappedThisType*)this };
}

template<typename WrappedResultType, typename... ParameterTypes>
typename TUnwrapType<WrappedResultType>::Type TManagedDelegateInstance<WrappedResultType, ParameterTypes...>::Execute(ParameterTypes... Arguments) const
{
	return this->FunctionPointer(Arguments...);
}

template<typename... ParameterTypes>
TManagedDelegateInstance<void, ParameterTypes...>::TManagedDelegateInstance(
	typename TManagedDelegateInstance<TTypeWrapper<void>, ParameterTypes...>::FFunctionPointer InFunctionPointer, intptr_t InGCHandle)
	: TManagedDelegateInstance<TTypeWrapper<void>, ParameterTypes...>{ InFunctionPointer, InGCHandle }
{
}

template<typename... ParameterTypes>
bool TManagedDelegateInstance<void, ParameterTypes...>::ExecuteIfSafe(ParameterTypes... Arguments) const
{
	this->Execute(Arguments...);
	return true;
}

template<typename ResultType, typename... ParameterTypes>
TBaseDelegate<ResultType, ParameterTypes...> TManagedDelegate<TBaseDelegate<ResultType, ParameterTypes...>>::Create(
	typename TBaseDelegate<ResultType, ParameterTypes...>::FStaticDelegate::FFuncPtr FunctionPointer, intptr_t GCHandle)
{
	TBaseDelegate<ResultType, ParameterTypes...> Result;
	new(Result) typename TManagedDelegateInstance<ResultType, ParameterTypes...>::UnwrappedThisType{ FunctionPointer, GCHandle };
	return Result;
}

RESTORE_WARNINGS;
