public interface IRunManager
{
    int CurrentSeed { get; }
    int CurrentFloor { get; }

    void StartRun();
    void EndRun();
    void RestartRun();
    void AdvanceFloor();
    void SetSelectedCharacter(string characterID);
}
