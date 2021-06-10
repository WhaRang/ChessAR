using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoardAccessor : MonoBehaviour, IBoardAccessor
{
    [SerializeField] private MeshRenderer[] firstLineMeshRends;
    [SerializeField] private MeshRenderer[] secondLineMeshRends;
    [SerializeField] private MeshRenderer[] thirdLineMeshRends;
    [SerializeField] private MeshRenderer[] fourthLineMeshRends;
    [SerializeField] private MeshRenderer[] fifthLineMeshRends;
    [SerializeField] private MeshRenderer[] sixthLineMeshRends;
    [SerializeField] private MeshRenderer[] seventhLineMeshRends;
    [SerializeField] private MeshRenderer[] eigthLineMeshRends;

    [Inject] private readonly ISignalSystem signalSystem = null;
    [Inject] private readonly IMaterialAccessor materialAccessor = null;
    [Inject] private readonly IChessMoveLogic chessMoveLogic = null;
    [Inject] private readonly PlayerSettingsSO playerSettings = null;
   

    public MeshRenderer[][] AllSquares { get; private set; }


    private void OnEnable()
    {
        signalSystem.SubscribeSignal<FigureSelectedSignal>(HighlightAllSqaures);
        signalSystem.SubscribeSignal<FigureDeselectedSignal>(MakeAllSquaresInvisible);
    }


    private void OnDisable()
    {
        signalSystem.UnSubscribeSignal<FigureSelectedSignal>(HighlightAllSqaures);
        signalSystem.UnSubscribeSignal<FigureDeselectedSignal>(MakeAllSquaresInvisible);
    }


    private void Awake()
    {
        AllSquares = new MeshRenderer[8][];
        AllSquares[0] = firstLineMeshRends;
        AllSquares[1] = secondLineMeshRends;
        AllSquares[2] = thirdLineMeshRends;
        AllSquares[3] = fourthLineMeshRends;
        AllSquares[4] = fifthLineMeshRends;
        AllSquares[5] = sixthLineMeshRends;
        AllSquares[6] = seventhLineMeshRends;
        AllSquares[7] = eigthLineMeshRends;
    }


    private void HighlightAllSqaures(FigureSelectedSignal signal)
    {
        List<Transform> _highlightedSquares = new List<Transform>();
        List<List<int>> _allowedPositions = new List<List<int>>();
        List<List<bool>> allAvailableMoves = chessMoveLogic.GetAllMoves(
            new List<int>{ signal.figureData.GetFirstIndex(), signal.figureData.GetSecondIndex() },
            !playerSettings.IsPlayerStartsGame);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allAvailableMoves[i][j])
                {
                    AllSquares[i][j].material = materialAccessor.GetByIndex(MaterialIndex.BOARD_HIGHLIGHTED);
                    _highlightedSquares.Add(AllSquares[i][j].GetComponent<Transform>());
                    List<int> allowedPosition = new List<int>(){ i, j };
                    _allowedPositions.Add(allowedPosition);
                }
            }
        }

        signalSystem.FireSignal(new SquaresHighlightedSignal()
        {
            highlightedSquares = _highlightedSquares,
            allowedPositions = _allowedPositions
        });
    }


    private void MakeAllSquaresInvisible(FigureDeselectedSignal signal)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                AllSquares[i][j].material = materialAccessor.GetByIndex(MaterialIndex.BOARD_EMPTY);
            }
        }
    }
}
