using UnityEngine;


public class EnemyMoveState : BaseState
{
    private IOpponentMoveManager opponentMoveManager;
    private ISpecialMovesHandler specialMovesHandler;
    private PlayerSettingsSO playerSettings;


    public EnemyMoveState(
        IStateMachine _stateMachine,
        IOpponentMoveManager _opponentMoveManager,
        ISpecialMovesHandler _specialMovesHandler,
        PlayerSettingsSO _playerSettings) : base(_stateMachine)
    {
        opponentMoveManager = _opponentMoveManager;
        specialMovesHandler = _specialMovesHandler;
        playerSettings = _playerSettings;
    }


    public override void InitializeState()
    {
        //TODO
        if (playerSettings.CurrentPlayMode == PlayMode.AI_VS_PLAYER)
        {
            playerSettings.IsPlayerStartsGame = !playerSettings.IsPlayerStartsGame;
            stateMachine.CreateNewState<PlayerMoveState>();
            return;
        }

        opponentMoveManager.PerformMove();
    }


    public override void DisposeState()
    {
        specialMovesHandler.TryPerformPromotionMove();
    }
}
