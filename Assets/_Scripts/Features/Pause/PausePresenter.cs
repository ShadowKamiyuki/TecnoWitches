public class PausePresenter
{
    private readonly PauseView _view;
    private readonly IAppStateMachine _stateMachine;

    public PausePresenter(PauseView view)
    {
        _view = view;
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.Show();
        _view.OnResume += Resume;
        _view.OnQuit += QuitToMenu;
    }

    public void Dispose()
    {
        _view.OnResume -= Resume;
        _view.OnQuit -= QuitToMenu;
        _view.Hide();
    }

    private void Resume()
    {
        _stateMachine.SetState(AppState.Gameplay);
    }

    private void QuitToMenu()
    {
        var request = new LoadingRequest(
            load: new[] { "MainMenu" },
            unload: new[] { "Gameplay" },
            nextState: AppState.MainMenu
        );

        _stateMachine.RequestSceneChange(request);
    }
}
