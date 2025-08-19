# Credits
The original repo can be found [here](https://github.com/Bankokwak/RaCustomMenu/).
All credit goes to **Bankokwak**

# RaCustomMenu
[![Downloads](https://img.shields.io/github/downloads/Bankokwak/RaCustomMenu/total.svg)](https://github.com/Bankokwak/RaCustomMenu/releases/latest)

### Exiled Version
This plugin works on [EXILED](https://gitlab.com/exmod-team/EXILED/-/tree/LabAPI?ref_type=heads) >= **9.6.0**.
Download `RaCustomMenu.dll` in the [latest](https://github.com/OMEGA3065/RaCustomMenu/releases/latest) release assets, then put it in your plugins folder `./EXILED/Plugins/`.

# What is this plugin for ?
RaCustomMenu allows you to add custom Categories and Actions to the Dummy Ra Category. You can (multi)select players and click a button to perform custom actions.
Example: give a [loadout](https://github.com/OMEGA3065/RaCustomMenu/blob/master/RaCustomMenu/RaCustomMenu/Example/ProviderLoadout.cs) or trigger another custom action.

If you multiselect players, the action can be stacked if the action names are the same; otherwise, the action is performed only on the clicked player's button.

## Config
In config file you can enable and disable the test button: See [here](https://github.com/Bankokwak/RaCustomMenu/blob/652f4ba746ee9f3c005b377b671de89fcf5e5277/RaCustomMenu/Config.cs#L11C5-L11C6)

## Permission
You need to add `rcm.actions` to a RA role in your permissions.yml file, with that, the users can use the RA Custom Menu

### For EXILED :
`./EXILED/Config/permissions.yml`

## For plugin creator
A [wiki](https://github.com/Bankokwak/RaCustomMenu/wiki) has been setup here just for you.

## if you see a bug, please report this [here](https://github.com/OMEGA3065/RaCustomMenu/issues) or in my Discord DMs ( omaga3065 ).
## or to the orignal creator (https://github.com/Bankokwak/RaCustomMenu/issues) or in my Discord dm ( bankokwak ).
