public class MainMenuPresenter
{
    private readonly MainMenuView _view;
    private readonly IAppStateMachine _stateMachine;

    public MainMenuPresenter(MainMenuView view)
    {
        _view = view;
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.OnStartClicked += OnStartClicked;
        _view.OnQuitClicked += OnQuitClicked;
        _view.Show();
    }

    public void Dispose()
    {
        _view.OnStartClicked -= OnStartClicked;
        _view.OnQuitClicked -= OnQuitClicked;
        _view.Hide();
    }

    private void OnStartClicked()
    {
        var request = new LoadingRequest(
            load: new[] { "SaveSelect" },
            unload: new[] { "MainMenu" },
            nextState: AppState.SaveSelect
        );

        _stateMachine.RequestSceneChange(request);
    }

    private void OnQuitClicked()
    {
        UnityEngine.Application.Quit();
    }
}
