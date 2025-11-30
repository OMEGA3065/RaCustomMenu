using System.Reflection;
using CommandSystem;
using JetBrains.Annotations;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenu.API;

public abstract class Provider
{
    public static readonly List<Provider> providersLoaded = new List<Provider>();
    protected static List<Player> targetedPlayers = new();
    
    public abstract string CategoryName { get; }
    
    public abstract bool IsDirty { get; }

    public abstract List<DummyAction> AddAction(ReferenceHub hub);
    public virtual bool MayExecuteThis(ICommandSender sender) => true;
    
    public virtual List<Player> TargetPlayer()
    {
        return targetedPlayers.Count == 0 ? null : targetedPlayers;
    }
    
    public static bool HasProvider(string categoryName)
    {
        return providersLoaded.Exists(p => p.CategoryName == categoryName);
    }
    
    public static void RegisterDynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<DummyAction>> actionGenerator, [CanBeNull] Func<ICommandSender, bool> AllowedPlayers)
    {
        var dynamicProvider = new DynamicProvider(categoryName, isDirty, actionGenerator, AllowedPlayers);
        RegisterProvider(dynamicProvider);
    }
    
    public static void UnregisterDynamicProvider(string categoryName)
    {
        for (int i = 0; i < providersLoaded.Count; i++)
        {
            if (providersLoaded[i] is DynamicProvider dp && dp.CategoryName == categoryName)
            {
                providersLoaded.RemoveAt(i);
                RaCustomMenu.DebugLogger.Log($"[DynamicProvider] Provider \"{categoryName}\" removed.");
                return;
            }
        }

        Logger.Warn($"[DynamicProvider] No provider found to unregister for category: {categoryName}");
    }

    public static void AddActionDynamic(string categoryName, List<DummyAction> actions)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.AddDynamicActions(actions);
                RaCustomMenu.DebugLogger.Log($"[DynamicProvider] Actions added to category: {categoryName}");
                return;
            }
        }

        Logger.Warn($"[DynamicProvider] No provider found for category: {categoryName}");
    }
    
    public static void RemoveActionDynamic(string categoryName, string actionName)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.RemoveDynamicActionByName(actionName);
                RaCustomMenu.DebugLogger.Log($"[DynamicProvider] Action \"{actionName}\" removed from category: {categoryName}");
                return;
            }
        }

        Logger.Warn($"[DynamicProvider] No provider found for category: {categoryName}");
    }

    public static void RegisterAllProviders() => RegisterProviders(Assembly.GetCallingAssembly());

    public static void RegisterProviders(IEnumerable<Type> typesToCheck)
    {
        List<Provider> providers = new();

        foreach (Type type in typesToCheck)
        {
            if (!type.IsAbstract && typeof(Provider).IsAssignableFrom(type) && type != typeof(DynamicProvider))
            {
                try
                {
                    Provider provider = Activator.CreateInstance(type) as Provider;
                    if (provider != null)
                        providers.Add(provider);
                }
                catch (Exception ex)
                {
                    Logger.Error($"[DynamicProvider] Error while instantiating {type.Name}: {ex}");
                }
            }
        }

        RegisterProviders(providers);
    }

    public static void RegisterProviders(IEnumerable<Provider> providers)
    {
        foreach (Provider provider in providers)
        {
            try
            {
                RegisterProvider(provider);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
    
    public static void RegisterProviders(Assembly assembly)
    {
        RegisterProviders(assembly.GetTypes());
    }
    
    public static void RegisterProvider(Provider provider)
    {
        if(provider == null)
            return;
        providersLoaded.Add(provider);
        provider.OnRegistered();
    }
    
    protected virtual void OnRegistered(){}
}

public class DynamicProvider : Provider
{
    private readonly string _categoryName;
    private readonly bool _isDirty;
    private readonly Func<ReferenceHub, List<DummyAction>> _baseActionGenerator;
    private readonly List<DummyAction> _additionalActions = new();
    private readonly Func<ICommandSender, bool> _mayBeAllowed;

    public DynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<DummyAction>> actionGenerator, [CanBeNull] Func<ICommandSender, bool> AllowedPlayers)
    {
        _categoryName = categoryName;
        _isDirty = isDirty;
        _baseActionGenerator = actionGenerator;
        _mayBeAllowed = AllowedPlayers;
    }

    public override string CategoryName => _categoryName;

    public override bool IsDirty => _isDirty;

    public void AddDynamicActions(List<DummyAction> actions)
    {
        _additionalActions.AddRange(actions);
    }

    public override List<DummyAction> AddAction(ReferenceHub hub)
    {
        var baseActions = _baseActionGenerator?.Invoke(hub) ?? new List<DummyAction>();
        baseActions.AddRange(_additionalActions);
        return baseActions;
    }

    // public override List<Player> TargetPlayer()
    // {
    //     return _mayBeAllowed;
    // }

    public override bool MayExecuteThis(ICommandSender sender)
    {
        return _mayBeAllowed?.Invoke(sender) ?? true;
    }

    public void RemoveDynamicActionByName(string actionName)
    {
        _additionalActions.RemoveAll(action => action.Name == actionName);
    }
}