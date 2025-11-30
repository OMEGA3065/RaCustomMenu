using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenu;

public class EventHandlers
{
    private static readonly FieldInfo CollectionCache = typeof(DummyActionCollector).GetField("CollectionCache", BindingFlags.NonPublic | BindingFlags.Static);
    public static void RegisterEvents()
    {
        PlayerEvents.Joined += OnVerified;
        PlayerEvents.Left += OnLeft;
    }
    
    public static void UnRegisterEvents()
    {
        PlayerEvents.Joined -= OnVerified;
        PlayerEvents.Left -= OnLeft;
    }
    
    static void OnVerified(PlayerJoinedEventArgs ev)
    {
        if (CollectionCache == null)
        {
            Logger.Error($"CollectionCache not found!\n Stacktrace: {new StackTrace()}");
            return;
        }
        if (CollectionCache.GetValue(null) is Dictionary<ReferenceHub, DummyActionCollector.CachedActions> dict)
        {
            var newCache = new DummyActionCollector.CachedActions(ev.Player.ReferenceHub);
            dict[ev.Player.ReferenceHub] = newCache;
        }
    }

    static void OnLeft(PlayerLeftEventArgs ev)
    {
        if (CollectionCache == null)
        {
            Logger.Error($"CollectionCache not found!\n Stacktrace: {new StackTrace()}");
            return;
        }
        if (CollectionCache.GetValue(null) is Dictionary<ReferenceHub, DummyActionCollector.CachedActions> dict)
        {
            if (dict.ContainsKey(ev.Player.ReferenceHub))
                dict.Remove(ev.Player.ReferenceHub);
        }
    }
}