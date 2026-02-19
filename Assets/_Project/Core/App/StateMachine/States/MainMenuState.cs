using UnityEngine;

public class MainMenuState : IAppState
{
    private MainMenuPresenter _presenter;
    private MainMenuView _view;

    public MainMenuState() { }

    public void Enter()
    {
        _view = Object.FindFirstObjectByType<MainMenuView>();

        if (_view == null)
        {
            Debug.LogError("MainMenuView no encontrada en la escena.");
            return;
        }

        _presenter = new MainMenuPresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        if (_view == null) return;

        _presenter?.Dispose();
        _presenter = null;
    }
}
