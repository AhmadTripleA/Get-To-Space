using System;
using System.Collections.Generic;

public class ServiceDB
{
    private static Dictionary<Type, object> services = new();

    public static void Register<T>(T service) where T : class
    {
        // Only add if it does not exist
        if (!services.ContainsKey(typeof(T)))
        {
            services[typeof(T)] = service;
        }
    }

    public static T Get<T>() where T : class
    {
        return services.TryGetValue(typeof(T), out var service) ? service as T : null;
    }

    public static void Unregister<T>() where T : class
    {
        services.Remove(typeof(T));
    }
}
