using System;

public interface IRunManager
{
    int CurrentSeed { get; }
    int CurrentFloor { get; }
    bool IsRunActive { get; }
    string CurrentCharacterID { get; }

    event Action<int> OnFloorChanged;
    event Action OnRunStarted;
    event Action OnRunEnded;

    void StartRun(string characterID);
    void EndRun();
    void RestartRun(int seed, string characterID);
    void AdvanceFloor();
}
