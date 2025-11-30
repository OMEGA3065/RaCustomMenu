using System;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using RaCustomMenu.API;

namespace RaCustomMenu;

public class RaCustomMenuPlugin : Plugin<Config>
{
    public static RaCustomMenuPlugin Instance;
    
    public override string Name { get; } = "RA Custom Menu";
    public override string Description { get; } = "A library which provides support for creating custom dummy actions on players.";
    public override string Author { get; } = "OMEGA3065; Bankokwak";
    public override Version Version { get; } = new Version(0, 1, 0);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

    private Harmony _harmony;

    public override void Enable()
    {
        Instance = this;
        _harmony = new Harmony("omega3065.racustommenu");
        _harmony.PatchAll();
        
        EventHandlers.RegisterEvents();
        if (Config.EnableExamples)
            Provider.RegisterAllProviders();
    }

    public override void Disable()
    {
        EventHandlers.UnRegisterEvents();
        
        _harmony.UnpatchAll();
        _harmony = null;
        Instance = null;
    }
}