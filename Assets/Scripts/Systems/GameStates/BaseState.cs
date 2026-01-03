using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly GameManager gameManager;

    public virtual string Name => GetType().Name;

    protected BaseState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public virtual void Enter()
    {
#if UNITY_EDITOR
        Debug.Log($"[STATE ENTER] {Name}");
#endif
    }

    public virtual void Exit()
    {
#if UNITY_EDITOR
        Debug.Log($"[STATE EXIT] {Name}");
#endif
    }

    public virtual void Update(float deltaTime) { }

    protected void ChangeState(GameManager.GameState newState)
    {
        gameManager.SetGameState(newState);
    }
}
