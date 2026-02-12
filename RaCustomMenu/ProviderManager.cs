using System;
using System.Collections.Generic;
using System.ComponentModel;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu;

public class ProviderManager(ReferenceHub hub) : IRootDummyActionProvider
{
    private readonly List<Provider> AllProviders = Provider.providersLoaded;

    public void PopulateDummyActions(Action<DummyAction> actionAdder, Action<string> categoryAdder)
    {
        DummyActionsDirty = false;
        foreach (Provider provider in AllProviders)
        {
            if (!provider.IsAllowedOnPlayer(hub)) continue;
            categoryAdder(provider.CategoryName);
            DebugLogger.Log($"Provider Category name {provider.CategoryName} added");

            var actions = provider.GetActionList(hub);
            foreach (DummyAction dummyAction in actions)
            {
                DebugLogger.Log("Action name : " + dummyAction.Name);
                actionAdder(dummyAction);
            }

            DummyActionsDirty |= provider.IsDirty;
        }
    }

    public bool DummyActionsDirty { get; set; }
}