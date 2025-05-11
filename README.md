# RaCustomMenu
[![Downloads](https://img.shields.io/github/downloads/Bankokwak/RaCustomMenu/total.svg)](https://github.com/Bankokwak/RaCustomMenu/releases/latest)

### Exiled Version
This plugin works on [EXILED](https://gitlab.com/exmod-team/EXILED/-/tree/LabAPI?ref_type=heads) >= **9.6.0**.
Download `RaCustomMenuExiled.dll` in the [latest](https://github.com/Bankokwak/RaCustomMenu/releases/latest) release assets, then put it in your plugins folder `./EXILED/Plugins/`.

### LabApi Version
This plugin works on [LabApi](https://github.com/northwood-studios/LabAPI/releases/tag/0.7.0) >= **0.7.0**.
Download `RaCustomMenuLabApi.dll` in the [latest](https://github.com/Bankokwak/RaCustomMenu/releases/latest) release assets, then put it in your plugins folder `./LabAPI-Beta/plugins/global(or {port})`.
And you need to add `0Harmony.dll` into your `./LabAPI-Beta/dependencies/global(or {port})`.

# What is this plugin for ?
RaCustomMenu allows you to add custom Categories and Actions to the Dummy Ra Category. You can (multi)select players and click a button to perform custom actions.
Example: give a [loadout](https://github.com/Bankokwak/RaCustomMenu/blob/master/RaCustomMenu/RaCustomMenu/Example/ProviderLoadout.cs) or trigger another custom action.

If you multiselect players, the action can be stacked if the action names are the same; otherwise, the action is performed only on the clicked player's button.

## For plugin creator
Reference RaCustomMenu DLL using the `RaCustomMenuExiled.dll` or `RaCustomMenuLabApi.dll`.

- Create a new class

this is a example of a provider class (with needed `override`):

```c#
    public class Test : Provider
    {
        public override string CategoryName { get; set; } = "Test";

        public override List<DummyAction> AddAction(ReferenceHub hub)
        {
            return new List<DummyAction>()
            {
                new DummyAction("Name button", () =>
                {
                    //Action
                })
            };
        }
    }
```

Now you need, on the start of the server, register `Provider` of your `Assembly`
```c#
Provider.RegisterAll();
```

for EXILED, you can call this when the plugin is enabled:
```c#
public override void OnEnabled()
{
    Provider.RegisterAll();
    base.OnEnabled();
}
```
and for LabApi, you can call this when the plugin is enabled:
```c#
public override void Enable()
{
    Provider.RegisterAll();
}
```

## if you see a bug, please report this [here](https://github.com/Bankokwak/RaCustomMenu/issues) or in my mp ( bankokwak ).
