public interface IRunManager
{
    int CurrentSeed { get; }
    int CurrentFloor { get; }
    bool IsRunActive { get; }
    string CurrentCharacterID { get; }

    void StartRun(int seed, string characterID);
    void EndRun();
    void RestartRun(int seed, string characterID);
    void AdvanceFloor();
}
