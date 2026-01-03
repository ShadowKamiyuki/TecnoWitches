using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : PersistentService<CustomUpdateManager>
{
    // Lista de todas las clases que deben recibir el Tick()
    private List<IUpdatable> updatables = new List<IUpdatable>();

    // ambos metodos de servicios pueden ser quitados si no tienen logica adicional
    // se registran y desregistran desde persistentService.cs
    protected override void OnAwakeService()
    {
        Debug.Log("UpdateManager registrado");
    }

    protected override void OnDestroyService()
    {
        Debug.Log("UpdateManager destruido");
    }

    void Update()
    {
        foreach (var u in updatables)
        {
            u.Tick(Time.deltaTime); // Llamamos al método Tick de cada clase registrada
        }
    }

    // Método para registrar una clase al sistema de actualización
    public void Register(IUpdatable updatable)
    {
        if (!updatables.Contains(updatable))
            updatables.Add(updatable);
    }

    // Método para quitar una clase del sistema si ya no necesita actualizarse
    public void Unregister(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }
}