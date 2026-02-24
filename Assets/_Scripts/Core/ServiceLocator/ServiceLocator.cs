using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _globalServices = new();
    private static Dictionary<Type, object> _runServices = new();


    // Global services
    public static void RegisterGlobal<T>(T service)
    {
        Type type = typeof(T);

        if (service == null)
        {
            Debug.LogError($"[ServiceLocator] Intento de registrar servicio null: {type.Name}");
            return;
        }

        if (_globalServices.ContainsKey(type))
        {
            Debug.LogWarning($"[ServiceLocator] Servicio ya registrado: {type.Name}");
            return;
        }

        _globalServices[typeof(T)] = service;
        Debug.Log($"[ServiceLocator] Registrado: {type.Name}");
    }

    public static void UnregisterGlobal<T>()
    {
        Type type = typeof(T);
        if (_globalServices.Remove(type))
            Debug.Log($"[ServiceLocator] Eliminado: {type.Name}");
    }

    // Run services
    public static void RegisterRun<T>(T service)
    {
        Type type = typeof(T);

        if (service == null)
        {
            Debug.LogError($"[ServiceLocator] Intento de registrar servicio null: {type.Name}");
            return;
        }

        if (_runServices.ContainsKey(type))
        {
            Debug.LogWarning($"[ServiceLocator] Servicio ya registrado: {type.Name}");
            return;
        }

        _runServices[typeof(T)] = service;
        Debug.Log($"[ServiceLocator] Registrado: {type.Name}");
    }

    public static void UnregisterRun<T>()
    {
        Type type = typeof(T);
        if (_runServices.Remove(type))
            Debug.Log($"[ServiceLocator] Eliminado: {type.Name}");
    }

    public static void ClearRunServices()
    {
        _runServices.Clear();
    }

    // Getter
    public static T Get<T>()
    {
        Type type = typeof(T);

        if (_runServices.TryGetValue(type, out var runService))
            return (T)runService;

        if (_globalServices.TryGetValue(type, out var service))
            return (T)service;

        Debug.LogWarning($"[ServiceLocator] Servicio no encontrado: {type.Name}");
        return default;
    }

    public static bool Exists<T>()
    {
        Type type = typeof(T);
        return _runServices.ContainsKey(type) || _globalServices.ContainsKey(type);
    }
}
