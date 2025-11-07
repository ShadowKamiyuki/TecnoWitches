using System.Collections.Generic;
using UnityEngine;

public class CustomUpdateManager : MonoBehaviourSingleton<CustomUpdateManager>
{
    // Lista de todas las clases que deben recibir el Tick()
    private List<IUpdatable> updatables = new List<IUpdatable>();

    protected override void OnAwaken()
    {
        ServiceLocator.Register<CustomUpdateManager>(this);
        Debug.Log("UpdateManager registrado");
    }

    protected override void OnDestroyed()
    {
        ServiceLocator.Unregister<CustomUpdateManager>();
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