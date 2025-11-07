using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service)
    {
        var type = typeof(T);
        _services[type] = service;
        Debug.Log($"[ServiceLocator] Registrado: {type.Name}");
    }

    public static void Unregister<T>()
    {
        var type = typeof(T);
        if (_services.Remove(type))
            Debug.Log($"[ServiceLocator] Eliminado: {type.Name}");
    }

    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;

        Debug.LogWarning($"[ServiceLocator] Servicio no encontrado: {typeof(T).Name}");
        return default;
    }

    public static bool Exists<T>() => _services.ContainsKey(typeof(T));
}
