using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Exiled.API.Features;
using HarmonyLib;
using NetworkManagerUtils.Dummies;
using RemoteAdmin;
using RemoteAdmin.Communication;

namespace RaCustomMenu.Patchs;

[HarmonyPatch(typeof(RaDummyActions), nameof(RaDummyActions.ReceiveData), new Type[]{typeof(CommandSender), typeof(string)})]
public static class ReceiveDataPatch
{
    [HarmonyPatch,HarmonyPrefix]
    public static bool Prefix(RaDummyActions __instance, CommandSender sender, string data)
    {
        if (sender is not PlayerCommandSender playerCommandSender)
        {
            return true;
        }

        bool shouldCallOriginal = false;
        uint senderNetId = playerCommandSender.ReferenceHub.netId;
        var field = __instance.GetType().GetField("_senderNetId", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
        {
            Log.Error("Champ privé '_senderNetId' introuvable dans " + __instance.GetType());
        }
        else
        {
            field.SetValue(__instance, senderNetId);
        }

        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
        {
            if(referenceHub.IsHost)
                continue;
            HashSet<uint> orAddNew = RaDummyActions.NonDirtyReceivers.GetOrAddNew(referenceHub.netId);
            if (DummyActionCollector.IsDirty(referenceHub))
            {
                orAddNew.Clear();
                shouldCallOriginal = true;
            }
            else if (!orAddNew.Contains(senderNetId))
            {
                shouldCallOriginal = true;
            }
        }

        if (shouldCallOriginal)
        {
            CallBaseReceiveData(__instance, sender);
        }
        return false;
    }
    
    public static void CallBaseReceiveData(RaClientDataRequest target, CommandSender sender)
    {
        var sb = new StringBuilder();
        sb.Append("$").Append(target.DataId).Append(" ");
        var field = typeof(RaClientDataRequest).GetField("_stringBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
        field?.SetValue(target, sb);

        target.GetType().BaseType
            .GetMethod("GatherData", BindingFlags.Instance | BindingFlags.NonPublic)
            ?.Invoke(target, null);

        sender.RaReply($"${target.DataId} {sb}", true, false, string.Empty);
    }
}