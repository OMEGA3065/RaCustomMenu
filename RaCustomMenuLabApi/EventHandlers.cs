using System;
using System.Collections.Generic;
using System.Reflection;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Console;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenuLabApi;

public class EventHandlers
{
    public static void RegisterEvent()
    {
        PlayerEvents.Joined += Verified;
        PlayerEvents.Left += OnLeft;
    }
    
    public static void UnRegisterEvent()
    {
        PlayerEvents.Joined -= Verified;
        PlayerEvents.Left -= OnLeft;
    }
    
    static void Verified(PlayerJoinedEventArgs ev)
    {
        Type targetType = typeof(DummyActionCollector);
        FieldInfo field = targetType.GetField("CollectionCache", BindingFlags.NonPublic | BindingFlags.Static);
        if (field == null)
        {
            Logger.Error("CollectionCache not found");
            return;
        }
        var dict = field.GetValue(null) as Dictionary<ReferenceHub, DummyActionCollector.CachedActions>;
        if (dict != null)
        {
            var newCache = new DummyActionCollector.CachedActions(ev.Player.ReferenceHub);
            dict[ev.Player.ReferenceHub] = newCache;
        }
    }

    static void OnLeft(PlayerLeftEventArgs ev)
    {
        Type targetType = typeof(DummyActionCollector);
        FieldInfo field = targetType.GetField("CollectionCache", BindingFlags.NonPublic | BindingFlags.Static);
        if (field == null)
        {
            Logger.Error("CollectionCache not found");
            return;
        }
        var dict = field.GetValue(null) as Dictionary<ReferenceHub, DummyActionCollector.CachedActions>;
        if (dict != null)
        {
            if (dict.ContainsKey(ev.Player.ReferenceHub))
                dict.Remove(ev.Player.ReferenceHub);
        }
    }
}