using System;
using HarmonyLib;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenuExiled.Patchs;

[HarmonyPatch(typeof(DummyActionCollector), nameof(DummyActionCollector.GetCache))]
public static class PatchGetCache
{
    [HarmonyPatch, HarmonyPrefix]
    public static bool Prefix(ReferenceHub hub, ref DummyActionCollector.CachedActions __result)
    {
        if (hub == null && hub.IsHost)
        {
            throw new ArgumentException("Provided argument is null", "hub");
        }
        if (!DummyActionCollector.CollectionCache.TryGetValue(hub, out var cachedActions))
        {
            cachedActions = new DummyActionCollector.CachedActions(hub);
            DummyActionCollector.CollectionCache[hub] = cachedActions;
        }

        __result = cachedActions;
        return false;
    }
}