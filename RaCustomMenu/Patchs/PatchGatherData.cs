using System;
using HarmonyLib;
using RemoteAdmin.Communication;

namespace RaCustomMenu.Patchs;

[HarmonyPatch(typeof(RaDummyActions), nameof(RaDummyActions.GatherData))]
public static class PatchGatherData
{
    [HarmonyPrefix]
    public static bool Prefix(RaDummyActions __instance)
    {
        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
        {
            if (referenceHub.IsHost)
                continue;
            __instance.AppendDummy(referenceHub);
        }

        return false;
    }
}