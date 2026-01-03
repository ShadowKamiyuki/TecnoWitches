public interface IState
{
    string Name { get; }

    void Enter();

    void Update(float deltaTime);

    void Exit();
}
