public class FinalState : BaseState
{

    private readonly IGameFinalizer gameFinalizer;


    public FinalState(IStateMachine _stateMachine,
        IGameFinalizer _gameFinaizer) : base(_stateMachine)
    {
        gameFinalizer = _gameFinaizer;
    }


    public override void UpdateState()
    {
        gameFinalizer.UpdateFinalizer();
    }
}
