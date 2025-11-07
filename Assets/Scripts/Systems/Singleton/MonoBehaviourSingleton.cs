using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicado de {typeof(T)} destruido en {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        DontDestroyOnLoad(gameObject);
        OnAwaken();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            OnDestroyed();
            Instance = null;
        }
    }

    protected virtual void OnAwaken() { }

    protected virtual void OnDestroyed() { }
}