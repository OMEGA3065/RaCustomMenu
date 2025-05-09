using System.Collections.Generic;
using LabApi.Features.Console;
using NetworkManagerUtils.Dummies;
using RaCustomMenuLabApi.API;

namespace RaCustomMenuLabApi.Example;

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
            })
        };
    }

    public override string CategoryName { get; } = "Test Module";
}