using System;

public interface IRunService
{
    bool IsRunActive { get; }

    //void StartRun(CharacterType character);
    void EndRun();

    event Action OnRunStarted;
    event Action OnRunEnded;
}
