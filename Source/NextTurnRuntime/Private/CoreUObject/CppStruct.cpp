// // Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// // See LICENSE.txt in the project root for more information.

// #include "NextTurnRuntime/Private/Exports.hpp"

// #include "NextTurnRuntime/Private/Accessor.hpp"
// #include "NextTurnRuntime/Public/Compiler.hpp"

// #include "Runtime/Core/Public/CoreMinimal.h"

// #include "Runtime/CoreUObject/Public/UObject/Class.h"

// ENABLE_WARNINGS;

// using FCppStruct = UScriptStruct::ICppStructOps;
// DEFINE_OFFSET_ACCESSOR(FCppStruct, Size);

// #define TYPE_EXPORT_MANAGED_TYPE_NAME CppStruct

// TYPE_EXPORT_START

// bool EXPORT_CALL_CONV IsTriviallyDefaultConstructible(FCppStruct& CppStruct)
// {
// 	return CppStruct.HasZeroConstructor();
// }

// bool EXPORT_CALL_CONV IsTriviallyDestructible(FCppStruct& CppStruct)
// {
// 	return !CppStruct.HasDestructor();
// }

// const FMemberSymbol Members[] =
// {
// 	EXPORT_OFFSET(FCppStruct, Size, Size),

// 	EXPORT_METHOD(IsTriviallyDefaultConstructible),
// 	EXPORT_METHOD(IsTriviallyDestructible),
// };

// TYPE_EXPORT_END

// #undef TYPE_EXPORT_MANAGED_TYPE_NAME

// RESTORE_WARNINGS;
