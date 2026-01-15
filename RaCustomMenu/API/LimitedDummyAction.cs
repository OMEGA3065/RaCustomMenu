using CommandSystem;

namespace RaCustomMenu.API;

public readonly struct LimitedDummyAction
{
    public readonly string Name;

    public readonly Action<ICommandSender> Action;

    public LimitedDummyAction(string name, Action<ICommandSender> action)
    {
        Name = name;
        Action = action;
    }
}