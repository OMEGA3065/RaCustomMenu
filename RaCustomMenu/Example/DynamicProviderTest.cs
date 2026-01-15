using System.Collections.Generic;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

public class DynamicProviderTest : Provider
{
    private List<Player> allowPlayer = new List<Player>();
    public override string CategoryName { get; } = "DynamicProvider";
    public override bool IsDirty { get; } = true;
    public override List<LimitedDummyAction> AddActions(ReferenceHub hub)
    {
        return
        [
            new("Test", (sender) =>
            {
                allowPlayer.Add(Player.Get(hub));
                Provider.RegisterDynamicProvider("DynamicProviderTest", true, referenceHub => [
                    new("DynamicTest", (sender) =>
                    {
                        Logger.Info("Test via DynamicProviderTest");
                    })
                ]);
            })
        ];
    }
}