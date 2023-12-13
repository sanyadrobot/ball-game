namespace Mono.States.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
    }
}