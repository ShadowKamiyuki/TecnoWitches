using UnityEngine;

public class RunManager : IRunManager
{
    private RunData _currentRun;
    private string _selectedCharacter;

    private readonly ISceneLoader _sceneLoader;
    private readonly IAppStateMachine _stateMachine;

    public bool IsRunActive => _currentRun != null;
    public int CurrentSeed => _currentRun?.Seed ?? 0;
    public int CurrentFloor => _currentRun?.Floor ?? 0;

    public RunManager()
    {
        _sceneLoader = ServiceLocator.Get<ISceneLoader>();
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void StartRun()
    {
        if (IsRunActive)
            return;

        int seed = UnityEngine.Random.Range(0, int.MaxValue);
        _currentRun = new RunData(seed);

        RegisterRunServices();

        UnityEngine.Debug.Log($"Run iniciada con seed {seed}");
    }

    public void EndRun()
    {
        if (!IsRunActive)
            return;

        ServiceLocator.ClearRunServices();
        _currentRun = null;

        UnityEngine.Debug.Log("Run finalizada y runtime limpiado.");
    }

    private void RegisterRunServices()
    {
        //ServiceLocator.RegisterRun<IPlayerRuntimeStats>(new PlayerRuntimeStats());
        //ServiceLocator.RegisterRun<IEnemyFactory>(new EnemyFactory());
    }

    public void RestartRun()
    {
        EndRun();
        StartRun();
    }

    public void AdvanceFloor()
    {
        _currentRun?.AdvanceFloor();
    }

    public void PlayerDied()
    {
        EndRun();
        _stateMachine.SetState(AppState.GameOver);
    }

    public void SetSelectedCharacter(string id)
    {
        _selectedCharacter = id;
    }

    public string GetSelectedCharacter()
    {
        return _selectedCharacter;
    }

    private void EndRunAndSave()
    {
        var saveService = ServiceLocator.Get<ISaveService>();

        var saveData = saveService.LoadSlot(saveService.ActiveSlot);

        // Ejemplo simple de meta progreso:
        saveData.Currency += 10;
        saveData.TotalPlayTime += Time.time;

        saveService.SaveSlot(saveService.ActiveSlot, saveData);

        ServiceLocator.Get<IAppStateMachine>()
            .SetState(AppState.GameOver);
    }
}
