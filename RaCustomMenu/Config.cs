using System.ComponentModel;

namespace RaCustomMenu;

public class Config
{
    public bool Debug { get; set; } = false;
    [Description("Whether or not to add Custom Dummy actions to actual dummies.")]
    public bool CustomMenuOnDummies { get; set; } = false;
    [Description("Whether or not to enable RA Custom Menu examples.")]
    public bool EnableExamples { get; set; } = false;
    [Description("What permission is required for all custom Dummy actions. Set to `null` or an `empty string` to disable this requirement.")]
    public string DefaultRequiredPermission { get; set; } = "";
}