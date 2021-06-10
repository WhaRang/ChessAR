public class PlayerMoveState : BaseState
{

    private readonly ISelectionManager selectionManager = null;
    private readonly IFigureAccessor figureAccessor = null;
    private readonly ISpecialMovesHandler specialMovesHandler = null;
    private readonly PlayerSettingsSO playerSettings = null;


    public PlayerMoveState(
        IStateMachine _stateMachine,
        ISelectionManager _selectionManager,
        IFigureAccessor _figureAccessor,
        ISpecialMovesHandler _specialMovesHandler,
        PlayerSettingsSO _playerSettings) : base(_stateMachine)
    {
        selectionManager = _selectionManager;
        figureAccessor = _figureAccessor;
        specialMovesHandler = _specialMovesHandler;
        playerSettings = _playerSettings;
    }


    public override void InitializeState()
    {
        figureAccessor.SetAllWhiteFiguresCollidersActive(playerSettings.IsPlayerStartsGame);
        figureAccessor.SetAllBlackFiguresCollidersActive(!playerSettings.IsPlayerStartsGame);
    }


    public override void UpdateState()
    {
        selectionManager.SelectionUpdate();
    }


    public override void DisposeState()
    {
        specialMovesHandler.TryPerformPromotionMove();
        figureAccessor.SetAllWhiteFiguresCollidersActive(false);
        figureAccessor.SetAllBlackFiguresCollidersActive(false);
    }
}
