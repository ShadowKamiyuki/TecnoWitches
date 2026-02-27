using System.Collections.Generic;

public class LoadingRequest
{
    public IReadOnlyList<string> ScenesToLoad => _scenesToLoad;
    public IReadOnlyList<string> ScenesToUnload => _scenesToUnload;
    public AppState NextState { get; }
    public object Payload;

    private readonly List<string> _scenesToLoad = new();
    private readonly List<string> _scenesToUnload = new();

    public LoadingRequest(IEnumerable<string> load, IEnumerable<string> unload, AppState nextState, object payload = null)
    {
        _scenesToLoad.AddRange(load);
        _scenesToUnload.AddRange(unload);
        NextState = nextState;
        Payload = payload;
    }
}
