using System;

public class RunManager : IRunManager
{
    private RunData _currentRun;

    public bool IsRunActive => _currentRun != null;
    public int CurrentSeed => _currentRun?.Seed ?? 0;
    public int CurrentFloor => _currentRun?.Floor ?? 0;
    public string CurrentCharacterID => _currentRun?.CharacterID;

    public void StartRun(int seed, string characterID)
    {
        if (IsRunActive)
            return;

        if (characterID == null)
            throw new ArgumentNullException(nameof(characterID));

        _currentRun = new RunData(seed, characterID);
    }

    public void EndRun()
    {
        _currentRun = null;
    }

    public void RestartRun(int seed, string characterID)
    {
        EndRun();
        StartRun(seed, characterID);
    }

    public void AdvanceFloor()
    {
        _currentRun?.AdvanceFloor();
    }

    public void PlayerDied()
    {
        EndRun();
    }
}