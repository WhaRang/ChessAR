using System.Collections;
using UnityEngine;


public class StartState : BaseState
{

    private readonly ITransformAnimator transformAnimator = null;
    private readonly IFigureAccessor figureAccessor = null;
    private readonly AsyncProcessor asyncProcessor = null;
    private readonly BoardMarker boardMarker = null;
    private readonly PlayerSettingsSO playerSettings = null;


    public StartState(
        IStateMachine _stateMachine,
        ITransformAnimator _transformAnimator,
        IFigureAccessor _figureAccessor,
        AsyncProcessor _asyncProcessor,
        BoardMarker _boardMarker,
        PlayerSettingsSO _playerSettings) : base(_stateMachine)
    {
        transformAnimator = _transformAnimator;
        figureAccessor = _figureAccessor;
        asyncProcessor = _asyncProcessor;
        boardMarker = _boardMarker;
        playerSettings = _playerSettings;
    }


    public override void InitializeState()
    {
        figureAccessor.SetAllBlackFiguresCollidersActive(false);
        figureAccessor.SetAllWhiteFiguresCollidersActive(false);

        if (playerSettings.IsPlayerStartsGame)
        {
            StartGame();
        }
        else
        {
            StartGameWithAnimation();
        }
    }


    private void StartGame()
    {
        if (playerSettings.IsPlayerStartsGame)
        {
            stateMachine.CreateNewState<PlayerMoveState>();
        }
        else
        {
            stateMachine.CreateNewState<EnemyMoveState>();
        }
    }


    private void StartGameWithAnimation()
    {
        asyncProcessor.StartCoroutine(StartGameWithAnimationCoroutine());
    }


    private IEnumerator StartGameWithAnimationCoroutine()
    {
        Vector3 oldVectorRotation = boardMarker.transform.rotation.eulerAngles;
        Vector3 newEulerRotation = new Vector3(oldVectorRotation.x, 180.0f, oldVectorRotation.z);

        transformAnimator.RotateObjToPoint(boardMarker.transform, newEulerRotation, playerSettings.BoardRotationAnimationTime);
        yield return new WaitForSeconds(playerSettings.BoardRotationAnimationTime);

        StartGame();
    }
}
