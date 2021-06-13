using System;
using Zenject;
using UnityEngine;
using UnityEngine.SceneManagement;


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


    public void UpdateFinalizer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneIndexes.MENU.ToString());
        }
    }


    private void FinalizeGame(FinalizeGameSignal signal)
    {
        stateMachine.CreateNewState<FinalState>();
    }
}
