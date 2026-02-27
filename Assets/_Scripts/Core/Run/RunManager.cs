using System;

public class RunManager : IRunManager
{
    private RunData _currentRun;
    private readonly ISeedGenerator _seedGenerator;

    public bool IsRunActive => _currentRun != null;
    public int CurrentSeed => _currentRun?.Seed ?? 0;
    public int CurrentFloor => _currentRun?.Floor ?? 0;
    public string CurrentCharacterID => _currentRun?.CharacterID;

    public event Action<int> OnFloorChanged;
    public event Action OnRunStarted;
    public event Action OnRunEnded;

    public RunManager(ISeedGenerator seedGenerator)
    {
        _seedGenerator = seedGenerator;
    }

    public void StartRun(string characterID)
    {
        if (IsRunActive)
            return;

        if (characterID == null)
            throw new ArgumentNullException(nameof(characterID));

        int seed = _seedGenerator.Generate();

        _currentRun = new RunData(seed, characterID);
        OnRunStarted?.Invoke();
    }

    public void EndRun()
    {
        _currentRun = null;
        OnRunEnded?.Invoke();
    }

    public void RestartRun(int seed, string characterID)
    {
        EndRun();
        StartRun(characterID);
    }

    public void AdvanceFloor()
    {
        _currentRun?.AdvanceFloor();
        OnFloorChanged?.Invoke(CurrentFloor);
    }

    public void PlayerDied()
    {
        EndRun();
    }
}