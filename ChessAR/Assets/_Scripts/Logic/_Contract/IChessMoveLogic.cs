using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessMoveLogic
{
    public List<List<List<int>>> boardStory { get; }

    List<List<int>> board { get; }

    void MakeEnemyMove(List<int> pieceStartPosition, List<int> pieceEndPosition);

    void MakeMove(List<int> pieceStartPosition, List<int> pieceEndPosition);

    bool IsMoveAllowed(List<int> pieceStartPosition, List<int> pieceEndPosition);

    List<List<bool>> GetAllMoves(List<int> piecePosition, bool isBlack, bool isAlt = false);
}
