// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#pragma once

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

ENABLE_WARNINGS;

template<typename NameType, typename Type, typename MemberType, MemberType Type::* MemberPointer>
struct TOffsetOf
{
	friend size_t OffsetOf(NameType)
	{
		return reinterpret_cast<size_t>(std::addressof(static_cast<Type*>(nullptr)->*MemberPointer));
	}
};

#define DEFINE_OFFSET_ACCESSOR(Type, Member) \
	struct FOffsetOf_##Type##_##Member {}; \
	template struct TOffsetOf<FOffsetOf_##Type##_##Member, Type, decltype(Type::Member), &Type::Member>; \
	size_t OffsetOf(FOffsetOf_##Type##_##Member);

#define OFFSETOF(Type, Member) OffsetOf(FOffsetOf_##Type##_##Member{})

template<typename NameType, typename Type, typename MemberType, MemberType Type::* MemberPointer>
struct TSizeOf
{
	constexpr friend size_t SizeOf(NameType)
	{
		return sizeof(std::declval<Type>().*MemberPointer);
	}
};

#define DEFINE_SIZE_ACCESSOR(Type, Member) \
	struct FSizeOf_##Type##_##Member {}; \
	template struct TSizeOf<FSizeOf_##Type##_##Member, Type, decltype(Type::Member), &Type::Member>; \
	constexpr size_t SizeOf(FSizeOf_##Type##_##Member);

#define SIZEOF(Type, Member) SizeOf(FSizeOf_##Type##_##Member{})

template<typename NameType, typename Type, typename MemberType, MemberType MemberPointer, typename ResultType, typename... ParameterTypes>
struct TMethod
{
	friend ResultType Invoke(NameType, Type& Value, ParameterTypes... Parameters)
	{
		return (Value.*MemberPointer)(Parameters...);
	}
};

#define DEFINE_METHOD_ACCESSOR(Type, Member, ResultType, ...) \
	struct FMethod_##Type##_##Member {}; \
	template struct TMethod<FMethod_##Type##_##Member, Type, ResultType (Type::*)(__VA_ARGS__), &Type::Member, ResultType, __VA_ARGS__>; \
	ResultType Invoke(FMethod_##Type##_##Member, Type& Value, __VA_ARGS__);

#define INVOKE(Type, Member, Value, ...) Invoke(FMethod_##Type##_##Member{}, Value, __VA_ARGS__)

RESTORE_WARNINGS;
