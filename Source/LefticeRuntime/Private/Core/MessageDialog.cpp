// Copyright (c) NextTurn.
// See LICENSE.txt in the project root for more information.

#include "LefticeRuntime/Private/Exports.hpp"

#include "LefticeRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/Misc/MessageDialog.h"

ENABLE_WARNINGS;

#define TYPE_EXPORT_MANAGED_TYPE_NAME MessageDialog

TYPE_EXPORT_START

EAppReturnType::Type EXPORT_CALL_CONV Show(const FText& Content, const FText* Title, EAppMsgType::Type Button)
{
	return FMessageDialog::Open(Button, Content, Title);
}

EAppReturnType::Type EXPORT_CALL_CONV ShowWithDefaultResult(const FText& Content, const FText* Title, EAppMsgType::Type Button, EAppReturnType::Type DefaultResult)
{
	return FMessageDialog::Open(Button, DefaultResult, Content, Title);
}

const FMemberSymbol Members[] =
{
	EXPORT_METHOD(Show),
	EXPORT_METHOD(ShowWithDefaultResult),
};

TYPE_EXPORT_END

#undef TYPE_EXPORT_MANAGED_TYPE_NAME

RESTORE_WARNINGS;
