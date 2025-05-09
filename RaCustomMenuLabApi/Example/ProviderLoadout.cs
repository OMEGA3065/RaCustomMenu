using System.Collections.Generic;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenuLabApi.API;

namespace RaCustomMenuLabApi.Example;

public class ProviderLoadout: Provider
{
    public override string CategoryName { get; } = "Loadout";

    public override bool IsDirty { get; } = true;

    public override List<DummyAction> AddAction(ReferenceHub hub)
    {
        return new List<DummyAction>()
        {
            new DummyAction("Give Loadout", () =>
            {
                Player pl = Player.Get(hub);
                pl.AddItem(ItemType.GunFRMG0);
                pl.AddItem(ItemType.Adrenaline);
            })
        };
    }
}