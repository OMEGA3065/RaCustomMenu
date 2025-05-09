using System;
using System.Collections.Generic;
using System.Reflection;
using Exiled.Events.EventArgs.Player;
using NetworkManagerUtils.Dummies;
using Log = Exiled.API.Features.Log;

namespace RaCustomMenuExiled;

public class EventHandlers
{
    public static void RegisterEvent()
    {
        Exiled.Events.Handlers.Player.Verified += Verified;
        Exiled.Events.Handlers.Player.Left += OnLeft;
    }
    
    public static void UnRegisterEvent()
    {
        Exiled.Events.Handlers.Player.Verified -= Verified;
        Exiled.Events.Handlers.Player.Left -= OnLeft;
    }
    
    static void Verified(VerifiedEventArgs ev)
    {
        Type targetType = typeof(DummyActionCollector);
        FieldInfo field = targetType.GetField("CollectionCache", BindingFlags.NonPublic | BindingFlags.Static);
        if (field == null)
        {
            Log.Error("Field not found.");
            return;
        }
        var dict = field.GetValue(null) as Dictionary<ReferenceHub, DummyActionCollector.CachedActions>;
        if (dict != null)
        {
            var newCache = new DummyActionCollector.CachedActions(ev.Player.ReferenceHub);
            dict[ev.Player.ReferenceHub] = newCache;
        }
    }

    static void OnLeft(LeftEventArgs ev)
    {
        Type targetType = typeof(DummyActionCollector);
        FieldInfo field = targetType.GetField("CollectionCache", BindingFlags.NonPublic | BindingFlags.Static);
        if (field == null)
        {
            Log.Error("Field not found.");
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