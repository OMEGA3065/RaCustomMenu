using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exiled.API.Features;
using NetworkManagerUtils.Dummies;

namespace RaCustomMenu.API;

public abstract class Provider
{
    private static readonly List<Provider> ProvidersLoaded = new List<Provider>();
    
    public abstract string CategoryName { get; set; }
    
    public abstract List<DummyAction> AddAction(ReferenceHub hub);

    public static void RegisterAllProviders() => RegisterProviders(Assembly.GetCallingAssembly());

    private static void RegisterProviders(Assembly assembly)
    {
        List<Provider> providers = new();

        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsAbstract && typeof(Provider).IsAssignableFrom(type))
            {
                try
                {
                    Provider provider = Activator.CreateInstance(type) as Provider;
                    if (provider != null)
                        providers.Add(provider);
                }
                catch (Exception ex)
                {
                    Log.Error($"Erreur lors de l'instanciation de {type.Name} : {ex}");
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
        ProvidersLoaded.Add(provider);
        provider.OnRegistered();
    }

    public static List<Provider> GetAllProviders() => ProvidersLoaded.ToList();
    
    protected virtual void OnRegistered(){}
}