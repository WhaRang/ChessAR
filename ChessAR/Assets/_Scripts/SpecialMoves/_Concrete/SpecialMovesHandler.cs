using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SpecialMovesHandler : ISpecialMovesHandler, IInitializable, IDisposable
{

    private readonly ISignalSystem signalSystem;
    private readonly IFigureAccessor figureAccessor;
    private readonly IBoardAccessor boardAccessor;
    private readonly IMaterialAccessor materialAccessor;
    private readonly ITransformAnimator transformAnimator;
    private readonly ICastleRookPointsAccessor castleRookPointsAccessor;
    private readonly PlayerSettingsSO playerSettings;
    private readonly AsyncProcessor asyncProcessor;

    private bool isPromotion = false;
    private PromotionSignal lasPromotionSignal = null;


    public SpecialMovesHandler(
        ISignalSystem _signalSystem,
        IFigureAccessor _figureAccessor,
        IBoardAccessor _boardAccessor,
        IMaterialAccessor _materialAccessor,
        ITransformAnimator _transformAnimator,
        ICastleRookPointsAccessor _castleRookPointsAccessor,
        PlayerSettingsSO _playerSettings,
        AsyncProcessor _asyncProcessor)
    {
        signalSystem = _signalSystem;
        figureAccessor = _figureAccessor;
        boardAccessor = _boardAccessor;
        materialAccessor = _materialAccessor;
        transformAnimator = _transformAnimator;
        castleRookPointsAccessor = _castleRookPointsAccessor;
        playerSettings = _playerSettings;
        asyncProcessor = _asyncProcessor;
    }


    public void Initialize()
    {
        signalSystem.SubscribeSignal<CastlingSignal>(PerformCastleRookMove);
        signalSystem.SubscribeSignal<EnPassantSignal>(BeatPawnEnPassant);
        signalSystem.SubscribeSignal<PromotionSignal>(PromotePawn);
    }


    public void Dispose()
    {
        signalSystem.UnSubscribeSignal<CastlingSignal>(PerformCastleRookMove);
        signalSystem.UnSubscribeSignal<EnPassantSignal>(BeatPawnEnPassant);
        signalSystem.UnSubscribeSignal<PromotionSignal>(PromotePawn);
    }


    public void TryPerformPromotionMove()
    {
        if (!isPromotion)
        {
            return;
        }
        isPromotion = false;

        PositionData pawnToPromoteData
                = figureAccessor.GetFigureByIndexes(
                    lasPromotionSignal.promotedPawn[0], lasPromotionSignal.promotedPawn[1]);

        Mesh queenMesh = lasPromotionSignal.isBlack ?
            materialAccessor.GetBlackQueen() : materialAccessor.GetWhiteQueen();

        if (!queenMesh || !pawnToPromoteData)
        {
            return;
        }

        pawnToPromoteData.GetComponent<MeshFilter>().mesh = queenMesh;
    }


    private void PromotePawn(PromotionSignal signal)
    {
        isPromotion = true;
        lasPromotionSignal = signal;        
    }


    private void BeatPawnEnPassant(EnPassantSignal signal)
    {
        PositionData figureToDestroyData
                = figureAccessor.GetFigureByIndexes(
                    signal.beatenPawn[0], signal.beatenPawn[1]);

        if (figureToDestroyData)
        {
            figureAccessor.DeleteFigure(figureToDestroyData);
            UnityEngine.Object.Destroy(figureToDestroyData.gameObject);
        }
    }


    private void PerformCastleRookMove(CastlingSignal signal)
    {
        asyncProcessor.StartCoroutine(PerformCastleRookMoveCoroutine(signal));
    }


    private IEnumerator PerformCastleRookMoveCoroutine(CastlingSignal signal)
    {
        int startX = signal.startPositionRook[0];
        int startY = signal.startPositionRook[1];
        int endX = signal.endPositionRook[0];
        int endY = signal.endPositionRook[1];

        PositionData figureData =
            figureAccessor.GetFigureByIndexes(startX, startY);

        figureData.SetFirstIndex(endX);
        figureData.SetSecondIndex(endY);

        Transform aimObj = castleRookPointsAccessor.GetPointByPosition(figureData.transform.position);
        transformAnimator.MoveObjToAnotherObj(figureData.transform, aimObj, playerSettings.FigureMoveAnimationTime / 2);

        yield return new WaitForSeconds(playerSettings.FigureMoveAnimationTime / 2);

        aimObj = boardAccessor.AllSquares[endX][endY].transform;
        transformAnimator.MoveObjToAnotherObj(figureData.transform, aimObj, playerSettings.FigureMoveAnimationTime / 2);
    }
}
