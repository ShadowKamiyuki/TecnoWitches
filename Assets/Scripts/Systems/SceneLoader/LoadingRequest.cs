using System.Collections.Generic;

public class LoadingRequest
{
    public List<string> scenesToLoad { get; } = new();
    public List<string> scenesToUnload { get; } = new();
    public GameManager.GameState NextState { get; }

    public LoadingRequest(IEnumerable<string> load, IEnumerable<string> unload, GameManager.GameState nextState)
    {
        scenesToLoad.AddRange(load);
        scenesToUnload.AddRange(unload);
        NextState = nextState;
    }
}
