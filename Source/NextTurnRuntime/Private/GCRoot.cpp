// Copyright (c) NextTurn. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// See LICENSE.txt in the project root for more information.

#include "NextTurnRuntime/Public/Compiler.hpp"

#include "Runtime/Core/Public/CoreMinimal.h"

#include <functional>
#include <list>
#include <set>
#include <utility>

ENABLE_WARNINGS;

namespace GCRoot
{
	class FGCRootReferenceCollector final : public FReferenceCollector
	{
	public:
		FGCRootReferenceCollector(std::function<void (UObject*)> Callback)
			: Callback{ Callback }
		{
		}

		virtual bool IsIgnoringArchetypeRef() const override
		{
			return false;
		}

		virtual bool IsIgnoringTransient() const override
		{
			return false;
		}

		virtual void HandleObjectReference(UObject*& InObject, const UObject* /*InReferencingObject*/, const FProperty* /*InReferencingProperty*/) override
		{
			this->Callback(InObject);
		}

	private:
		FGCRootReferenceCollector(const FGCRootReferenceCollector&) = delete;
		FGCRootReferenceCollector& operator=(const FGCRootReferenceCollector&) = delete;

		const std::function<void (UObject*)> Callback;
	};

	void PathTo(UObject* Target)
	{
		check(Target != nullptr);

		std::list<std::pair<UObject*, std::list<UObject*>>> path{};

		std::set<UObject*> considered{};

		auto get_refs = [&](UObject* object, bool& found) -> std::list<UObject*>
		{
			std::list<UObject*> refs{};

			auto callback = [&](UObject* reference)
			{
				if (reference == nullptr)
				{
					return;
				}

				if (considered.find(reference) != considered.end())
				{
					return;
				}

				refs.push_back(reference);

				if (reference == Target)
				{
					found = true;
				}
			};

			FGCRootReferenceCollector collector{ callback };
			object->CallAddReferencedObjects(collector);

			return refs;
		};

		for (int32 index = 0; index < GUObjectArray./*GetObjectArrayNumPermanent()*/GetObjectArrayNum(); index++)
		{
			UObject* source = static_cast<UObject*>(GUObjectArray.IndexToObject(index)->Object);

			if (source == nullptr)
			{
				continue;
			}

			if (!considered.insert(source).second)
			{
				continue;
			}

			if (source == Target)
			{
				path.push_back({ Target, {} });
				UE_DEBUG_BREAK();
				return;
			}

			bool found = false;
			path.push_back({ source, get_refs(source, found) });

			if (found)
			{
				path.push_back({ Target, {} });
				UE_DEBUG_BREAK();
				return;
			}

			while (!path.empty())
			{
				std::list<UObject*>& refs = path.back().second;
				if (refs.empty())
				{
					path.pop_back();
					continue;
				}

				do
				{
					UObject* next = refs.front();
					refs.pop_front();

					if (!considered.insert(next).second)
					{
						continue;
					}

					path.push_back({ next, get_refs(next, found) });
					{
						FString s{};
						for (auto& n : path)
						{
							s.AppendInt(GUObjectArray.ObjectToIndex(n.first));
							s.Append("\t");
							s.Append(n.first->GetName());
							s.Append("\t");
						}
						RESTORE_WARNINGS;
						UE_LOG(LogCore, Log, L"%s", *s);
						ENABLE_WARNINGS;
					}

					if (found)
					{
						path.push_back({ Target, {} });
						UE_DEBUG_BREAK();
						return;
					}

					break;
				} while (!refs.empty());
			}
		}
	}
}

RESTORE_WARNINGS;
