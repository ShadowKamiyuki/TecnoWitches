using UnityEngine;
using UnityEngine.SceneManagement;

public class AppBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);
    }
}
