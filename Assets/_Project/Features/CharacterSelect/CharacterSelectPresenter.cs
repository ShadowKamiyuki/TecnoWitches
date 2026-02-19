using System.Collections.Generic;

public class CharacterSelectPresenter
{
    private readonly CharacterSelectView _view;
    private readonly ISaveService _saveService;
    private readonly IAppStateMachine _stateMachine;

    private SaveData _saveData;

    private readonly List<CharacterDefinition> _allCharacters = new()
    {
        new CharacterDefinition { Id = "warrior", DisplayName = "Warrior" },
        new CharacterDefinition { Id = "mage", DisplayName = "Mage" },
        new CharacterDefinition { Id = "rogue", DisplayName = "Rogue" }
    };

    public CharacterSelectPresenter(CharacterSelectView view)
    {
        _view = view;
        _saveService = ServiceLocator.Get<ISaveService>();
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.Show();

        _saveData = _saveService.LoadSlot(_saveService.ActiveSlot);

        SetupCharacters();
    }

    public void Dispose()
    {
        _view.Hide();
    }

    private void SetupCharacters()
    {
        for (int i = 0; i < _view.Characters.Count; i++)
        {
            var button = _view.Characters[i];
            var character = _allCharacters[i];

            bool unlocked = _saveData.UnlockedCharacters.Contains(character.Id)
                            || character.Id == "warrior"; // starter default

            button.Setup(character.DisplayName, unlocked);

            if (unlocked)
            {
                string capturedId = character.Id;
                button.OnSelected += () => OnCharacterSelected(capturedId);
            }
        }
    }

    private void OnCharacterSelected(string characterId)
    {
        var runManager = ServiceLocator.Get<IRunManager>();
        runManager.SetSelectedCharacter(characterId);

        var request = new LoadingRequest(
            load: new[] { "Game" },
            unload: new[] { "CharacterSelect" },
            nextState: AppState.Gameplay
        );

        _stateMachine.RequestSceneChange(request);
    }
}
