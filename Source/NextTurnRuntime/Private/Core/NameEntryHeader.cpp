// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Private/Exports.hpp"

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include <bitset>

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME NameEntryHeader

TYPE_EXPORT_START

static_assert(sizeof(FNameEntryHeader) == sizeof(uint16_t), "");

#define BIT_MAX(E) std::numeric_limits<decltype(E)>::max()
#define BIT_SIZEOF(E) sizeof(decltype(E)) * CHAR_BIT

#if WITH_CASE_PRESERVING_NAME
constexpr FNameEntryHeader Header
{
	BIT_MAX(FNameEntryHeader::bIsWide),
	BIT_MAX(FNameEntryHeader::Len)
};
#else
constexpr FNameEntryHeader Header
{
	BIT_MAX(FNameEntryHeader::bIsWide),
	BIT_MAX(FNameEntryHeader::LowercaseProbeHash),
	BIT_MAX(FNameEntryHeader::Len)
};
constexpr std::bitset<BIT_SIZEOF(FNameEntryHeader::bIsWide)> ProbeHashBitSet{ Header.LowercaseProbeHash };
#endif
constexpr std::bitset<BIT_SIZEOF(FNameEntryHeader::bIsWide)> bIsWideBitSet{ Header.bIsWide };
constexpr std::bitset<BIT_SIZEOF(FNameEntryHeader::Len)> LenBitSet{ Header.Len };

uint16_t GetIsWideMask()
{
	return ~(~0 << bIsWideBitSet.count());
}

uint16_t GetIsWideOffset()
{
	return 0;
}

uint16_t GetLengthMask()
{
	return ~(~0 << LenBitSet.count());
}

uint16_t GetLengthOffset()
{
#if WITH_CASE_PRESERVING_NAME
	return bIsWideBitSet.count();
#else
	return bIsWideBitSet.count() + ProbeHashBitSet.count();
#endif
}

const FMemberSymbol Members[] =
{
	EXPORT_SYMBOL(IsWide_Mask, GetIsWideMask()),
	EXPORT_SYMBOL(IsWide_Offset, GetIsWideOffset()),
	EXPORT_SYMBOL(Length_Mask, GetLengthMask()),
	EXPORT_SYMBOL(Length_Offset, GetLengthOffset()),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
