using System.Collections.Generic;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

public class ProviderRandomTest: Provider
{
    private int num = 0;

    public override bool IsDirty { get;} = true;

    public override List<DummyAction> AddAction(ReferenceHub hub)
    {
        return new List<DummyAction>()
        {
            new DummyAction("Test", () =>
            {
                Logger.Info($"Test {hub.nicknameSync.DisplayName}");
            }),
            new DummyAction($"Test {num}", () =>
            {
                num++;
            }),
            new DummyAction($"Test Add Target Loadout", () =>
            {
                var player = Player.Get(hub);
                if (!ProviderLoadout.Instance.TargetPlayers.Contains(player))
                    ProviderLoadout.Instance.TargetPlayers.Add(player);
            })
        };
    }

    public override string CategoryName { get; } = "Test Module";
}