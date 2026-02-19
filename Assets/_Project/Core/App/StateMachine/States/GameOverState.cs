using UnityEngine;

public class GameOverState : IAppState
{
    private GameOverPresenter _presenter;
    private GameOverView _view;

    public void Enter()
    {
        _view = Object.FindFirstObjectByType<GameOverView>();

        if (_view == null)
        {
            Debug.LogError("GameOverView no encontrada.");
            return;
        }

        _presenter = new GameOverPresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        _presenter?.Dispose();
        _presenter = null;
    }
}
