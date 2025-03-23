using System;
using System.Collections.Generic;

public class ServiceDB
{
    private static Dictionary<Type, object> services = [];

    public static void Register<T>(T service) where T : class
    {
        services[typeof(T)] = service;
    }

    public static T Get<T>() where T : class
    {
        return services[typeof(T)] as T;
    }

    public static void Unregister<T>() where T : class
    {
        services.Remove(typeof(T));
    }
}