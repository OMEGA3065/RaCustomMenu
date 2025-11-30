using LabApi.Features.Console;

namespace RaCustomMenu;

public static class DebugLogger
{
    public static void Log(object message)
    {
        if (RaCustomMenuPlugin.Instance?.Config?.Debug != true)
            return;
        Logger.Debug(message);
    }
}