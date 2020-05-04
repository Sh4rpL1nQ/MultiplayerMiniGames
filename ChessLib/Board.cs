using ChessLib.EventArguments;
using ChessLib.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLib
{
    public class Board : ICloneable
    {
        private string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        private Dictionary<string, Piece> setup = new Dictionary<string, Piece>()
        {
            { "A1", new Rook(Color.White) },  { "B1", new Knight(Color.White) }, { "C1", new Bishop(Color.White) }, { "D1", new King(Color.White) },
            { "E1", new Queen(Color.White) }, { "F1", new Bishop(Color.White) }, { "G1", new Knight(Color.White) }, { "H1", new Rook(Color.White) },
            { "A2", new Pawn(Color.White) },  { "B2", new Pawn(Color.White) },   { "C2", new Pawn(Color.White) },   { "D2", new Pawn(Color.White) },
            { "E2", new Pawn(Color.White) },  { "F2", new Pawn(Color.White) },   { "G2", new Pawn(Color.White) },   { "H2", new Pawn(Color.White) },

            { "A7", new Pawn(Color.Black) },  { "B7", new Pawn(Color.Black) },   { "C7", new Pawn(Color.Black) },   { "D7", new Pawn(Color.Black) },
            { "E7", new Pawn(Color.Black) },  { "F7", new Pawn(Color.Black) },   { "G7", new Pawn(Color.Black) },   { "H7", new Pawn(Color.Black) },
            { "A8", new Rook(Color.Black) },  { "B8", new Knight(Color.Black) }, { "C8", new Bishop(Color.Black) }, { "D8", new King(Color.Black) },
            { "E8", new Queen(Color.Black) }, { "F8", new Bishop(Color.Black) }, { "G8", new Knight(Color.Black) }, { "H8", new Rook(Color.Black) }            
        };

        public List<Square> Squares { get; set; }

        private List<Move> currentPossibleMoves;

        public Board()
        {
            Squares = new List<Square>();

            currentPossibleMoves = new List<Move>();
        }

        public void MakeBoard()
        {
            var toggle = Color.White;
            Squares = new List<Square>();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var name = letters[x] + (8 - y);
                    var square = new Square()
                    {
                        Position = new Position(x, 7 - y),
                        Piece = setup.FirstOrDefault(x => x.Key.Equals(name)).Value,
                        Color = toggle,
                        Name = name
                    };
                    if (x != 7)
                        toggle = toggle == Color.White ? Color.Black : Color.White;

                    Squares.Add(square);
                }
            }
        }

        private Square GetKingSquare(Color color)
        {
            var king = Squares.FirstOrDefault(x => x.Piece is King && x.Piece?.Color == color);
            if (king != null)
                return king;

            return null;
        }

        private bool IsKingChecked(Color color, Move move)
        {
            var prediction = move.Predict();
            var king = prediction.GetKingSquare(color);
            var allEnemySquares = prediction.GetAllSquaresWithPieces(Switch(color));

            if (king == null)
                return false;

            foreach (var enemySquare in allEnemySquares)
                if (enemySquare.Piece.CanBeMovedTo(king, prediction))
                    return true;

            return false;
        }

        private Square IsKingChecked(Color color)
        {
            var enemySquares = GetAllSquaresWithPieces(Switch(color));
            var king = GetKingSquare(color);
            if (king == null)
                return null;

            foreach (var enemySquare in enemySquares)
                if (enemySquare.Piece.CanBeMovedTo(king, this))
                    return enemySquare;

            return null;
        }

        private Color Switch(Color color)
        {
            return color == Color.White ? Color.Black : Color.White;
        }

        public IEnumerable<Square> GetAllSquaresWithPieces(Color color)
        {
            foreach (var square in Squares)
                if (square.Piece?.Color == color)
                    yield return square;

        }

        public IEnumerable<Move> CalculatePossibleMoves(Color color, Square start)
        {
            var list = new List<Move>();
            foreach (var square in Squares)
            {
                if (square.Position == start.Position)
                    continue;

                var passantMove = CheckEnPassant(start, square);
                if (passantMove != null)
                    list.Add(passantMove);

                var castle = CheckCastle(start, square);
                if (castle != null)
                    list.Add(castle);

                if (start.Piece.CanBeMovedTo(square, this))
                {
                    var move = new Move(this, start, square, MoveType.Normal);
                    if (!IsKingChecked(color, move))
                        list.Add(move);
                }
            }

            return list;
        }

        private Move CheckEnPassant(Square start, Square end)
        {
            Square square = null;
            if (start.Piece.Color == Color.White && start.Position.PosY == 4 &&
                (end.Position == start.Position + new Position(1, 1) && GetSquareAtPosition(start.Position + new Position(1, 0))?.Piece?.Color == Color.Black) ^
                (end.Position == start.Position + new Position(-1, 1) && GetSquareAtPosition(start.Position + new Position(-1, 0))?.Piece?.Color == Color.Black))
                square = end;

            if (start.Piece.Color == Color.Black && start.Position.PosY == 3 &&
                (end.Position == start.Position + new Position(1, -1) && GetSquareAtPosition(start.Position + new Position(1, 0))?.Piece?.Color == Color.White) ^
                (end.Position == start.Position + new Position(-1, -1) && GetSquareAtPosition(start.Position + new Position(-1, 0))?.Piece?.Color == Color.White))
                square = end;

            if (square != null)
            {
                var move = new Move(this, start, square, MoveType.EnPassant);
                if (!IsKingChecked(start.Color, move))
                    return move;
            }

            return null;
        }

        public Square GetSquareAtPosition(Position position)
        {
            return Squares.FirstOrDefault(x => x.Position == position);
        }

        public Square GetSquareAtPosition(string name)
        {
            return Squares.FirstOrDefault(x => x.Name == name);
        }

        private Move CheckCastle(Square start, Square end)
        {
            if (!(start.Piece is King)) return null;

            var rooks = GetAllSquaresWithPieces(start.Color).Where(x => x.Piece is Rook && x.Piece.NumberOfMoves == 0);

            if (rooks.Count() == 0 || start.Piece.NumberOfMoves != 0 || IsKingChecked(start.Color) != null)
                return null;

            foreach (var rook in rooks)
            {
                var squares = GetAllSquaresBetween(start.Position, rook);
                var pieces = squares.Where(x => x.Piece != null);
                squares = squares.Take(2);
                if (pieces.Count() != 0)
                    continue;

                if (squares.LastOrDefault()?.Position == end.Position)
                {
                    foreach (var m in squares)
                    {
                        var move = new Move(this, start, m, MoveType.Normal);
                        if (IsKingChecked(start.Color, move))
                            return null;
                    }

                    return new Move(this, start, end, MoveType.Castle);
                }
            }

            return null;
        }


        public GameOver CheckMateOrStalemate(Color color)
        {
            var king = GetKingSquare(color);
            var enemyPiece = IsKingChecked(color);
            var moves = CalculatePossibleMovesForPiece(king.Piece);
            if (enemyPiece != null)
            {
                var enemyPieces = GetAllSquaresWithPieces(color).Where(x => !(x.Piece is King));

                //Can be blocked?
                foreach (var piece in enemyPieces)
                {
                    var pieceMoves = king.Position.AllMovesFromStartToEnd(enemyPiece.Position, king.Position.GetDirection(enemyPiece.Position));
                    foreach (var end in pieceMoves)
                    {
                        var square = Squares.FirstOrDefault(x => x.Position == end);
                        if (piece.Piece.CanBeMovedTo(square, this))
                            return GameOver.None;
                    }
                }

                //Can be moved?
                if (moves.Count() != 0)
                    return GameOver.None;

                return GameOver.Checkmate;
            }
            else
            {
                foreach (var piece in GetAllSquaresWithPieces(color))
                    if (CalculatePossibleMovesForPiece(piece.Piece).Count() != 0)
                        return GameOver.None;

                return GameOver.Stalemate;
            }
        }

        public bool CheckDraw()
        {
            return Squares.Where(x => x.Piece != null).Count() == 2;
        }

        public IEnumerable<Square> GetAllSquaresBetween(Position a, Square b)
        {
            var start = a.Clone() as Position;
            var dir = start.GetDirection(b.Position);
            if (dir == null) yield break;

            while (start != b.Position)
            {
                start += dir;

                if (start == b.Position)
                    yield break;

                yield return GetSquareAtPosition(start);
            }
        }

        public Move MakeMove(Square start, Square end)
        {
            Move moveName = null;
            var move = currentPossibleMoves.FirstOrDefault(x => x.End.Position == end.Position && x.Start.Position == start.Position);
            if (move == null)
                return null;
            moveName = move.Clone() as Move;
            move.Do();
            ClearBoardSelections();
            return moveName;
        }

        public List<Move> CalculatePossibleMovesForPiece(Piece piece)
        {
            var list = new List<Move>();
            foreach (var move in CalculatePossibleMoves(piece.Color, Squares.FirstOrDefault(x => x.Position == piece.Position)))
            {
                move.End.PossibleMove = true;
                currentPossibleMoves.Add(move);
                list.Add(move);
            }

            return list;
        }

        public void ClearBoardSelections()
        {
            currentPossibleMoves.Clear();
            foreach (var square in Squares)
            {
                square.PossibleMove = false;
                square.IsSelected = false;
            }
        }

        public Piece ShiftPiece(Square start, Square end)
        {
            var startPiece = start.Piece.Clone() as Piece;
            var endPiece = end.Piece?.Clone() as Piece;

            start.Piece = null;
            end.Piece = startPiece;

            return endPiece;
        }

        public object Clone()
        {
            var board = new Board();

            foreach (var square in Squares)
                board.Squares.Add(square.Clone() as Square);

            return board;
        }
    }
}
