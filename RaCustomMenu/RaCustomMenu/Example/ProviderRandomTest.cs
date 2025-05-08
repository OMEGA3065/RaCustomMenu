using System.Collections.Generic;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;
using RaCustomMenu.API;

namespace RaCustomMenu.Example;

public class ProviderRandomTest: Provider
{
    private int num = 0;
    
    public override List<DummyAction> AddAction(ReferenceHub hub)
    {
        return new List<DummyAction>()
        {
            new DummyAction("Test", () =>
            {
                Log.Info($"Test {hub.nicknameSync.DisplayName}");
            }),
            new DummyAction($"Test {num}", () =>
            {
                num++;
            })
        };
    }

    public override string CategoryName { get; set; } = "Test Module";
}