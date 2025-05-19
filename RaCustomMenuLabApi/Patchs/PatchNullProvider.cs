using System;
using HarmonyLib;
using NetworkManagerUtils.Dummies;
using PlayerRoles;

namespace RaCustomMenuLabApi.Patchs;

[HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.PopulateDummyActions))]
public class PatchNullProvider
{
    [HarmonyPatch,HarmonyPrefix]
    public static bool Prefix(PlayerRoleManager __instance, Action<DummyAction> actionAdder, Action<string> categoryAdder)
    {
        if (__instance._dummyProviders is null)
            return false;
        return true;
    }
}