using System;
using System.Collections.Generic;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;
using RaCustomMenuExiled.API;

namespace RaCustomMenuExiled;

public class ProviderManager(ReferenceHub hub): IRootDummyActionProvider
{
    private List<Provider> Allproviders = Provider.providersLoaded;
    
    public void PopulateDummyActions(Action<DummyAction> actionAdder, Action<string> categoryAdder)
    {
        foreach (Provider provider in Allproviders)
        {
            List<Player> targets = provider.TargetPlayer();
            if (targets == null || targets.Contains(Player.Get(hub)))
            {
                categoryAdder(provider.CategoryName);
                Log.Debug($"Provider Category name {provider.CategoryName} added");

                List<DummyAction> actions = provider.AddAction(hub);
                foreach (DummyAction dummyAction in actions)
                {
                    Log.Debug("Action name : " + dummyAction.Name);
                    actionAdder(dummyAction);
                }

                DummyActionsDirty = provider.IsDirty;
            }
        }
    }

    public bool DummyActionsDirty { get; set; }
}