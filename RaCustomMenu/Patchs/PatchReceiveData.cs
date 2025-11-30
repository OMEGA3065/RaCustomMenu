using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using NetworkManagerUtils.Dummies;
using RemoteAdmin;
using RemoteAdmin.Communication;

namespace RaCustomMenu.Patchs;

[HarmonyPatch(typeof(RaDummyActions), nameof(RaDummyActions.ReceiveData), new Type[] { typeof(CommandSender), typeof(string) })]
public static class ReceiveDataPatch
{
    private static readonly AccessTools.FieldRef<RaClientDataRequest, StringBuilder> StringBuilderRef =
        AccessTools.FieldRefAccess<RaClientDataRequest, StringBuilder>("_stringBuilder");
    
    [HarmonyPrefix]
    public static bool Prefix(RaDummyActions __instance, CommandSender sender, string data)
    {
        if (sender is not PlayerCommandSender playerCommandSender)
        {
            return true;
        }

        bool shouldCallOriginal = false;
        uint senderNetId = playerCommandSender.ReferenceHub.netId;

        __instance._senderNetId = senderNetId;

        foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
        {
            if (referenceHub.IsHost)
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
        target._stringBuilder.Clear();
        target._stringBuilder.Append("$").Append(target.DataId).Append(" ");


        target.GatherData();

        sender.RaReply($"${target.DataId} {target._stringBuilder}", true, false, string.Empty);
    }
}
