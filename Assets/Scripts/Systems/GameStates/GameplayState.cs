using UnityEngine;

public class GameplayState : BaseState
{
    public GameplayState(GameManager gameManager) : base(gameManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 1f;

        InitializeGameplay();
    }

    public override void Exit()
    {
        Time.timeScale = 0f;
        base.Exit();
    }

    public override void Update(float deltaTime)
    {
        // lógica de gameplay si es necesario
    }

    private void InitializeGameplay()
    {
        var selectionService = ServiceLocator.Get<CharacterSelectionService>();
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
}
