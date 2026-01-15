using System.Collections.Generic;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

public class ProviderRandomTest : Provider
{
    private int num = 0;

    public override bool IsDirty { get; } = true;

    public override List<LimitedDummyAction> AddActions(ReferenceHub hub)
    {
        return
        [
            new("Test", (sender) =>
            {
                Logger.Info($"Test {hub.nicknameSync.DisplayName}");
            }),
            new($"Test {num}", (sender) =>
            {
                num++;
            }),
            new($"Test Add Target Loadout", (sender) =>
            {
                var player = Player.Get(hub);
                if (!ProviderLoadout.Instance.TargetPlayers.Contains(player))
                    ProviderLoadout.Instance.TargetPlayers.Add(player);
            })
        ];
    }

    public override string CategoryName { get; } = "Test Module";
}