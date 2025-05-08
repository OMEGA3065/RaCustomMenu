using System.Reflection;
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
            if(referenceHub.IsHost)
                continue;
            MethodInfo method = typeof(RaDummyActions).GetMethod("AppendDummy", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(__instance, new object[] { referenceHub });
        }
        return false;
    }
}