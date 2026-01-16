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

    public abstract string CategoryName { get; }

    public abstract bool IsDirty { get; }

    public abstract List<LimitedDummyAction> AddActions(ReferenceHub hub);
    public virtual IEnumerable<DummyAction> GetActionList(ReferenceHub hub)
    {
        static void blank() { }
        return AddActions(hub).Select(a => new DummyAction(a.Name, blank));
    }
    // public virtual bool MayExecuteThis(ICommandSender sender) => true;

    public static bool HasProvider(string categoryName)
    {
        return providersLoaded.Exists(p => p.CategoryName == categoryName);
    }

    public static void RegisterDynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<LimitedDummyAction>> actionGenerator)
    {
        var dynamicProvider = new DynamicProvider(categoryName, isDirty, actionGenerator);
        RegisterProvider(dynamicProvider);
    }

    public static void UnregisterDynamicProvider(string categoryName)
    {
        for (int i = 0; i < providersLoaded.Count; i++)
        {
            if (providersLoaded[i] is DynamicProvider dp && dp.CategoryName == categoryName)
            {
                providersLoaded.RemoveAt(i);
                DebugLogger.Log($"[DynamicProvider] Provider \"{categoryName}\" removed.");
                return;
            }
        }

        Logger.Warn($"[DynamicProvider] No provider found to unregister for category: {categoryName}");
    }

    public static bool TryGetDynamicProvider(string categoryName, out DynamicProvider outProvider)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                outProvider = dynamicProvider;
                return true;
            }
        }
        outProvider = null;
        return false;
    }

    public static void AddActionDynamic(string categoryName, List<LimitedDummyAction> actions)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.AdditionalActions.AddRange(actions);
                // DebugLogger.Log($"[DynamicProvider] Actions added to category: {categoryName}");
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
                // DebugLogger.Log($"[DynamicProvider] Action \"{actionName}\" removed from category: {categoryName}");
                return;
            }
        }

        Logger.Warn($"[DynamicProvider] No provider found for category: {categoryName}");
    }

    public static void RemoveActionsDynamic(string categoryName, Predicate<LimitedDummyAction> predicate)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.RemoveDynamicActions(predicate);
                // DebugLogger.Log($"[DynamicProvider] Action \"{actionName}\" removed from category: {categoryName}");
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
                    if (Activator.CreateInstance(type) is Provider provider)
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
        if (provider == null)
            return;
        providersLoaded.Add(provider);
        provider.OnRegistered();
    }

    protected virtual void OnRegistered() { }
    public virtual bool IsAllowedOnPlayer(ReferenceHub hub) => true;
}

public class DynamicProvider : Provider
{
    private readonly string _categoryName;
    private readonly bool _isDirty;
    private readonly Func<ReferenceHub, List<LimitedDummyAction>> _baseActionGenerator;
    public readonly List<LimitedDummyAction> AdditionalActions = new();

    public DynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<LimitedDummyAction>> actionGenerator)
    {
        _categoryName = categoryName;
        _isDirty = isDirty;
        _baseActionGenerator = actionGenerator;
    }

    public override string CategoryName => _categoryName;

    public override bool IsDirty => _isDirty;

    public override List<LimitedDummyAction> AddActions(ReferenceHub hub)
    {
        var output = _baseActionGenerator.Invoke(hub);
        output.AddRange(AdditionalActions);
        return output;
    }

    // public void AddDynamicActions(List<LimitedDummyAction> actions)
    // {
    //     static void blank() { }
    //     _additionalActions.AddRange(actions.Select(a => new DummyAction(a.Name, blank)));
    // }

    // public override IEnumerable<DummyAction> GetActionList(ReferenceHub hub)
    // {
    //     var baseActions = _baseActionGenerator?.Invoke(hub) ?? [];
    //     static void blank() { }
    //     baseActions.AddRange(_additionalActions);
    //     return baseActions;
    // }

    // public override List<Player> TargetPlayer()
    // {
    //     return _mayBeAllowed;
    // }

    public void RemoveDynamicActionByName(string actionName)
    {
        AdditionalActions.RemoveAll(action => action.Name == actionName);
    }

    public void RemoveDynamicActions(Predicate<LimitedDummyAction> predicate)
    {
        AdditionalActions.RemoveAll(predicate);
    }
}