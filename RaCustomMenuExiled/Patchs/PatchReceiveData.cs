using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using NetworkManagerUtils.Dummies;
using RemoteAdmin;
using RemoteAdmin.Communication;

namespace RaCustomMenuExiled.Patchs;

[HarmonyPatch(typeof(RaDummyActions), nameof(RaDummyActions.ReceiveData), new Type[] { typeof(CommandSender), typeof(string) })]
public static class ReceiveDataPatch
{
    private static readonly AccessTools.FieldRef<RaDummyActions, uint> SenderNetIdRef =
        AccessTools.FieldRefAccess<RaDummyActions, uint>("_senderNetId");

    private static readonly AccessTools.FieldRef<RaClientDataRequest, StringBuilder> StringBuilderRef =
        AccessTools.FieldRefAccess<RaClientDataRequest, StringBuilder>("_stringBuilder");
    
    private static readonly Action<RaClientDataRequest> GatherDataDelegate;
    
    static ReceiveDataPatch()
    {
        var methodInfo = AccessTools.Method(typeof(RaClientDataRequest), "GatherData");
        if (methodInfo == null)
            throw new Exception("Method 'GatherData' not found on RaClientDataRequest.");

        GatherDataDelegate = AccessTools.MethodDelegate<Action<RaClientDataRequest>>(methodInfo);
    }
    
    [HarmonyPrefix]
    public static bool Prefix(RaDummyActions __instance, CommandSender sender, string data)
    {
        if (sender is not PlayerCommandSender playerCommandSender)
        {
            return true;
        }

        bool shouldCallOriginal = false;
        uint senderNetId = playerCommandSender.ReferenceHub.netId;

        SenderNetIdRef(__instance) = senderNetId;

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
        var sb = new StringBuilder();
        sb.Append("$").Append(target.DataId).Append(" ");

        StringBuilderRef(target) = sb;

        GatherDataDelegate(target);

        sender.RaReply($"${target.DataId} {sb}", true, false, string.Empty);
    }
}
