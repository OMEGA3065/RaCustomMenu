# RaCustomMenu
[![Downloads](https://img.shields.io/github/downloads/Bankokwak/RaCustomMenu/total.svg)](https://github.com/Bankokwak/RaCustomMenu/releases/latest)

This plugin works on [EXILED](https://gitlab.com/exmod-team/EXILED/-/tree/LabAPI?ref_type=heads) >= **9.6.0**.
Download `RaCustomMenu.dll` in the [latest](https://github.com/Bankokwak/RaCustomMenu/releases/latest) release assets, then put it in your plugins folder `../EXILED/Plugins/`.

## For plugin creator
Reference RaCustomMenu DLL using the `RaCustomMenu.dll`.

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

## if you see a bug, please report this [here](https://github.com/Bankokwak/RaCustomMenu/issues) or in my mp ( bankokwak ).
