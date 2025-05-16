using System;
using System.Collections.Generic;
using LabApi.Features.Console;
using NetworkManagerUtils.Dummies;
using RaCustomMenuLabApi.API;

namespace RaCustomMenuLabApi;

public class ProviderManager(ReferenceHub hub): IRootDummyActionProvider
{
    private List<Provider> Allproviders = Provider.providersLoaded;
    
    public void PopulateDummyActions(Action<DummyAction> actionAdder, Action<string> categoryAdder)
    {
        foreach (Provider provider in Allproviders)
        {
            categoryAdder(provider.CategoryName);
            Logger.Debug($"Provider Category name {provider.CategoryName} added");
            List<DummyAction> actions = provider.AddAction(hub);
            foreach (DummyAction dummyAction in actions)
            {
                Logger.Debug("Action name : "+dummyAction.Name);
                actionAdder(dummyAction);
            }
            DummyActionsDirty = provider.IsDirty;
        }
    }

    public bool DummyActionsDirty { get; set; }
}