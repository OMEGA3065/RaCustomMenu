using System.Collections.Generic;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

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
                pl.AddItem(ItemType.Adrenaline);
                pl.AddItem(ItemType.GunFRMG0);
            })
        };
    }
}