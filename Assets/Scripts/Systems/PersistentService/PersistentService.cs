using UnityEngine;

public abstract class PersistentService<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual void Awake()
    {
        if (this is not T service)
        {
            Debug.LogError($"{GetType().Name} no coincide con {typeof(T).Name}");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        ServiceLocator.Register(service);
        OnAwakeService();
    }

    private void OnDestroy()
    {
        OnDestroyService();
        ServiceLocator.Unregister<T>();
    }

    protected virtual void OnAwakeService() { }
    protected virtual void OnDestroyService() { }
}
