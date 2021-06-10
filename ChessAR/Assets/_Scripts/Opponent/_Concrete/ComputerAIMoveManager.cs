using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputerAIMoveManager : IOpponentMoveManager
{
    private readonly IChessMoveLogic chessMoveLogic = null;
    private readonly IStateMachine stateMachine = null;
    private readonly IFigureAccessor figureAccessor = null;
    private readonly IBoardAccessor boardSquaresAccessor = null;
    private readonly ITransformAnimator transformAnimator = null;
    private readonly AsyncProcessor asyncProcessor = null;
    private readonly PlayerSettingsSO playerSettings = null;


    public ComputerAIMoveManager(
        IChessMoveLogic _chessMoveLogic,
        IStateMachine _stateMachine,
        IFigureAccessor _figureAccessor,
        IBoardAccessor _boardSquaresAccessor,
        ITransformAnimator _transformAnimator,
        AsyncProcessor _asyncProcessor,
        PlayerSettingsSO _playerSettings)
    {
        chessMoveLogic = _chessMoveLogic;
        stateMachine = _stateMachine;
        figureAccessor = _figureAccessor;
        boardSquaresAccessor = _boardSquaresAccessor;
        transformAnimator = _transformAnimator;
        asyncProcessor = _asyncProcessor;
        playerSettings = _playerSettings;
    }


    public void PerformMove()
    {
        int[] computerMove = BenChess.ChessBoard.GetComputerMove(chessMoveLogic.boardStory, playerSettings.AiDepth);

        Debug.Log("Opponent move: " + computerMove[0]
            + " " + computerMove[1] + " -> "
            + computerMove[2] + " " + computerMove[3]);

        PerformMoveToPosition(computerMove);
    }


    private void PerformMoveToPosition(int[] computerMove)
    {
        asyncProcessor.StartCoroutine(PerformMoveToPositionCoroutine(computerMove));
    }


    private IEnumerator PerformMoveToPositionCoroutine(int[] computerMove)
    {
        PositionData figureData =
            figureAccessor.GetFigureByIndexes(computerMove[0], computerMove[1]);

        transformAnimator.MoveObjToAnotherObj(
            figureData.gameObject.transform,
            boardSquaresAccessor.AllSquares[computerMove[2]][computerMove[3]].gameObject.transform,
            playerSettings.FigureMoveAnimationTime);

        yield return new WaitForSeconds(playerSettings.FigureMoveAnimationTime);

        if (figureAccessor.GetFigureByIndexes(computerMove[2], computerMove[3]))
        {
            DestroyBeatenFigure(computerMove);
        }

        chessMoveLogic.MakeEnemyMove(new List<int> { computerMove[0], computerMove[1] },
            new List<int> { computerMove[2], computerMove[3] });

        figureData.SetFirstIndex(computerMove[2]);
        figureData.SetSecondIndex(computerMove[3]);

        stateMachine.CreateNewState<PlayerMoveState>();
    }


    private void DestroyBeatenFigure(int[] computerMove)
    {
        PositionData figureToDestroyData = figureAccessor.GetFigureByIndexes(computerMove[2], computerMove[3]);

        if (figureToDestroyData)
        {
            figureAccessor.DeleteFigure(figureToDestroyData);
            Object.Destroy(figureToDestroyData.gameObject);
        }
    }
}
