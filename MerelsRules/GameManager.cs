using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerelsRules
{
    public class GameManager
    {
        public enum Piece { Empty = 0, O = 1, X = 2 };

        private static int[,] winConditions = new int[4, 3]
        {
            { 3, 4, 5 }, { 1, 4, 7 }, 
            { 0, 4, 8 }, { 2, 4, 6 }
        };

        private const int MaxDepth = 10;

        // stores location of all pieces in board (indexes 0-9), to print out
        public Piece[] Board;
        //stores list of locations of pieces, first 3 (0-3) are player O, last 3 (3-6) are player X
        public int[] PieceLocations;
        public Piece CurrentTurn;
        public Piece Player;
        public Piece Computer;

        private int _choiceInitial;
        private int _choiceDestination;

        private static GameManager _gameManager;

        private GameManager()
        {
            Reset();
        }

        public static GameManager GetInstance()
        {
            if(_gameManager == null)
            {
                _gameManager = new GameManager();
            }
            return _gameManager;
        }

        public void Reset()
        {
            Board = new Piece[9];
            PieceLocations = new int[6];
            CurrentTurn = Piece.O;
            SetPlayer(Piece.O);
            _choiceInitial = 0;
            _choiceDestination = 0;
        }

        public void SetPlayer(Piece piece)
        {
            Player = piece;
            Computer = SwitchPiece(Player);
        }

        public Tuple<int, int> MakeMove(int moveInitial, int moveDestination)
        {
            if (CurrentTurn == Player)
            {
                var boardAndPieceLocations = MakeBoardMove(Board, PieceLocations, CurrentTurn, moveInitial, moveDestination);
                Board = boardAndPieceLocations.Item1;
                PieceLocations = boardAndPieceLocations.Item2;
                CurrentTurn = SwitchPiece(CurrentTurn);
            }
            else if (CurrentTurn == Computer)
            {
                int score = MiniMax(new Tuple<Piece[], int[]>(Board, PieceLocations), CurrentTurn, 0);
                var boardAndPieceLocations = MakeBoardMove(Board, PieceLocations, CurrentTurn, _choiceInitial, _choiceDestination);
                Board = boardAndPieceLocations.Item1;
                PieceLocations = boardAndPieceLocations.Item2;
                CurrentTurn = SwitchPiece(CurrentTurn);
                return new Tuple<int, int>(_choiceInitial, _choiceDestination);
            }
            return null;
        }

        private int MiniMax(Tuple<Piece[], int[]> boardAndLocations, Piece player, int depth)
        {
            Piece[] inputBoard = boardAndLocations.Item1;
            int[] inputPieceLocations = boardAndLocations.Item2;
            Piece[] board = new Piece[inputBoard.Length];
            Array.Copy(inputBoard, board, inputBoard.Length);

            int[] pieceLocations = new int[inputPieceLocations.Length];
            Array.Copy(inputPieceLocations, pieceLocations, inputPieceLocations.Length);

            if (CheckScore(board, Computer) != 0)
                return CheckScore(board, Computer);
            else if (CheckGameEnd(board) || depth >= MaxDepth) return 0;

            List<int> scores = new List<int>();
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

            for(int p = ((int) player - 1) * 3; p < 3 + ((int) player - 1) * 3; p++)
            {
                foreach(int m in GetMoves(board, player, pieceLocations[p])){
                    scores.Add(MiniMax(MakeBoardMove(board, pieceLocations, player, pieceLocations[p], m), SwitchPiece(player), depth + 1));
                    moves.Add(new Tuple<int, int>(pieceLocations[p], m));
                }
            }

            if (player == Computer)
            {
                int MaxScoreIndex = scores.IndexOf(scores.Max());
                _choiceInitial = moves[MaxScoreIndex].Item1;
                _choiceDestination = moves[MaxScoreIndex].Item2;
                return scores.Max();
            }
            else
            {
                int MinScoreIndex = scores.IndexOf(scores.Min());
                _choiceInitial = moves[MinScoreIndex].Item1;
                _choiceDestination = moves[MinScoreIndex].Item2;
                return scores.Min();
            }
        }

        public bool CheckGameEnd()
        {
            return CheckGameEnd(Board);
        }

        private static List<int> GetMoves(Piece[] board, Piece player, int location)
        {
            Debug.Assert(location >= 0);
            Debug.Assert(location < board.Length);

            List<int> moves = new List<int>();
            if (location + 1 < board.Length && board[location + 1] == Piece.Empty) moves.Add(location + 1);
            if (location - 1 >= 0 && board[location - 1] == Piece.Empty) moves.Add(location - 1);
            if (location + 3 < board.Length && board[location + 3] == Piece.Empty) moves.Add(location + 3);
            if (location - 3 >= 0 && board[location - 3] == Piece.Empty) moves.Add(location - 3);

            if ((location == 0 || location == 2 || location == 6 || location == 8) && board[4] == Piece.Empty) moves.Add(4);
            if (location == 4)
            {
                if (board[0] == Piece.Empty) moves.Add(0);
                if (board[2] == Piece.Empty) moves.Add(2);
                if (board[6] == Piece.Empty) moves.Add(6);
                if (board[8] == Piece.Empty) moves.Add(8);
            }

            return moves;
        }

        private static int CheckScore(Piece[] board, Piece player)
        {
            if (CheckGameWin(board, player)) return 10;

            else if (CheckGameWin(board, SwitchPiece(player))) return -10;

            else return 0;
        }

        private static bool CheckGameWin(Piece[] board, Piece player)
        {
            for (int i = 0; i <= winConditions.GetUpperBound(0); i++)
            {
                if
                (
                    board[winConditions[i, 0]] == player &&
                    board[winConditions[i, 1]] == player &&
                    board[winConditions[i, 2]] == player
                )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckGameEnd(Piece[] board)
        {
            return CheckGameWin(board, Piece.O) || CheckGameWin(board, Piece.X);
        }

        private static Piece SwitchPiece(Piece piece)
        {
            if (piece == Piece.X) return Piece.O;
            else return Piece.X;
        }

        private static Tuple<Piece[], int[]> MakeBoardMove(Piece[] board, int[] pieceLocations, Piece piece, int initial, int destination)
        {
            //changes location of piece in board
            Piece[] newBoard = new Piece[board.Length];
            Array.Copy(board, newBoard, board.Length);
            newBoard[initial] = Piece.Empty;
            newBoard[destination] = piece;

            //changes location of piece in list of piece locations
            int[] newPieceLocations = new int[pieceLocations.Length];
            Array.Copy(pieceLocations, newPieceLocations, pieceLocations.Length);
            newPieceLocations[Array.IndexOf(newPieceLocations, initial)] = destination;

            return new Tuple<Piece[], int[]>(newBoard, newPieceLocations);
        }
    }
}
