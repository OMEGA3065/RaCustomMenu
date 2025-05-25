using System;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using RaCustomMenuLabApi.API;

namespace RaCustomMenuLabApi;

public class Plugin : Plugin<Config>
{
    public override string Name { get; } = "RaCustomMenuLabApi";
    public override string Description { get; } = "";
    public override string Author { get; } = "Bankokwak";
    public override Version Version { get; } = new Version(1, 1, 2);
    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

    public static Plugin Instance;
    private Harmony _harmony;

    public override void Enable()
    {
        Instance = this;
        _harmony = new Harmony("fr.bankokwak.patchs");
        _harmony.PatchAll();
        
        EventHandlers.RegisterEvent();
        if(Config.EnableExamble)
            Provider.RegisterAllProviders();
    }

    public override void Disable()
    {
        EventHandlers.UnRegisterEvent();
        
        _harmony.UnpatchAll();
        _harmony = null;
        Instance = null;
    }
}