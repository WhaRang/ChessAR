﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BenChess
{
   public class ChessBoard
   {

      [Flags()]
      public enum BoardFlags : short
      {
         WhiteQueenSideCastlingEnded = 1,
         WhiteKingSideCastlingEnded = 2,
         BlackQueenSideCastlingEnded = 4,
         BlackKingSideCastlingEnded = 8,
         BlacksTurn = 16,
         WhiteInCheck = 32,
         BlackInCheck = 64,
         IsCheckmate = 128,
         CheckedForCheckmate = 256
      }

      private ChessPiece[,] board;
      public ChessPiece[,] Board
        {
            get => board;
            set { board = value; }
        }

      private Dictionary<ChessMove, ChessBoard> moveResults = null;
      private BoardFlags flags = 0;
      // For En Passant validation, this is set to the file of
      // any pawn that advances 2 spaces in 1 move, and cleared
      // for any other move.
      private byte pawnJumpCol = 99;
      private short value = short.MinValue;
      private const string newGame = "rnbqkbnr\npppppppp\n\n\n\n\nPPPPPPPP\nRNBQKBNR";

        

        private const string uniqueKeyDistanceCodes = "0123456789ACDEFGHIJLMOSTUVWXYZacdefghijlmostuvwxyz";

        /* Capitals are white pieces, and lowercase are black pieces.
         * Initial layout looks like:
         * rnbqkbnr
         * pppppppp
         * 
         * 
         * 
         * 
         * PPPPPPPP
         * RNBQKBNR
         */


        public static int[] GetComputerMove(List<List<List<int>>> array, int depth)
        {
            int size = array.Count;
            //ChessBoard finalBoard = new ChessBoard();
            ChessBoard finalBoard = ChessBoard.Parse("rnbqkbnr\npppppppp\n\n\n\n\nPPPPPPPP\nRNBQKBNR");

            if (size != 0)
            {
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        switch (array[size - 1][row][col])
                        {
                            case 0:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece(' ');
                                break;
                            case 1:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('P');
                                break;
                            case 2:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('R');
                                break;
                            case 3:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('N');
                                break;
                            case 4:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('B');
                                break;
                            case 5:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('Q');
                                break;
                            case 6:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('K');
                                break;
                            case -1:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('p');
                                break;
                            case -2:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('r');
                                break;
                            case -3:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('n');
                                break;
                            case -4:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('b');
                                break;
                            case -5:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('q');
                                break;
                            case -6:
                                finalBoard.board[7 - row, 7 - col] = GetChessPiece('k');
                                break;
                            default:
                                break;
                        }
                    }

                }
                //FLAGI OD ROSZAD 
                for (int h = 0; h < size; h++)
                {
                    //hetmańska biała wieża
                    if (array[h][0][0] != 2)
                    {
                        finalBoard.flags |= BoardFlags.WhiteQueenSideCastlingEnded;
                    }
                    //królewska biała wieża
                    if (array[h][0][7] != 2)
                    {
                        finalBoard.flags |= BoardFlags.WhiteKingSideCastlingEnded;
                    }
                    // biały król
                    if (array[h][0][3] != 6)
                    {
                        finalBoard.flags |= BoardFlags.WhiteKingSideCastlingEnded | BoardFlags.WhiteQueenSideCastlingEnded;
                    }
                    //hetmańska czarna wieża
                    if (array[h][7][0] != -2)
                    {
                        finalBoard.flags |= BoardFlags.BlackQueenSideCastlingEnded;
                    }
                    //królewska czarna wieża
                    if (array[h][7][7] != -2)
                    {
                        finalBoard.flags |= BoardFlags.BlackKingSideCastlingEnded;
                    }
                    //czarny król
                    if (array[h][7][3] != -6)
                    {
                        finalBoard.flags |= BoardFlags.BlackKingSideCastlingEnded | BoardFlags.BlackQueenSideCastlingEnded;
                    }

                }
                //RUCH CZARNCYH
                bool check = true;
                if (size % 2 == 0)
                {
                    finalBoard.flags ^= BoardFlags.BlacksTurn;
                    if (check)
                    {
                        finalBoard.flags ^= BoardFlags.BlackInCheck;
                    }
                }
                else
                {
                    if (check)
                    {
                        finalBoard.flags ^= BoardFlags.WhiteInCheck;
                    }
                }
            }

            finalBoard.CheckForCheck();
            ChessMove move = Evaluator.GetBestMove(finalBoard, depth);
            String ruch = move.ToString();

            char row1 = ruch[1];
            char row2 = ruch[4];
            char col1 = ruch[0];
            char col2 = ruch[3];
            int col1int = (int)col1;
            int col2int = (int)col2;
            int row1int, row2int;
            row1int = row1 - '1';
            row2int = row2 - '1';
            col1int -= 97;
            col2int -= 97;

            int[] movetab = { row1int, 7 - col1int, row2int, 7 - col2int };
            return movetab;

        }



        public static ChessBoard Parse(string boardString)
      {
         int stringPos = 0;
         ChessBoard result = new ChessBoard();

         for (int row = 0; row < 8; row++)
         {
            for (int col = 0; col < 8; col++)
            {
               if (stringPos >= boardString.Length)
                  throw new ArgumentException(string.Format("Expected row {0}, column {1} at string index {2}, but found end of string.", row, col, stringPos));
               if (boardString[stringPos] == '\n')
               {
                  col = 7;
                  continue;
               }
               else
                  result.board[row, col] = GetChessPiece(boardString[stringPos++]);
            }
            if ((stringPos < boardString.Length) && (boardString[stringPos++] != '\n'))
               throw new ArgumentException(string.Format("Expected row {0} to be terminated by a newline", row));
         }
         return result;
      }

      public override string ToString()
      {
         StringBuilder sb = new StringBuilder(80);

         for (int row = 0; row < 8; row++)
         {
            for (int col = 0; col < 8; col++)
            {
               sb.Append(GetPieceChar(board[row, col]));
            }
            sb.Append('\n');
         }

         return sb.ToString();
      }

      private static ChessPiece GetChessPiece(char piece)
      {
         switch (piece)
         {
            case ' ':
               return ChessPiece.Empty;
            case 'P':
               return ChessPiece.Pawn;
            case 'R':
               return ChessPiece.Rook;
            case 'N':
               return ChessPiece.Knight;
            case 'B':
               return ChessPiece.Bishop;
            case 'Q':
               return ChessPiece.Queen;
            case 'K':
               return ChessPiece.King;
            case 'p':
               return ChessPiece.BlackPawn;
            case 'r':
               return ChessPiece.BlackRook;
            case 'n':
               return ChessPiece.BlackKnight;
            case 'b':
               return ChessPiece.BlackBishop;
            case 'q':
               return ChessPiece.BlackQueen;
            case 'k':
               return ChessPiece.BlackKing;
            default:
               throw new ArgumentException(string.Format("Unknown piece code {0}", piece));
         }
      }

      private static char GetPieceChar(ChessPiece piece)
      {
         switch (piece)
         {
            case ChessPiece.Empty:
            case ChessPiece.Black:
               return ' ';
            case ChessPiece.Pawn:
               return 'P';
            case ChessPiece.Rook:
               return 'R';
            case ChessPiece.Knight:
               return 'N';
            case ChessPiece.Bishop:
               return 'B';
            case ChessPiece.Queen:
               return 'Q';
            case ChessPiece.King:
               return 'K';
            case ChessPiece.BlackPawn:
               return 'p';
            case ChessPiece.BlackRook:
               return 'r';
            case ChessPiece.BlackKnight:
               return 'n';
            case ChessPiece.BlackBishop:
               return 'b';
            case ChessPiece.BlackQueen:
               return 'q';
            case ChessPiece.BlackKing:
               return 'k';
            default:
               throw new ArgumentException("Invalid chess piece specified");
         }
      }

      public static ChessBoard NewGame()
      {
         return ChessBoard.Parse(newGame);
      }

      private ChessBoard()
      {
         board = new ChessPiece[8, 8];
      }

      public ChessPiece this[Coordinate coord]
      {
         get
         {
            return board[coord.row, coord.col];
         }
      }

      public ChessPiece this[int row, int col]
      {
         get
         {
            return board[row, col];
         }
      }

      public void CopyFrom(ChessBoard source)
      {
         for (int row = 0; row < 8; row++)
            for (int col = 0; col < 8; col++)
            {
               board[row, col] = source.board[row, col];
            }
         flags = source.flags;
         pawnJumpCol = source.pawnJumpCol;
      }

      public ChessBoard Clone()
      {
         ChessBoard clone = new ChessBoard();
         clone.CopyFrom(this);
         return clone;
      }

      private List<Coordinate> GetMovesForPiece(Coordinate source)
      {
         List<Coordinate> candidates = new List<Coordinate>(30);
         bool isBlack = PieceColorAt(source) == PieceColor.Black;

         switch (this[source])
         {
            case ChessPiece.Empty:
               return null;
            case ChessPiece.Pawn:
               if (source.row > 0)
               {
                  AddIfNotBlocked(candidates, source, source.row - 1, source.col, isBlack, true);
                  if (source.row == 6)
                     AddIfNotBlocked(candidates, source, 4, source.col, isBlack, true);
                  if (source.col > 0)
                  {
                     if (!AddIfTargetIsOpponent(candidates, source.row - 1, source.col - 1, isBlack))
                     {
                        // En Passant
                        if ((pawnJumpCol < 8) && (pawnJumpCol == source.col - 1) && (source.row == 3))
                        {
                           candidates.Add(new Coordinate(source.row - 1, source.col - 1));
                        }
                     }
                  }
                  if (source.col < 7)
                     if (!AddIfTargetIsOpponent(candidates, source.row - 1, source.col + 1, isBlack))
                     {
                        // En Passant
                        if ((pawnJumpCol < 8) && (pawnJumpCol == source.col + 1) && (source.row == 3))
                        {
                           candidates.Add(new Coordinate(source.row - 1, source.col + 1));
                        }
                     }
               }
               break;
            case ChessPiece.BlackPawn:
               if (source.row < 7)
               {
                  AddIfNotBlocked(candidates, source, source.row + 1, source.col, isBlack, true);
                  if (source.row == 1)
                     AddIfNotBlocked(candidates, source, 3, source.col, isBlack, true);
                  if (source.col > 0)
                     if (!AddIfTargetIsOpponent(candidates, source.row + 1, source.col - 1, isBlack))
                     {
                        // En Passant
                        if ((pawnJumpCol < 8) && (pawnJumpCol == source.col - 1) && (source.row == 4))
                        {
                           candidates.Add(new Coordinate(source.row + 1, source.col - 1));
                        }
                     }
                  if (source.col < 7)
                     if (!AddIfTargetIsOpponent(candidates, source.row + 1, source.col + 1, isBlack))
                     {
                        // En Passant
                        if ((pawnJumpCol < 8) && (pawnJumpCol == source.col + 1) && (source.row == 4))
                        {
                           candidates.Add(new Coordinate(source.row + 1, source.col + 1));
                        }
                     }
               }
               break;
            case ChessPiece.Rook:
            case ChessPiece.BlackRook:
               for (int to = 0; to < 8; to++)
               {
                  AddIfNotBlocked(candidates, source, source.row, to, isBlack);
                  AddIfNotBlocked(candidates, source, to, source.col, isBlack);
               }
               break;
            case ChessPiece.Knight:
            case ChessPiece.BlackKnight:
               if ((source.row > 0) && (source.col > 1))
                  AddIfTargetIsNotSelf(candidates, source.row - 1, source.col - 2, isBlack);
               if ((source.row > 1) && (source.col > 0))
                  AddIfTargetIsNotSelf(candidates, source.row - 2, source.col - 1, isBlack);
               if ((source.row < 7) && (source.col > 1))
                  AddIfTargetIsNotSelf(candidates, source.row + 1, source.col - 2, isBlack);
               if ((source.row < 6) && (source.col > 0))
                  AddIfTargetIsNotSelf(candidates, source.row + 2, source.col - 1, isBlack);
               if ((source.row < 6) && (source.col < 7))
                  AddIfTargetIsNotSelf(candidates, source.row + 2, source.col + 1, isBlack);
               if ((source.row < 7) && (source.col < 6))
                  AddIfTargetIsNotSelf(candidates, source.row + 1, source.col + 2, isBlack);
               if ((source.row > 0) && (source.col < 6))
                  AddIfTargetIsNotSelf(candidates, source.row - 1, source.col + 2, isBlack);
               if ((source.row > 1) && (source.col < 7))
                  AddIfTargetIsNotSelf(candidates, source.row - 2, source.col + 1, isBlack);
               break;
            case ChessPiece.Bishop:
            case ChessPiece.BlackBishop:
            case ChessPiece.Queen:
            case ChessPiece.BlackQueen:
               for (int offset = 1; offset < 8; offset++)
               {
                  if ((source.row + offset < 8) && (source.col + offset < 8))
                     AddIfNotBlocked(candidates, source, source.row + offset, source.col + offset, isBlack);
                  if ((source.row + offset < 8) && (source.col - offset >= 0))
                     AddIfNotBlocked(candidates, source, source.row + offset, source.col - offset, isBlack);
                  if ((source.row - offset >= 0) && (source.col + offset < 8))
                     AddIfNotBlocked(candidates, source, source.row - offset, source.col + offset, isBlack);
                  if ((source.row - offset >= 0) && (source.col - offset >= 0))
                     AddIfNotBlocked(candidates, source, source.row - offset, source.col - offset, isBlack);
               }
               if ((this[source] & ChessPiece.PieceMask) == ChessPiece.Queen)
               {
                  for (int to = 0; to < 8; to++)
                  {
                     AddIfNotBlocked(candidates, source, source.row, to, isBlack);
                     AddIfNotBlocked(candidates, source, to, source.col, isBlack);
                  }
               }
               break;
            case ChessPiece.King:
            case ChessPiece.BlackKing:
               if ((source.row > 0) && (source.col > 0))
                  AddIfTargetNotUnderAttack(candidates, source.row - 1, source.col - 1, isBlack);
               if (source.row > 0)
                  AddIfTargetNotUnderAttack(candidates, source.row - 1, source.col, isBlack);
               if ((source.row > 0) && (source.col < 7))
                  AddIfTargetNotUnderAttack(candidates, source.row - 1, source.col + 1, isBlack);
               if (source.col > 0)
                  AddIfTargetNotUnderAttack(candidates, source.row, source.col - 1, isBlack);
               if ((source.col < 7))
                  AddIfTargetNotUnderAttack(candidates, source.row, source.col + 1, isBlack);
               if ((source.row < 7) && (source.col > 0))
                  AddIfTargetNotUnderAttack(candidates, source.row + 1, source.col - 1, isBlack);
               if (source.row < 7)
                  AddIfTargetNotUnderAttack(candidates, source.row + 1, source.col, isBlack);
               if ((source.row < 7) && (source.col < 7))
                  AddIfTargetNotUnderAttack(candidates, source.row + 1, source.col + 1, isBlack);
               if ((this[source] == ChessPiece.BlackKing) && ((flags & BoardFlags.BlackInCheck) == 0))
               {
                  if (((flags & BoardFlags.BlackKingSideCastlingEnded) == 0)
                     && (this[0, 7] == ChessPiece.BlackRook) 
                     && !IsUnderAttack(new Coordinate(source.row, source.col + 2), PieceColor.White)
                     && !IsUnderAttack(new Coordinate(source.row, source.col + 1), PieceColor.White))
                  {
                     AddIfNotBlocked(candidates, source, source.row, source.col + 2, isBlack);
                  }
                  if (((flags & BoardFlags.BlackQueenSideCastlingEnded) == 0)
                     && ((this[source.row, 1] & ChessPiece.PieceMask) == 0) && (this[0, 0] == ChessPiece.BlackRook)
                     && !IsUnderAttack(new Coordinate(source.row, source.col - 2), PieceColor.White)
                     && !IsUnderAttack(new Coordinate(source.row, source.col - 1), PieceColor.White))
                  {
                     AddIfNotBlocked(candidates, source, source.row, source.col - 2, isBlack);
                  }
               }
               if ((this[source] == ChessPiece.King) && ((flags & BoardFlags.WhiteInCheck) == 0))
               {
                  if (((flags & BoardFlags.WhiteKingSideCastlingEnded) == 0)
                     && (this[7, 7] == ChessPiece.Rook)
                     && !IsUnderAttack(new Coordinate(source.row, source.col + 2), PieceColor.Black)
                     && !IsUnderAttack(new Coordinate(source.row, source.col + 1), PieceColor.Black))
                  {
                     AddIfNotBlocked(candidates, source, source.row, source.col + 2, isBlack);
                  }
                  if (((flags & BoardFlags.WhiteQueenSideCastlingEnded) == 0)
                     && ((this[source.row, 1] & ChessPiece.PieceMask) == 0) && (this[7, 0] == ChessPiece.Rook)
                     && !IsUnderAttack(new Coordinate(source.row, source.col - 2), PieceColor.Black)
                     && !IsUnderAttack(new Coordinate(source.row, source.col - 1), PieceColor.Black))
                  {
                     AddIfNotBlocked(candidates, source, source.row, source.col - 2, isBlack);
                  }
               }
               break;
         }
         return candidates;
      }

      private bool AddIfNotBlocked(IList<Coordinate> moveList, Coordinate source, int targetRow, int targetCol, bool isBlack, bool mustBeEmpty = false)
      {
         Coordinate target = new Coordinate(targetRow, targetCol);
         if (!IsMoveBlocked(source, target, isBlack, mustBeEmpty))
         {
            moveList.Add(target);
            return true;
         }
         return false;
      }

      private bool AddIfTargetIsNotSelf(IList<Coordinate> moveList, int row, int col, bool isBlack)
      {
         Coordinate target = new Coordinate(row, col);
         if (PieceColorAt(target) != (isBlack ? PieceColor.Black : PieceColor.White))
         {
            moveList.Add(target);
            return true;
         }
         return false;
      }

      private bool AddIfTargetNotUnderAttack(IList<Coordinate> moveList, int row, int col, bool isBlack)
      {
         if (!IsUnderAttack(new Coordinate(row, col), isBlack ? PieceColor.White : PieceColor.Black))
            return AddIfTargetIsNotSelf(moveList, row, col, isBlack);
         return false;
      }

      private bool AddIfTargetIsOpponent(IList<Coordinate> moveList, int row, int col, bool isBlack)
      {
         Coordinate target = new Coordinate(row, col);
         if (PieceColorAt(target) == (isBlack ? PieceColor.White : PieceColor.Black))
         {
            moveList.Add(target);
            return true;
         }
         return false;
      }

      private enum PieceColor
      {
         None,
         White,
         Black
      }

      private PieceColor PieceColorAt(int row, int col)
      {
         if ((this[row, col] & ChessPiece.PieceMask) == ChessPiece.Empty)
            return PieceColor.None;
         if ((this[row, col] & ChessPiece.ColorMask) == ChessPiece.Black)
            return PieceColor.Black;
         return PieceColor.White;
      }

      private PieceColor PieceColorAt(Coordinate target)
      {
         if ((this[target] & ChessPiece.PieceMask) == ChessPiece.Empty)
            return PieceColor.None;
         if ((this[target] & ChessPiece.ColorMask) == ChessPiece.Black)
            return PieceColor.Black;
         return PieceColor.White;
      }

      private bool IsMoveBlocked(Coordinate source, Coordinate target, bool isBlack, bool mustBeEmpty)
      {
         if (mustBeEmpty)
         {
            if (PieceColorAt(target) != PieceColor.None)
               return true;
         }
         else
         {
            if (PieceColorAt(target) == (isBlack ? PieceColor.Black : PieceColor.White))
               return true;
         }
         int horizontalDirection = target.col > source.col ? 1 : target.col == source.col ? 0 : -1;
         int verticalDirection = target.row > source.row ? 1 : target.row == source.row ? 0 : -1;
         int distance = source.row - target.row;
         if (distance == 0)
            distance = source.col - target.col;
         if (distance < 0)
            distance = -distance;
         for (int offset = 1; offset < distance; offset++)
         {
            Coordinate step = new Coordinate(source.row + verticalDirection * offset, source.col + horizontalDirection * offset);
            if ((this[step] & ChessPiece.PieceMask) != ChessPiece.Empty)
               return true;
         }
         return false;
      }

      private bool IsUnderAttack(Coordinate target, PieceColor byColor)
      {
         for (int row = 0; row < 8; row++)
         {
            for (int col = 0; col < 8; col++)
            {
               if (PieceColorAt(row, col) == byColor)
               {
                  switch (this[row, col])
                  {
                     case ChessPiece.BlackPawn:
                        if ((target.row == row + 1) && ((target.col == col + 1) || (target.col == col - 1)))
                           return true;
                        break;
                     case ChessPiece.Pawn:
                        if ((target.row == row - 1) && ((target.col == col + 1) || (target.col == col - 1)))
                           return true;
                        break;
                     case ChessPiece.Rook | ChessPiece.BlackRook:
                        if ((target.col != col) && (target.row != row))
                           break;
                        if (!IsMoveBlocked(new Coordinate(row, col), target, byColor == PieceColor.Black, false))
                           return true;
                        break;
                     case ChessPiece.Knight | ChessPiece.BlackKnight:
                        if ((target.row == row - 1) && (target.col == col - 2))
                           return true;
                        if ((target.row == row - 2) && (target.col == col - 1))
                           return true;
                        if ((target.row == row + 1) && (target.col == col - 2))
                           return true;
                        if ((target.row == row + 2) && (target.col == col - 1))
                           return true;
                        if ((target.row == row + 2) && (target.col == col + 1))
                           return true;
                        if ((target.row == row + 1) && (target.col == col + 2))
                           return true;
                        if ((target.row == row - 1) && (target.col == col + 2))
                           return true;
                        if ((target.row == row - 2) && (target.col == col + 1))
                           return true;
                        break;;                        
                     case ChessPiece.Bishop:
                     case ChessPiece.BlackBishop:
                        if (((target.col - target.row) != (col - row)) && ((target.col + target.row) != (col + row)))
                           break;
                        if (!IsMoveBlocked(new Coordinate(row, col), target, byColor == PieceColor.Black, false))
                           return true;
                        break;
                     case ChessPiece.Queen:
                     case ChessPiece.BlackQueen:
                        if ((target.col != col) && (target.row != row) && ((target.col - target.row) != (col - row)) && ((target.col + target.row) != (col + row)))
                           break;
                        if (!IsMoveBlocked(new Coordinate(row, col), target, byColor == PieceColor.Black, false))
                           return true;
                        break;
                     case ChessPiece.King:
                     case ChessPiece.BlackKing:
                        if ((target.col >= col - 1) && (target.row >= row-1) 
                           && (target.col <= col + 1) && (target.row <= row + 1))
                           return true;
                        break;
                  }
               }
            }
         }
         return false;
      }

      public ChessBoard Move(ChessMove move)
      {
         ChessBoard result;
         if (moveResults == null)
            GetValidMoves();
         if (moveResults.TryGetValue(move, out result))
         {
            if (result == null)
            {
               result = Clone();
               result.ApplyMove(move);
            }
            moveResults[move] = result;
            return result;
         }
         throw new ArgumentException(string.Format("{0} is not a valid move.", move));
      }

      private void ApplyMove(ChessMove move)
      {
         bool isBlack = PieceColorAt(move.Source) == PieceColor.Black;
         if (IsBlacksTurn)
         {
            if (!isBlack)
               throw new InvalidOperationException("Attempted to move a white piece on black's turn");
         }
         else
         {
            if (isBlack)
               throw new InvalidOperationException("Attempted to move a black piece on white's turn");
         }

         switch (this[move.Source] & ChessPiece.PieceMask)
         {
            case ChessPiece.Pawn:
               if ((move.Source.col != move.Target.col) && ((this[move.Target] & ChessPiece.PieceMask) == ChessPiece.Empty))
               {
                  // En Passant
                  if (isBlack)
                     board[move.Target.row - 1, move.Target.col] = ChessPiece.Empty;
                  else
                     board[move.Target.row + 1, move.Target.col] = ChessPiece.Empty;
               }
               if ((move.Target.row - move.Source.row > 1) || (move.Source.row - move.Target.row > 1))
                  pawnJumpCol = move.Source.col;
               else
                  pawnJumpCol = 99;
               break;
            case ChessPiece.Rook:
               if (move.Source.col == 0)
                  flags |= BoardFlags.WhiteQueenSideCastlingEnded;
               else flags |= BoardFlags.WhiteKingSideCastlingEnded;
               pawnJumpCol = 99;
               break;
            case ChessPiece.BlackRook:
               if (move.Source.col == 0)
                  flags |= BoardFlags.BlackQueenSideCastlingEnded;
               else flags |= BoardFlags.BlackKingSideCastlingEnded;
               pawnJumpCol = 99;
               break;
            case ChessPiece.King:
               if (isBlack)
                  flags |= BoardFlags.BlackKingSideCastlingEnded | BoardFlags.BlackQueenSideCastlingEnded;
               else
                  flags |= BoardFlags.WhiteKingSideCastlingEnded | BoardFlags.WhiteQueenSideCastlingEnded;
               if (move.Target.col - move.Source.col > 1)
               {
                  // Castling to king's rook
                  board[move.Source.row, 5] = board[move.Source.row, 7];
                  board[move.Source.row, 7] = ChessPiece.Empty;
               }
               else if (move.Source.col - move.Target.col > 1)
               {
                  // Castling to queen's rook
                  board[move.Source.row, 3] = board[move.Source.row, 0];
                  board[move.Source.row, 0] = ChessPiece.Empty;
               }
               pawnJumpCol = 99;
               break;
            case ChessPiece.Empty:
               throw new InvalidOperationException("Attempted to move from a square without a piece.");
            default:
               pawnJumpCol = 99;
               break;
         }
         if ((isBlack && (move.Target.row == 7) && (board[move.Source.row, move.Source.col] == ChessPiece.BlackPawn))
            || (!isBlack && (move.Target.row == 0) && (board[move.Source.row, move.Source.col] == ChessPiece.Pawn)))
            board[move.Target.row, move.Target.col] = move.Promotion | (isBlack ? ChessPiece.Black : 0);
         else
            board[move.Target.row, move.Target.col] = board[move.Source.row, move.Source.col];
         board[move.Source.row, move.Source.col] = ChessPiece.Empty;
         flags ^= BoardFlags.BlacksTurn;
         CheckForCheck();
      }

      private void CheckForCheck()
      {
         flags &= ~(BoardFlags.BlackInCheck | BoardFlags.WhiteInCheck);
         for (int row = 0; row < 8; row++)
            for (int col = 0; col < 8; col++)
            {
               Coordinate source = new Coordinate(row, col);
               PieceColor sourceColor = PieceColorAt(source);
               if ((this[row, col] == ChessPiece.King) && IsUnderAttack(new Coordinate(row, col), PieceColor.Black))
                  flags |= BoardFlags.WhiteInCheck;
               if ((this[row, col] == ChessPiece.BlackKing) && IsUnderAttack(new Coordinate(row, col), PieceColor.White))
                  flags |= BoardFlags.BlackInCheck;
            }
      }

      public IEnumerable<ChessMove> GetValidMoves()
      {
         if (moveResults == null)
         {
            moveResults = new Dictionary<ChessMove, ChessBoard>();
            for (int row = 0; row < 8; row++)
               for (int col = 0; col < 8; col++)
               {
                  Coordinate source = new Coordinate(row, col);
                  if ((IsBlacksTurn && (PieceColorAt(source) == PieceColor.Black))
                     || (!IsBlacksTurn && (PieceColorAt(source) == PieceColor.White)))
                  {
                     foreach (Coordinate target in GetMovesForPiece(source))
                     {
                        if (((this[source] == ChessPiece.Pawn) && (target.row == 0))
                           || ((this[source] == ChessPiece.BlackPawn) && (target.row == 7)))
                        {
                           bool bFirstPromotion = true;
                           foreach (ChessPiece promotion in new ChessPiece[] { ChessPiece.Rook, ChessPiece.Knight, ChessPiece.Bishop, ChessPiece.Queen })
                           {
                              ChessBoard clone = Clone();
                              ChessMove move = new ChessMove(source, target, promotion);
                              clone.ApplyMove(move);
                              if (bFirstPromotion && ((IsBlacksTurn && clone.IsBlackInCheck) || (!IsBlacksTurn && clone.IsWhiteInCheck)))
                                 break;
                              bFirstPromotion = false;
                              moveResults[move] = clone;
                           }
                        }
                        else
                        {
                           ChessBoard clone = Clone();
                           ChessMove move = new ChessMove(source, target);
                           clone.ApplyMove(move);
                           if (((IsBlacksTurn && !clone.IsBlackInCheck) || (!IsBlacksTurn && !clone.IsWhiteInCheck)))                              
                              moveResults[move] = clone;
                        }
                     }
                  }
               }
         }
         return moveResults.Keys;
      }

      public bool IsCheckmate
      {
         get
         {
            if ((flags & BoardFlags.CheckedForCheckmate) == 0)
            {
               flags |= BoardFlags.CheckedForCheckmate;
               if (moveResults == null)
               {
                  for (int row = 0; row < 8; row++)
                     for (int col = 0; col < 8; col++)
                     {
                        Coordinate source = new Coordinate(row, col);
                        if ((IsBlacksTurn && (PieceColorAt(source) == PieceColor.Black))
                           || (!IsBlacksTurn && (PieceColorAt(source) == PieceColor.White)))
                        {
                           if (GetMovesForPiece(source).Count > 0)
                              return false;
                        }
                     }
                  flags |= BoardFlags.IsCheckmate;
               }
               else
               {
                  if (!moveResults.Any())
                     flags |= BoardFlags.IsCheckmate;
               }
            }
            return (flags & BoardFlags.IsCheckmate) != 0;
         }
      }

      public short BoardValue
      {
         get
         {
            if (value == short.MinValue)
            {
               if (IsCheckmate)
               {
                  if (IsBlacksTurn)
                     value = 9999;
                  else
                     value = -9999;
                  return value;
               }

               short whiteValue = 0;
               short blackValue = 0;

               for (int row = 0; row < 8; row++)
                  for (int col = 0; col < 8; col++)
                  {
                     short pieceValue = 0;
                     Coordinate c = new Coordinate(row, col);
                     switch (this[c] & ChessPiece.PieceMask)
                     {
                        case ChessPiece.Pawn:
                           pieceValue = 1;
                           break;
                        case ChessPiece.Rook:
                           pieceValue = 5;
                           break;
                        case ChessPiece.Knight:
                           pieceValue = 3;
                           break;
                        case ChessPiece.Bishop:
                           pieceValue = 3;
                           break;
                        case ChessPiece.Queen:
                           pieceValue = 9;
                           break;
                     }
                     if (PieceColorAt(c) == PieceColor.Black)
                        blackValue += pieceValue;
                     else
                        whiteValue += pieceValue;
                  }
               value = (short)(whiteValue - blackValue);
            }
            return value;
         }
      }

      public string GetUniqueKey()
      {
         int distance = 0;
         StringBuilder sb = new StringBuilder();
         for (int row = 0; row < 8; row++)
            for (int col = 0; col < 8; col++)
            {
               if ((distance >= uniqueKeyDistanceCodes.Length - 1) ||
                  ((this[row, col] & ChessPiece.PieceMask) != ChessPiece.Empty))
               {
                  if (distance > 0)
                     sb.Append(uniqueKeyDistanceCodes[distance - 1]);
                  sb.Append(GetPieceChar(this[row, col]));
                  distance = 0;
               }
               else
                  distance++;
            }
         sb.Append('-');
         sb.Append(uniqueKeyDistanceCodes[(int)flags & 31]);
         return sb.ToString();
      }

      public static ChessBoard FromUniqueKey(string key)
      {
         int index = 0;
         int row = 0, col = 0;
         ChessBoard result = new ChessBoard();
         while ((index < key.Length) && (key[index] != '-'))
         {
            int distance = uniqueKeyDistanceCodes.IndexOf(key[index]);
            if (distance >= 0)
            {
               int newCoord = row * 8 + col + distance + 1;
               row = newCoord / 8;
               col = newCoord % 8;
               index++;
            }
            if (row >= 8)
               throw new ArgumentException("Invalid board code");
            result.board[row, col] = GetChessPiece(key[index++]);
            col++;
            if (col > 7)
            {
               col = 0;
               row++;
            }
         }
         if (index >= key.Length - 1)
            throw new ArgumentException("Invalid board code");
         result.flags = (BoardFlags)uniqueKeyDistanceCodes.IndexOf(key[++index]);
         result.CheckForCheck();
         return result;
      }

      public bool IsBlacksTurn
      {
         get
         {
            return ((flags & BoardFlags.BlacksTurn) != 0);
         }
      }

      public void WriteToConsole(Coordinate highlight)
      {
         WriteToConsole(highlight, true);
      }
      
      public void WriteToConsole()
      {
         WriteToConsole(new Coordinate(), false);
      }

      private void WriteToConsole(Coordinate highlight, bool doHighlight)
      {
         Console.ForegroundColor = ConsoleColor.Yellow;
         Console.WriteLine("  abcdefgh");
         for (int row = 0; row < 8; row++)
         {
            Console.Write("{0} ", "87654321"[row]);
            Console.ResetColor();
            for (int col = 0; col < 8; col++)
            {
               if (doHighlight && (highlight.row == row) && (highlight.col == col))
               {
                  Console.BackgroundColor = ConsoleColor.DarkGreen;
                  Console.ForegroundColor = ConsoleColor.White;
               }
               else if ((col + row) % 2 == 0)
               {
                  Console.BackgroundColor = ConsoleColor.Gray;
                  Console.ForegroundColor = ConsoleColor.Black;
               }
               else
               {
                  Console.ForegroundColor = ConsoleColor.Gray;
                  Console.BackgroundColor = ConsoleColor.Black;
               }
               if ((IsBlackInCheck && (this[row, col] == ChessPiece.BlackKing))
                  || (IsWhiteInCheck && (this[row, col] == ChessPiece.King)))
               {
                  if ((col + row) % 2 == 0)
                     Console.BackgroundColor = ConsoleColor.Red;
                  else
                     Console.BackgroundColor = ConsoleColor.DarkRed;
               }
               Console.Write(GetPieceChar(this[row, col]));
               Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" {0}", "87654321"[row]);
         }
         Console.WriteLine("  abcdefgh");
         Console.ResetColor();
      }

      public bool IsBlackInCheck
      {
         get
         {
            return (flags & BoardFlags.BlackInCheck) != 0;
         }
      }

      public bool IsWhiteInCheck
      {
         get
         {
            return (flags & BoardFlags.WhiteInCheck) != 0;
         }
      }
   }
}
