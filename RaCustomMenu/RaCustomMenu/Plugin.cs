using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using RaCustomMenu.API;

namespace RaCustomMenu;

public class Plugin : Plugin<Config>
{
    public override string Name => "RaCustomMenu";
    public override string Author => "Bankokwak";
    public override Version Version => new Version(1, 0, 0);
    public override Version RequiredExiledVersion { get; } = new Version(9,6,0);
    public override PluginPriority Priority { get; } = PluginPriority.High;

    public static Plugin Instance;
    private Harmony _harmony;

    public override void OnEnabled()
    {
        Instance = this;
        _harmony = new Harmony("fr.bankokwak.patchs");
        _harmony.PatchAll();
        
        EventHandlers.RegisterEvent();
        if(Config.EnableExamble)
            Provider.RegisterAllProviders();
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        EventHandlers.UnRegisterEvent();
        
        _harmony.UnpatchAll();
        _harmony = null;
        Instance = null;
        base.OnDisabled();
    }
}