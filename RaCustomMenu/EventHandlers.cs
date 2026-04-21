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
        var newCache = new DummyActionCollector.CachedActions(ev.Player.ReferenceHub);
        DummyActionCollector.CollectionCache[ev.Player.ReferenceHub] = newCache;
    }

    static void OnLeft(PlayerLeftEventArgs ev)
    {
        DummyActionCollector.CollectionCache.Remove(ev.Player.ReferenceHub);
    }
}