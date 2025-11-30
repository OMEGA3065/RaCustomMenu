using System.ComponentModel;

namespace RaCustomMenu;

public class Config
{
    public bool Debug { get; set; } = false;
    [Description("Whether or not to enable MENU examples.")]
    public bool EnableExamples { get; set; } = false;
    [Description("What permission is required for all custom Dummy actions. Set to `null` or `empty string` for no permission required.")]
    public string DefaultRequiredPermission { get; set; } = "";
}