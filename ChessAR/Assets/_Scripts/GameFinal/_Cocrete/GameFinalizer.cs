using System;
using Zenject;


public class GameFinalizer : IGameFinalizer, IInitializable, IDisposable
{

    private readonly ISignalSystem signalSystem;
    private readonly IStateMachine stateMachine;


    public GameFinalizer(ISignalSystem _signalSystem, IStateMachine _stateMachine)
    {
        signalSystem = _signalSystem;
        stateMachine = _stateMachine;
    }


    public void Initialize()
    {
        signalSystem.SubscribeSignal<FinalizeGameSignal>(FinalizeGame);
    }


    public void Dispose()
    {
        signalSystem.UnSubscribeSignal<FinalizeGameSignal>(FinalizeGame);
    }


    private void FinalizeGame(FinalizeGameSignal signal)
    {
        stateMachine.CreateNewState<FinalState>();
    }
}
