using System;
using Zenject;


public class StateMachine<TStartState> : IStateMachine, IInitializable, ITickable, IDisposable, IFixedTickable
    where TStartState : BaseState
{
    private readonly DiContainer container;
    private BaseState currentBaseState;

    public StateMachine(DiContainer container)
    {
        this.container = container;
    }

    public void Initialize()
    {
        CreateNewState<TStartState>();
    }

    public void Tick()
    {
        currentBaseState?.UpdateState();
    }

    public void FixedTick()
    {
        currentBaseState?.FixedUpdateState();
    }

    public void Dispose()
    {
        currentBaseState?.DisposeState();
    }

    public void CreateNewState<TBaseState>()
        where TBaseState : BaseState
    {
        currentBaseState?.DisposeState();
        currentBaseState = container.Instantiate<TBaseState>();
        currentBaseState?.InitializeState();
    }

    public BaseState GetCurrentState()
    {
        return currentBaseState;
    }
}