using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ChessMoveLogic : IChessMoveLogic, IInitializable
{

    public List<List<List<int>>> boardStory { get; private set; } = new List<List<List<int>>>();
    public List<List<int>> board { get; private set; } = new List<List<int>>();

    private readonly ISignalSystem signalSystem;
    //private readonly PlayerSettingsSO playerSettings;

    public ChessMoveLogic(
        ISignalSystem _signalSystem)
    {
        signalSystem = _signalSystem;
    }


    public void Initialize()
    {
        board.Add(new List<int> { 2, 3, 4, 6, 5, 4, 3, 2 });
        board.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });//2,3
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 });
        board.Add(new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 });
        board.Add(new List<int> { -2, -3, -4, -6, -5, -4, -3, -2 });
        //showBoard(board);
        boardStory.Add(board);
    }


    public void MakeEnemyMove(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        List<List<int>> newBoard = boardStory[boardStory.Count - 1];
        newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
        newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
        boardStory.Add(newBoard);
    }


    public void MakeMove(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        if (isCastle(pieceStartPosition, pieceEndPosition))
        {
            if (isCastleAllowed(pieceStartPosition, pieceEndPosition))
            {
                int color = 1;
                if (boardStory.Count % 2 == 0)
                {
                    color = -1;
                }
                List<List<int>> newBoard = boardStory[boardStory.Count - 1];
                if (pieceEndPosition[1] == 1)//short castle
                {
                    //New position king
                    newBoard[pieceStartPosition[0]][1] = 6 * color;
                    //New position rook
                    newBoard[pieceEndPosition[0]][2] = 2 * color;
                    newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                    newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = 0;
                    boardStory.Add(newBoard);

                    signalSystem.FireSignal(new CastlingSignal()
                    {
                        startPositionRook = new List<int>() { pieceStartPosition[0], 0 },
                        endPositionRook = new List<int>() { pieceStartPosition[0], 2 }
                    });
                }
                else
                {//long castle
                 //New position king
                    newBoard[pieceStartPosition[0]][5] = 6 * color;
                    //New position rook
                    newBoard[pieceEndPosition[0]][4] = 2 * color;
                    newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                    newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = 0;
                    //
                    boardStory.Add(newBoard);

                    signalSystem.FireSignal(new CastlingSignal()
                    { 
                        startPositionRook = new List<int>() { pieceStartPosition[0], 7 },
                        endPositionRook = new List<int>() { pieceStartPosition[0], 4 }
                    });
                }
                return;
            }
                
        }
        if (boardStory[boardStory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == 1 ||
            (boardStory.Count % 2 == 0 && boardStory[boardStory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == -1))
        {
            if (isEnPassant(pieceStartPosition, new List<int> { pieceEndPosition[0] + 1, pieceEndPosition[1] }) ||
                isEnPassant(pieceStartPosition, new List<int> { pieceEndPosition[0] - 1, pieceEndPosition[1] }))
            {
                List<List<int>> newBoard = boardStory[boardStory.Count - 1];
                newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
                if (boardStory.Count % 2 == 0)
                {
                    newBoard[pieceEndPosition[0] + 1][pieceEndPosition[1]] = 0;
                    signalSystem.FireSignal(new EnPassantSignal()
                    {
                        beatenPawn = new List<int>() { pieceEndPosition[0] + 1, pieceEndPosition[1] }
                    });
                }
                else
                {
                    newBoard[pieceEndPosition[0] - 1][pieceEndPosition[1]] = 0;
                    signalSystem.FireSignal(new EnPassantSignal()
                    {
                        beatenPawn = new List<int>() { pieceEndPosition[0] - 1, pieceEndPosition[1] }
                    });
                }
                newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
                boardStory.Add(newBoard);
                return;
            }
        }

        if (IsPromotion(pieceStartPosition, pieceEndPosition))
        {
            List<List<int>> newBoard = boardStory[boardStory.Count - 1];
            int color = 1;
            if (boardStory.Count % 2 == 0)
            {
                color = -1;
            }
            Debug.Log("promotion: " + pieceEndPosition[0] + ", " + pieceEndPosition[1] + ", color: " + color);
            newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = 5 * color; // narazie zakladamy ze promocja jest do hetmana (krolowej)
            newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
            boardStory.Add(newBoard);

            signalSystem.FireSignal(new PromotionSignal()
            {
                promotedPawn = new List<int>() { pieceEndPosition[0], pieceEndPosition[1] },
                isBlack = (color == -1)
            } );
        }


        if (IsMoveAllowed(pieceStartPosition, pieceEndPosition))
        {
            List<List<int>> newBoard = boardStory[boardStory.Count - 1];
            newBoard[pieceEndPosition[0]][pieceEndPosition[1]] = newBoard[pieceStartPosition[0]][pieceStartPosition[1]];
            newBoard[pieceStartPosition[0]][pieceStartPosition[1]] = 0;
            boardStory.Add(newBoard);
        }
        else
        {
            Debug.Log("Move is not allowed: " + pieceStartPosition[0] + " " + 
            pieceStartPosition[1] + " -> " + pieceEndPosition[0] + " " +
            pieceEndPosition[1]);
        }
    }


    public bool IsMoveAllowed(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        List<List<bool>> allowedMoves = GetAllMoves(pieceStartPosition, boardStory.Count % 2 == 0);
        if (allowedMoves[pieceEndPosition[0]][pieceEndPosition[1]]) return true;
        else return false;
    }


	private bool isCastle(List<int> pieceStartPosition, List<int> pieceEndPosition) {
        int color = 1;
        if (boardStory.Count % 2 == 0)
        {
            color = -1;
        }
        return (Math.Abs(pieceStartPosition[1] - pieceEndPosition[1]) == 2)
            && (boardStory[boardStory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == 6 * color);
    }


    private bool isCastleAllowed(List<int> pieceStartPosition, List<int> pieceEndPosition) {
        List<List<int>> boardNow = boardStory[boardStory.Count - 1];
        int ky = pieceStartPosition[1];//3
        int ry = pieceStartPosition[1] > pieceEndPosition[1] ? 0 : 7;
        int color = 1;
        if (boardStory.Count % 2 == 0)
        {
            color = -1;
        }
        if (isCheck(boardNow)) return false;
        if (ky > ry) //0
        {
            //Short
            for (int i = ky - 1; i > ry; i--)
            {
                if (boardNow[pieceStartPosition[0]][i] != 0) return false;
            }
        }
        else
        {
            //Long
            for (int i = ky + 1; i < ry; i++) {
                if (boardNow[pieceStartPosition[0]][i] != 0) return false;
            }
        }
        for (int i = 0; i < boardStory.Count; i++) {
            if (boardStory[i][pieceStartPosition[0]][pieceStartPosition[1]] != 6 * color || boardStory[i][pieceEndPosition[0]][ry] != 2 * color) {
                return false;
            }
        }
        return true;
    }


    public bool IsPromotion(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {

        if (boardStory[boardStory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == 1
            || boardStory[boardStory.Count - 1][pieceStartPosition[0]][pieceStartPosition[1]] == -1) //jezeli rusza sie pionek
            if (pieceEndPosition[0] == 7 || pieceEndPosition[0] == 0 ) // jezeli rusza sie na ostatnia linie
                return true;
        return false;
    }


    private bool isEnPassant(List<int> pieceStartPosition, List<int> pieceEndPosition)
    {
        if (boardStory.Count <= 2)
        {
            return false;
        }

        int row_start = pieceStartPosition[0];
        int row_end = pieceEndPosition[0];
        int col_start = pieceStartPosition[1];
        int col_end = pieceEndPosition[1];
        List<List<int>> boardNow = boardStory[boardStory.Count - 1];
        List<List<int>> boardBefore = boardStory[boardStory.Count - 3];
        if ((pieceEndPosition[1] >= 0 && pieceEndPosition[1] <= 7) && (pieceStartPosition[0] == 4 || (boardStory.Count % 2 == 0 && pieceStartPosition[0] == 3)))
        {
            if ((boardNow[pieceStartPosition[0]][pieceStartPosition[1]] == 1 && boardNow[pieceEndPosition[0] - 1][pieceEndPosition[1]] == -1) || //biale
                (boardNow[pieceStartPosition[0]][pieceStartPosition[1]] == -1 && boardNow[pieceEndPosition[0] + 1][pieceEndPosition[1]] == 1)) //czarne
            {
                Debug.Log("first condition is true");
                if (boardNow[pieceEndPosition[0] - 1][pieceEndPosition[1]] == -1)
                {
                    string s = "";
                    foreach (List<List<int>> list in boardStory)
                    {
                        s += " " + list[7][3];
                    }
                    Debug.Log("czarny pionek stoi tam gdzie ma stac, board before:" + s);

                    if (boardBefore[pieceEndPosition[0] + 1][pieceEndPosition[1]] == -1)
                    {
                        Debug.Log("w poprzednim ruchu pionek byl na startie");
                        if (boardNow[pieceEndPosition[0] + 1][pieceEndPosition[1]] == 0)
                        {
                            Debug.Log("isEnpassant is true");
                            return true;
                        }
                    }
                }
                else if (boardNow[pieceEndPosition[0] + 1][pieceEndPosition[1]] == 1)
                {
                    Debug.Log("bialy pionek stoi tam gdzie ma stac");
                    if (boardBefore[pieceEndPosition[0] - 1][pieceEndPosition[1]] == 1)
                    {
                        Debug.Log("w poprzednim ruchu pionek byl na startie");
                        if (boardNow[pieceEndPosition[0] - 1][pieceEndPosition[1]] == 0)
                        {
                            Debug.Log("isEnpassant is true");
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }


    /*private bool isMate(List<List<List<int>>> boardStory)
    {
        List<List<int>> board = new List<List<int>>(boardStory[boardStory.Count - 1]);
        for (int i = 0; i < board.Count; i++)
        {
            List<int> list = board[i];
            for (int j = 0; j < list.Count; j++)
            {
                int cell = list[j];
              
                    bool tempIsBlack = false;
                    
                    if (boardStory.Count % 2 == 0) tempIsBlack = true;
                    if (board[i][j] > 0 || (board[i][j] < 0 && tempIsBlack)) {
                        bool isMove = false;
                        List<List<bool>> allowedMoves = GetAllMoves(new List<int> { i, j }, tempIsBlack, false);
                        for (int k = 0; k < allowedMoves.Count; k++)
                        {
                            List<bool> listtwo = allowedMoves[k];
                            for (int l = 0; l < listtwo.Count; l++)
                            {
                                bool celltwo = listtwo[l];
                                if (celltwo)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                
            }
        }
        return true;
    }*/


    private bool isCheck(List<List<int>> board)
    {
        bool tempIsBlack = true;
        if (boardStory.Count % 2 == 0)
        {
            tempIsBlack = false;
        }

        for (int i = 0; i < board.Count; i++)
        {
            List<int> list = board[i];
            for (int j = 0; j < list.Count; j++)
            {
                int cell = list[j];
                if (cell != 0)
                {                  
                    List<List<bool>> allowedMoves = GetAllMoves(new List<int> { i, j }, tempIsBlack, true);
                    for (int k = 0; k < allowedMoves.Count; k++) {
                        List<bool> listtwo = allowedMoves[k];
                        for (int l = 0; l < listtwo.Count; l++) {
                            bool celltwo = listtwo[l];
                            if (celltwo) {
                                if (board[k][l] == 6 || board[k][l] == -6) return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
		

    public List<List<bool>> GetAllMoves(List<int> piecePosition, bool isBlack, bool isAlt = false) {
            List<List<bool>> result = new List<List<bool>>();
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });
            result.Add(new List<bool> { false, false, false, false, false, false, false, false });

            List<List<int>> tempBoard = new List<List<int>>();
        for (int i = 0; i < 8; i++)
        {
            tempBoard.Add(new List<int>());
            for (int j = 0; j < 8; j++)
            {
                tempBoard[i].Add(boardStory[boardStory.Count - 1][i][j]);
            }
        }

            List<List<int>> board; 
            int piece = tempBoard[piecePosition[0]][piecePosition[1]];
            int coordY;
            int coordX; 
            if (isBlack)
            {
                piece *= -1;
                board = new List<List<int>>();
                for (int i = tempBoard.Count - 1; i > -1; i--) {
                    List<int> templist = tempBoard[i];
                    List<int> list = new List<int>();
                    for (int j = templist.Count - 1; j > -1; j--) {
                        list.Add(templist[j] * -1);
                    }
                    board.Add(list);
                }
                coordY = 7-piecePosition[0];
                coordX = 7-piecePosition[1];
            }
            else {
                board = new List<List<int>>(tempBoard);
                coordY = piecePosition[0];
                coordX = piecePosition[1];
            }

        //showBoard(tempBoard);
        int color = 1;
        if (isBlack)
        {
            color = -1;
        }

        if (piece == 0)
            {

            }
            else if (piece == 1)
            { //PAWN

            //Hit move
            if (coordX - 1 >= 0 || coordX + 1 < 8 && coordY == 4)
            {
                if (isEnPassant(piecePosition, new List<int> { piecePosition[0] + color, piecePosition[1] - 1 }))
                {
                    result[coordY + 1][coordX - 1] = true;
                }
                else if (isEnPassant(piecePosition, new List<int> { piecePosition[0] + color, piecePosition[1] + 1 }))
                {
                    result[coordY + 1][coordX + 1] = true;
                }
            }

            if (coordX - 1 >= 0 && coordY + 1 < 8)
                {
                    if (board[coordY + 1][coordX - 1] < 0)
                    {
                        result[coordY + 1][coordX - 1] = true;
                    }
                }
                if (coordX + 1 < 8 && coordY + 1 < 8)
                {
                    if (board[coordY + 1][coordX + 1] < 0)
                    {
                        result[coordY + 1][coordX + 1] = true;
                    }
                }
                //Normal Move
                if (coordY + 1 < 8 && board[coordY + 1][coordX] == 0)
                {
                    result[coordY + 1][coordX] = true;
                }
                if (coordY == 1 && board[coordY + 2][coordX] == 0 && board[coordY + 1][coordX] == 0)
                {
                    result[coordY + 2][coordX] = true;
                }
            }
            else if (piece == 2) //ROOK
            {
                if (coordY + 1 < 8)//UP
                {
                    for (int i = coordY + 1; i < 8; i++)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordY - 1 >= 0)//DOWN
                {
                    for (int i = coordY - 1; i >= 0; i--)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordX + 1 < 8)
                {
                    for (int i = coordX + 1; i < 8; i++)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
                if (coordX - 1 >= 0)
                {
                    for (int i = coordX - 1; i >= 0; i--)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
            }
            else if (piece == 3) //knight
            {
                if (coordY + 2 < 8)
                {
                    if (coordX - 1 >= 0)
                    {
                        if (board[coordY + 2][coordX - 1] <= 0)
                        {
                            result[coordY + 2][coordX - 1] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY + 2][coordX + 1] <= 0)
                        {
                            result[coordY + 2][coordX + 1] = true;
                        }
                    }
                }
                if (coordY - 2 >= 0)
                {
                    if (coordX - 1 >= 0)
                    {
                        if (board[coordY - 2][coordX - 1] <= 0)
                        {
                            result[coordY - 2][coordX - 1] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY - 2][coordX + 1] <= 0)
                        {
                            result[coordY - 2][coordX + 1] = true;
                        }
                    }
                }
                if (coordX + 2 < 8)
                {
                    if (coordY - 1 >= 0)
                    {
                        if (board[coordY - 1][coordX + 2] <= 0)
                        {
                            result[coordY - 1][coordX + 2] = true;
                        }
                    }
                    if (coordX + 1 < 8)
                    {
                        if (board[coordY + 1][coordX + 2] <= 0)
                        {
                            result[coordY + 1][coordX + 2] = true;
                        }
                    }
                }
                if (coordX - 2 >= 0)
                {
                    if (coordY - 1 >= 0)
                    {
                        if (board[coordY - 1][coordX - 2] <= 0)
                        {
                            result[coordY - 1][coordX - 2] = true;
                        }
                    }
                    if (coordY + 1 < 8)
                    {
                        if (board[coordY + 1][coordX - 2] <= 0)
                        {
                            result[coordY + 1][coordX - 2] = true;
                        }
                    }
                }
                
            }
            else if (piece == 4) //bishof
            {
                int i = 1;
                while (coordX + i < 8 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX + i] > 0) break;
                    result[coordY + i][coordX + i] = true;
                    if (board[coordY + i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX + i < 8 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX + i] > 0) break;
                    result[coordY - i][coordX + i] = true;
                    if (board[coordY - i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX - i] > 0) break;
                    result[coordY + i][coordX - i] = true;
                    if (board[coordY + i][coordX - i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX - i] > 0) break;
                    result[coordY - i][coordX - i] = true;
                    if (board[coordY - i][coordX - i] < 0) break;
                    i++;
                }
            }
            else if (piece == 5) { //queen
                int i = 1;
                while (coordX + i < 8 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX + i] > 0) break;
                    result[coordY + i][coordX + i] = true;
                    if (board[coordY + i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX + i < 8 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX + i] > 0) break;
                    result[coordY - i][coordX + i] = true;
                    if (board[coordY - i][coordX + i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY + i < 8)
                {
                    if (board[coordY + i][coordX - i] > 0) break;
                    result[coordY + i][coordX - i] = true;
                    if (board[coordY + i][coordX - i] < 0) break;
                    i++;
                }
                i = 1;
                while (coordX - i >= 0 && coordY - i >= 0)
                {
                    if (board[coordY - i][coordX - i] > 0) break;
                    result[coordY - i][coordX - i] = true;
                    if (board[coordY - i][coordX - i] < 0) break;
                    i++;
                }
                if (coordY + 1 < 8)//UP
                {
                    for ( i = coordY + 1; i < 8; i++)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordY - 1 >= 0)//DOWN
                {
                    for ( i = coordY - 1; i >= 0; i--)
                    {
                        if (board[i][coordX] > 0)
                        {
                            break;
                        }
                        else if (board[i][coordX] < 0)
                        {
                            result[i][coordX] = true;
                            break;
                        }
                        else
                        {
                            result[i][coordX] = true;
                        }

                    }

                }
                if (coordX + 1 < 8)
                {
                    for ( i = coordX + 1; i < 8; i++)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
                if (coordX - 1 >= 0)
                {
                    for ( i = coordX - 1; i >= 0; i--)
                    {
                        if (board[coordY][i] > 0)
                        {
                            break;
                        }
                        else if (board[coordY][i] < 0)
                        {
                            result[coordY][i] = true;
                            break;
                        }
                        else
                        {
                            result[coordY][i] = true;
                        }

                    }

                }
            }
            if (piece == 6) {
                if (coordY + 1 < 8 && board[coordY + 1][coordX] <= 0) result[coordY + 1][coordX] = true;
                if (coordY + 1 < 8 && coordX + 1 < 8 && board[coordY + 1][coordX + 1] <= 0) result[coordY + 1][coordX + 1] = true;
                if (coordX + 1 < 8 && board[coordY][coordX+1] <= 0) result[coordY][coordX+1] = true;
                if (coordY - 1 >= 0 && coordX + 1 < 8 && board[coordY - 1][coordX + 1] <= 0) result[coordY - 1][coordX + 1] = true;
                if (coordY - 1 >= 0 && board[coordY - 1][coordX] <= 0) result[coordY-1][coordX] = true;
                if (coordY - 1 >= 0 && coordX - 1 >= 0 && board[coordY - 1][coordX - 1] <= 0) result[coordY - 1][coordX - 1] = true;
                if (coordX - 1 >=0 && board[coordY][coordX - 1] <= 0) result[coordY][coordX - 1] = true;
                if (coordY + 1 < 8 && coordX - 1 >= 0 && board[coordY + 1][coordX - 1] <= 0) result[coordY + 1][coordX - 1] = true;
                if (!isAlt)
                {
                if (isBlack)
                {
                    if (isCastleAllowed(piecePosition, new List<int> { piecePosition[0], 1 }))
                    { result[0][6] = true; }
                    if (isCastleAllowed(piecePosition, new List<int> { piecePosition[0], 5 }))
                    { result[0][2] = true; }
                }
                else
                {
                    if (isCastleAllowed(piecePosition, new List<int> { piecePosition[0], 1 })) { result[piecePosition[0]][1] = true; }
                    if (isCastleAllowed(piecePosition, new List<int> { piecePosition[0], 5 })) { result[piecePosition[0]][5] = true; }
                }
            }            
        }

            if (isBlack) {
                List<List<bool>> tempResult = new List<List<bool>>();
                for (int i = result.Count - 1; i > -1; i--)
                {
                    List<bool> templist = result[i];
                    List<bool> list = new List<bool>();
                    for (int j = templist.Count - 1; j > -1; j--)
                    {
                        list.Add(templist[j]);
                        
                    }
                    tempResult.Add(list);
                }
                result = tempResult;
            }
            if (!isAlt)
            {
                for (int i = result.Count - 1; i > -1; i--)
                {
                    List<bool> list = result[i];
                    for (int j = list.Count - 1; j > -1; j--)
                    {
                        if (list[j])
                        {
                            
                            List<List<int>> altBoard = new List<List<int>>(tempBoard);
                            int old = altBoard[i][j];
                            altBoard[i][j] = altBoard[piecePosition[0]][piecePosition[1]];
                            
                            altBoard[piecePosition[0]][piecePosition[1]] = 0;
                            
                            if (isCheck(altBoard))
                            {
                            Debug.Log("check");
                                result[i][j] = false;
                            }
                            altBoard[piecePosition[0]][piecePosition[1]] = altBoard[i][j];
                            altBoard[i][j] = old;
                        }
                    }
                }
            }
            



            return result;


        }


}


   /* static void showBoard(List<List<int>> board)
    {
        //Console.Write("    | A| B| C| D| E| F| G| H|\n\n");
        for (int i = 7; i >= 0; i--)
        {
            int fi = i + 1;
            //Console.Write("[" + fi.ToString() + "] |");
            List<int> row = board[i];
            foreach (int pawn in row)
            {
                String strpawn;
                if (pawn >= 0)
                    strpawn = " " + pawn.ToString();
                else
                    strpawn = pawn.ToString();
                //Console.Write(strpawn + "|");
            }
            //Console.Write(" [" + fi.ToString() + "]\n");
        }
        //Console.Write("\n    | A| B| C| D| E| F| G| H|\n\n\n");
    }*/

    
