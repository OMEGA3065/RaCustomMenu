using System;
using HarmonyLib;
using RemoteAdmin.Communication;

namespace RaCustomMenuExiled.Patchs;

[HarmonyPatch(typeof(RaDummyActions), nameof(RaDummyActions.GatherData))]
public static class PatchGatherData
{
    private static readonly Action<RaDummyActions, ReferenceHub> AppendDummyDelegate =
        AccessTools.MethodDelegate<Action<RaDummyActions, ReferenceHub>>(
            AccessTools.Method(typeof(RaDummyActions), "AppendDummy"));

    [HarmonyPrefix]
    public static bool Prefix(RaDummyActions __instance)
    {
        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
        {
            if (referenceHub.IsHost)
                continue;
            AppendDummyDelegate(__instance, referenceHub);
        }

        return false;
    }
}