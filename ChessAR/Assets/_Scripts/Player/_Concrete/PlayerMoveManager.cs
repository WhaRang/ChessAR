using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

public class PlayerMoveManager : IPlayerMoveManager, IInitializable, IDisposable
{
    private readonly ISignalSystem signalSystem = null;
    private readonly IStateMachine stateMachine = null;
    private readonly IChessMoveLogic chessMoveLogic = null;
    private readonly IFigureAccessor figureAccessor = null;
    private readonly ITransformAnimator transformAnimator = null;
    private readonly AsyncProcessor asyncProcessor = null;
    private readonly PlayerSettingsSO playerSettings = null;

    private Transform figureToMove = null;
    private PositionData figureData = null;
    private List<Transform> allowedTransforms = new List<Transform>();
    private List<List<int>> allowedPositions = new List<List<int>>();

    public PlayerMoveManager(
        ISignalSystem _signalSystem,
        IStateMachine _stateMachine,
        IChessMoveLogic _chessMoveLogic,
        IFigureAccessor _figureAccessor,
        ITransformAnimator _tranformAnimator,
        AsyncProcessor _asyncProcessor,
        PlayerSettingsSO _playerSettings)
    {
        signalSystem = _signalSystem;
        stateMachine = _stateMachine;
        chessMoveLogic = _chessMoveLogic;
        figureAccessor = _figureAccessor;
        transformAnimator = _tranformAnimator;
        asyncProcessor = _asyncProcessor;
        playerSettings = _playerSettings;
    }


    public void Initialize()
    {
        signalSystem.SubscribeSignal<SquareSelectedSignal>(PerformMove);
        signalSystem.SubscribeSignal<FigureSelectedSignal>(FigureSelected);
        signalSystem.SubscribeSignal<FigureDeselectedSignal>(FigureDeselected);
        signalSystem.SubscribeSignal<SquaresHighlightedSignal>(SquaresHighlighted);
        signalSystem.SubscribeSignal<NothingWasSelectedSignal>(NothingWasSelected);
    }


    public void Dispose()
    {
        signalSystem.UnSubscribeSignal<SquareSelectedSignal>(PerformMove);
        signalSystem.UnSubscribeSignal<FigureSelectedSignal>(FigureSelected);
        signalSystem.UnSubscribeSignal<FigureDeselectedSignal>(FigureDeselected);
        signalSystem.UnSubscribeSignal<SquaresHighlightedSignal>(SquaresHighlighted);
        signalSystem.UnSubscribeSignal<NothingWasSelectedSignal>(NothingWasSelected);
    }


    private void PerformMove(SquareSelectedSignal signal)
    {
        int index = allowedTransforms.IndexOf(signal.squareTransform);
        if (index >= 0)
        {
            Debug.Log("Player move: " + figureData.GetFirstIndex()
                + " " + figureData.GetSecondIndex()
                + " -> " + allowedPositions[index][0]
                + " " + allowedPositions[index][1]);

            PerformPlayerMove(signal.squareTransform, index);
        }
    }


    private void PerformPlayerMove(Transform aimObj, int index)
    {
        asyncProcessor.StartCoroutine(PerformPlayerMoveCoroutine(aimObj, index));
    }


    private IEnumerator PerformPlayerMoveCoroutine(Transform aimObj, int index)
    {
        allowedTransforms.Clear();

        figureAccessor.SetAllWhiteFiguresCollidersActive(false);
        figureAccessor.SetAllBlackFiguresCollidersActive(false);

        chessMoveLogic.MakeMove(new List<int> {
                figureData.GetFirstIndex(), figureData.GetSecondIndex()},
            new List<int> { allowedPositions[index][0], allowedPositions[index][1] });

        transformAnimator.MoveObjToAnotherObj(figureToMove, aimObj, playerSettings.FigureMoveAnimationTime);


        yield return new WaitForSeconds(playerSettings.FigureMoveAnimationTime);

        if (figureAccessor.GetFigureByIndexes(allowedPositions[index][0], allowedPositions[index][1]))
        {
            DestroyBeatenFigure(index);
        }

        figureData.SetFirstIndex(allowedPositions[index][0]);
        figureData.SetSecondIndex(allowedPositions[index][1]);

        //TODO
        stateMachine.CreateNewState<EnemyMoveState>();

        //GameFinal test
        signalSystem.FireSignal(new FinalizeGameSignal()
        {
            isWhiteWin = true
        });
    }


    private void DestroyBeatenFigure(int index)
    {
        PositionData figureToDestroyData
                = figureAccessor.GetFigureByIndexes(
                    allowedPositions[index][0], allowedPositions[index][1]);

        if (figureToDestroyData)
        {
            figureAccessor.DeleteFigure(figureToDestroyData);
            UnityEngine.Object.Destroy(figureToDestroyData.gameObject);
        }
    }


    private void FigureSelected(FigureSelectedSignal signal)
    {
        figureToMove = signal.figureTransform;
        figureData = signal.figureData;
    }


    private void FigureDeselected(FigureDeselectedSignal signal)
    {
    }


    private void SquaresHighlighted(SquaresHighlightedSignal signal)
    {
        allowedTransforms = signal.highlightedSquares;
        allowedPositions = signal.allowedPositions;
    }


    private void NothingWasSelected(NothingWasSelectedSignal signal)
    {
        allowedTransforms.Clear();
    }
}
