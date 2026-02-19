using System;

public interface IAppStateMachine
{
    AppState CurrentState { get; }

    event Action<AppState> OnStateChanged;

    void SetState(AppState newState);
    void RequestSceneChange(LoadingRequest request);
}