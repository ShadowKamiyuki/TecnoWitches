using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPresenter
{
    private readonly CharacterSelectView _view;
    private readonly CharacterDatabase _characterDatabase;
    private readonly ISaveService _saveService;
    private readonly IAppStateMachine _stateMachine;

    private SaveData _saveData;

    public CharacterSelectPresenter(CharacterSelectView view)
    {
        _view = view;
        _characterDatabase = view.Database;

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
        IReadOnlyList<CharacterData> characters = _characterDatabase.Characters;
        IReadOnlyList<CharacterButtonView> buttons = _view.Characters;

        int count = Mathf.Min(characters.Count, buttons.Count);

        for (int i = 0; i < count; i++)
        {
            CharacterData data = characters[i];
            CharacterButtonView button = buttons[i];

            bool unlocked = data.UnlockedByDefault ||  _saveData.UnlockedCharacters.Contains(data.Id);

            button.Setup(data.DisplayName, data.Icon, unlocked);

            if (unlocked)
            {
                CharacterData capturedCharacter = data;
                button.OnSelected += () => OnCharacterSelected(capturedCharacter);
            }
        }
    }

    private void OnCharacterSelected(CharacterData character)
    {
        Debug.Log($"Selected character ID: '{character.Id}'");

        IRunManager runManager = ServiceLocator.Get<IRunManager>();

        RunStartRequest runStartData = new RunStartRequest(character.Id);
        Debug.Log($"DTO created with ID: '{runStartData.CharacterID}'");

        LoadingRequest request = new LoadingRequest(
            load: new[] { "Game" },
            unload: new[] { "CharacterSelect" },
            nextState: AppState.Gameplay,
            payload: runStartData
        );

        _stateMachine.RequestSceneChange(request);
    }
}
