public class GameOverPresenter
{
    private readonly GameOverView _view;
    private readonly IAppStateMachine _stateMachine;

    public GameOverPresenter(GameOverView view)
    {
        _view = view;
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.Show();
        _view.OnBackToMenu += OnBackToMenu;
    }

    public void Dispose()
    {
        _view.OnBackToMenu -= OnBackToMenu;
        _view.Hide();
    }

    private void OnBackToMenu()
    {
        var request = new LoadingRequest(
            load: new[] { "MainMenu" },
            unload: new[] { "Gameplay" },
            nextState: AppState.MainMenu
        );

        _stateMachine.RequestSceneChange(request);
    }
}
