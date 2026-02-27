using System;

public interface IAppStateMachine
{
    AppState CurrentState { get; }

    event Action<AppState> OnStateChanged;

    void SetState(AppState newState, object payload = null);

    LoadingRequest ConsumePendingRequest();
    void RequestSceneChange(LoadingRequest request);
}