using System.Threading.Tasks;
using UnityEngine;

public class GameplayState : BaseState
{
    private SceneLoaderService sceneLoader;
    private LevelController levelController;

    private const string PLAYER_HUD_SCENE = "UI_PlayerHUD";
    private const string INITIAL_LEVEL = "Lobby"; // tu nivel inicial

    public GameplayState(GameManager gameManager) : base(gameManager)
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
        levelController = ServiceLocator.Get<LevelController>();
    }

    public override async void Enter()
    {
        base.Enter();
        Time.timeScale = 1f;

        if (sceneLoader == null)
        {
            Debug.LogError("GameplayState: SceneLoaderService no registrado!");
            return;
        }

        if (levelController == null)
        {
            Debug.LogError("GameplayState: LevelController no registrado!");
            return;
        }

        levelController.OnLevelLoaded += InitializeGameplay;

        // Cargar HUD y nivel en paralelo
        await Task.WhenAll(LoadPlayerHUDAsync(), levelController.LoadLevel(INITIAL_LEVEL));
    }

    public override void Exit()
    {
        Time.timeScale = 0f;

        if (levelController != null)
            levelController.OnLevelLoaded -= InitializeGameplay;

        base.Exit();
    }

    public override void Update(float deltaTime)
    {
        // lógica de gameplay si es necesario
    }

    private void InitializeGameplay()
    {
        CharacterSelectionService selectionService = ServiceLocator.Get<CharacterSelectionService>();
        CharacterData selectedCharacter = selectionService.GetSelectedCharacter();

        if (selectedCharacter == null)
        {
            Debug.LogError("GameplayState: No character selected!");
            return;
        }

        var player = Object.FindFirstObjectByType<PlayerStats>();

        if (player != null)
        {
            player.Initialize(selectedCharacter);
        }
        else
        {
            Debug.LogWarning("GameplayState: PlayerController not found in scene");
        }
    }

    private async Task LoadPlayerHUDAsync()
    {
        sceneLoader = ServiceLocator.Get<SceneLoaderService>();
        if (sceneLoader == null) return;

        // Evitar cargar el HUD si ya está cargado
        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(PLAYER_HUD_SCENE).isLoaded)
        {
            await sceneLoader.LoadSceneAsync(PLAYER_HUD_SCENE);
        }
    }
}
