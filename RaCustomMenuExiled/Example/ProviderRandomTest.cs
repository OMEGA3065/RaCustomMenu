using System.Collections.Generic;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;
using RaCustomMenuExiled.API;

namespace RaCustomMenuExiled.Example;

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
                Log.Info($"Test {hub.nicknameSync.DisplayName}");
            }),
            new DummyAction($"Test {num}", () =>
            {
                num++;
            })
        };
    }

    public override string CategoryName { get; } = "Test Module";
}