public interface IAppState
{
    void Enter(object payload = null);   // Lógica al entrar al estado
    void Exit();    // Lógica al salir del estado
}