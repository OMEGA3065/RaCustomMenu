using System.Collections.Generic;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

public class ProviderLoadout: Provider
{
    public static ProviderLoadout Instance { get; private set; }

    public List<Player> TargetPlayers { get; set; } = new();

    public override string CategoryName { get; } = "Loadout";

    public override bool IsDirty { get; } = true;

    public ProviderLoadout()
    {
        Instance = this;
    }

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

    public override List<Player> TargetPlayer()
    {
        return TargetPlayers;
    }
}