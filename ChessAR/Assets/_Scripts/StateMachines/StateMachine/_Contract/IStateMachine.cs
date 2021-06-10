public interface IStateMachine
{
    void CreateNewState<TBaseState>() where TBaseState : BaseState;

    BaseState GetCurrentState();
}
