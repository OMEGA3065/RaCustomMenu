using System;
using System.Collections.Generic;
using HarmonyLib;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenu.Patchs;

[HarmonyPatch(typeof(DummyActionCollector.CachedActions), MethodType.Constructor, new Type[]{typeof(ReferenceHub)})]
public static class PatchCachedAction
{
    private static readonly AccessTools.FieldRef<DummyActionCollector.CachedActions, IRootDummyActionProvider[]> ProvidersFieldRef =
        AccessTools.FieldRefAccess<DummyActionCollector.CachedActions, IRootDummyActionProvider[]>("_providers");
    [HarmonyPatch, HarmonyPostfix]
    public static void Postfix(DummyActionCollector.CachedActions __instance, ReferenceHub hub)
    {
        if(!hub.IsDummy && !hub.IsHost)
        {
            var newProviders = new List<IRootDummyActionProvider>
            {
                new ProviderManager(hub)
            };

            ProvidersFieldRef(__instance) = newProviders.ToArray();
        }
    }
}