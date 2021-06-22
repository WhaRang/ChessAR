using System.Collections;
using UnityEngine;


public class StartState : BaseState
{

    private readonly ITransformAnimator transformAnimator = null;
    private readonly IFigureAccessor figureAccessor = null;
    private readonly ICamerasAccessor camerasAccessor = null;
    private readonly AsyncProcessor asyncProcessor = null;
    private readonly BoardMarker boardMarker = null;
    private readonly PlayerSettingsSO playerSettings = null;
    private readonly DefaultTrackableEventHandler defaultTrackableEventHandler = null;


    public StartState(
        IStateMachine _stateMachine,
        ITransformAnimator _transformAnimator,
        IFigureAccessor _figureAccessor,
        ICamerasAccessor _camerasAccessor,
        AsyncProcessor _asyncProcessor,
        BoardMarker _boardMarker,
        DefaultTrackableEventHandler _defaultTrackableEventHandler,
        PlayerSettingsSO _playerSettings) : base(_stateMachine)
    {
        transformAnimator = _transformAnimator;
        figureAccessor = _figureAccessor;
        camerasAccessor = _camerasAccessor;
        asyncProcessor = _asyncProcessor;
        boardMarker = _boardMarker;
        defaultTrackableEventHandler = _defaultTrackableEventHandler;
        playerSettings = _playerSettings;
    }


    public override void InitializeState()
    {
        SetupSceneOnProperCameraMode();
        DisableAllFiguresColliders();

        if (playerSettings.IsPlayerStartsGame || playerSettings.CurrentPlayMode == PlayMode.PLAYER_VS_PLAYER)
        {
            StartGame();
        }
        else
        {
            StartGameWithAnimation();
        }
    }


    private void SetupSceneOnProperCameraMode()
    {
        camerasAccessor.DefaultCamera.gameObject.SetActive(!playerSettings.IsArEnbled);
        camerasAccessor.ARcamera.gameObject.SetActive(playerSettings.IsArEnbled);
        defaultTrackableEventHandler.gameObject.SetActive(playerSettings.IsArEnbled);
    }


    private void DisableAllFiguresColliders()
    {
        figureAccessor.SetAllBlackFiguresCollidersActive(false);
        figureAccessor.SetAllWhiteFiguresCollidersActive(false);
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
