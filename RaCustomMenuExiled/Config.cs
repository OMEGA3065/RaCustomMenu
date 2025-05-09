using Exiled.API.Interfaces;

namespace RaCustomMenuExiled;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public bool Debug { get; set; }
    
    public bool EnableExamble { get; set; } = false;
}