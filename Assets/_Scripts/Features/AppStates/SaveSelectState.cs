using UnityEngine;

public class SaveSelectState : IAppState
{
    private SaveSelectPresenter _presenter;
    private SaveSelectView _view;

    public void Enter(object payload)
    {
        _view = Object.FindFirstObjectByType<SaveSelectView>();

        if (_view == null)
        {
            Debug.LogError("SaveSelectView no encontrada.");
            return;
        }

        _presenter = new SaveSelectPresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        _presenter?.Dispose();
        _presenter = null;
    }
}
