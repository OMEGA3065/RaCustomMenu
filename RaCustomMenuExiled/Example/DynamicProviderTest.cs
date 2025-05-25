using System.Collections.Generic;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;
using RaCustomMenuExiled.API;

namespace RaCustomMenuExiled.Example;

public class DynamicProviderTest: Provider
{
    private List<Player> allowPlayer = new List<Player>();
    public override string CategoryName { get; } = "DynamicProvider";
    public override bool IsDirty { get; } = true;
    public override List<DummyAction> AddAction(ReferenceHub hub)
    {
        return new List<DummyAction>()
        {
            new DummyAction("Test", () =>
            {
                allowPlayer.Add(Player.Get(hub));
                Provider.RegisterDynamicProvider("DynamicProviderTest", true, referenceHub => new List<DummyAction>()
                {
                    new DummyAction("DynamicTest", () =>
                    {
                        Log.Info("Test via DynamicProviderTest");
                    })
                }, allowPlayer);
            })
        };
    }
}