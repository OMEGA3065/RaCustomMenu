using System;
using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using JetBrains.Annotations;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenuExiled.API;

public abstract class Provider
{
    public static readonly List<Provider> providersLoaded = new List<Provider>();
    protected static List<Player> targetedPlayers = new();
    
    public abstract string CategoryName { get; }
    
    public abstract bool IsDirty { get; }
    
    public abstract List<DummyAction> AddAction(ReferenceHub hub);
    
    public virtual List<Player> TargetPlayer()
    {
        return targetedPlayers.Count == 0 ? null : targetedPlayers;
    }
    
    public static void AddTarget(Player player)
    {
        if (!targetedPlayers.Contains(player))
            targetedPlayers.Add(player);
    }

    public static void RemoveTarget(Player player)
    {
        if (targetedPlayers.Contains(player))
            targetedPlayers.Remove(player);
    }
    
    public static void ClearTargets()
    {
        targetedPlayers.Clear();
    }
    
    public static bool HasProvider(string categoryName)
    {
        return providersLoaded.Exists(p => p.CategoryName == categoryName);
    }
    
    public static void RegisterDynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<DummyAction>> actionGenerator, [CanBeNull] List<Player> PlayerAllow)
    {
        var dynamicProvider = new DynamicProvider(categoryName, isDirty, actionGenerator,PlayerAllow);
        RegisterProviders(dynamicProvider);
    }
    
    public static void UnregisterDynamicProvider(string categoryName)
    {
        for (int i = 0; i < providersLoaded.Count; i++)
        {
            if (providersLoaded[i] is DynamicProvider dp && dp.CategoryName == categoryName)
            {
                providersLoaded.RemoveAt(i);
                Log.Debug($"[DynamicProvider] Provider \"{categoryName}\" removed.");
                return;
            }
        }

        Log.Warn($"[DynamicProvider] No provider found to unregister for category: {categoryName}");
    }

    public static void AddActionDynamic(string categoryName, List<DummyAction> actions)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.AddDynamicActions(actions);
                Log.Debug($"[DynamicProvider] Actions added to category: {categoryName}");
                return;
            }
        }

        Log.Warn($"[DynamicProvider] No provider found for category: {categoryName}");
    }
    
    public static void RemoveActionDynamic(string categoryName, string actionName)
    {
        foreach (var provider in providersLoaded)
        {
            if (provider is DynamicProvider dynamicProvider && dynamicProvider.CategoryName == categoryName)
            {
                dynamicProvider.RemoveDynamicActionByName(actionName);
                Log.Debug($"[DynamicProvider] Action \"{actionName}\" removed from category: {categoryName}");
                return;
            }
        }

        Log.Warn($"[DynamicProvider] No provider found for category: {categoryName}");
    }

    public static void RegisterAllProviders() => RegisterProviders(Assembly.GetCallingAssembly());

    private static void RegisterProviders(Assembly assembly)
    {
        List<Provider> providers = new();

        foreach (Type type in assembly.GetTypes())
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
                    Log.Error($"[DynamicProvider] Error while instantiating {type.Name}: {ex}");
                }
            }
        }

        foreach (Provider provider in providers)
        {
            try
            {
                RegisterProviders(provider);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
    
    private static void RegisterProviders(Provider provider)
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
    private readonly List<Player> _targetedPlayers;

    public DynamicProvider(string categoryName, bool isDirty, Func<ReferenceHub, List<DummyAction>> actionGenerator, [CanBeNull] List<Player> PlayerAllow)
    {
        _categoryName = categoryName;
        _isDirty = isDirty;
        _baseActionGenerator = actionGenerator;
        _targetedPlayers = PlayerAllow;
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

    public override List<Player> TargetPlayer()
    {
        return _targetedPlayers;
    }

    public void RemoveDynamicActionByName(string actionName)
    {
        _additionalActions.RemoveAll(action => action.Name == actionName);
    }
}