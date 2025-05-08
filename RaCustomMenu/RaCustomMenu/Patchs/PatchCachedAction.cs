using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenu.Patchs;

[HarmonyPatch(typeof(DummyActionCollector.CachedActions), MethodType.Constructor, new Type[]{typeof(ReferenceHub)})]
public static class PatchCachedAction
{
    [HarmonyPatch, HarmonyPostfix]
    public static void Postfix(DummyActionCollector.CachedActions __instance, ReferenceHub hub)
    {
        if(!hub.IsDummy && !hub.IsHost)
        {
            var field = typeof(DummyActionCollector.CachedActions)
                .GetField("_providers", BindingFlags.Instance | BindingFlags.NonPublic);
            var newProviders = new List<IRootDummyActionProvider>();
            
            newProviders.Add(new ProviderManager(hub));
        
            field.SetValue(__instance, newProviders.ToArray());
        }
    }
}