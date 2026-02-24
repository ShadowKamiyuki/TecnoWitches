using UnityEngine;

public class CharacterSelectState : IAppState
{
    private CharacterSelectPresenter _presenter;
    private CharacterSelectView _view;

    public void Enter()
    {
        _view = Object.FindFirstObjectByType<CharacterSelectView>();

        if (_view == null)
        {
            Debug.LogError("CharacterSelectView no encontrada.");
            return;
        }

        _presenter = new CharacterSelectPresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        _presenter?.Dispose();
        _presenter = null;
    }
}
