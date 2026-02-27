using UnityEngine;

public class PausedState : IAppState
{
    private PausePresenter _presenter;
    private PauseView _view;

    public void Enter(object payload)
    {
        Time.timeScale = 0f;

        _view = Object.FindFirstObjectByType<PauseView>();

        if (_view == null)
        {
            Debug.LogError("PauseView no encontrada.");
            return;
        }

        _presenter = new PausePresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        Time.timeScale = 1f;

        _presenter?.Dispose();
        _presenter = null;
    }
}
