# RA Custom Menu
[![Downloads](https://img.shields.io/github/downloads/OMEGA3065/RaCustomMenu/total.svg)](https://github.com/OMEGA3065/RaCustomMenu/releases/latest)

### Installation Dependencies
This plugin works on [LabApi](https://github.com/northwood-studios/LabAPI/releases/tag/1.1.4) >= **1.1.4**.
Download `RaCustomMenu.dll` from the [latest](https://github.com/OMEGA3065/RaCustomMenu/releases/latest) release and put it in your plugins folder `./LabAPI/plugins/global(or {port})`.
And you need to add `0Harmony.dll` into your `./LabAPI/dependencies/global(or {port})`.

# Features
RaCustomMenu allows you to add custom Categories and Actions (Buttons) to the Dummy RA Category. You can (multi)select players and click a button to perform custom actions.
Example: give a [loadout](https://github.com/OMEGA3065/RaCustomMenu/blob/master/RaCustomMenu/RaCustomMenu/Example/ProviderLoadout.cs) or trigger another custom action.

If you multiselect players, the action can be stacked if the action names are the same; otherwise, the action is performed only on the clicked player's button.

## Config
### Example Toggle
In config file you can enable and disable the example for this project: See [here](https://github.com/OMEGA3065/RaCustomMenu/blob/f5cbf6c148c4c47036a6d33b76e41385640581ce/RaCustomMenu/Config.cs#L11C1-L11C54).
### Default Permission Requirement
The default required permission to use any features of RA Custom Menu can also be set in the config. It can be left blank to allow anyone with RA access to run commands.
This is the intended feature to allow developers to handle permission requirements in their own code. 

## For plugin developers
You might want to look at the [wiki for the parent project](https://github.com/Bankokwak/RaCustomMenu/wiki).
The only difference should be that this project's Providers use a LimitedDummyAction instead of the regular one.
The LimitedDummyAction gives you access to the ICommandSender of whoever initiated the action.

## Contributing
If you find and a bug, and you can fix it, feel free to make a PR.
You can also report bugs in the [GitHub Issues tab](https://github.com/OMEGA3065/RaCustomMenu/issues).

