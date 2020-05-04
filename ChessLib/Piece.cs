using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessLib
{
    public abstract class Piece : ICloneable
    {
        public Piece()
        {
            Position = new Position();
        }

        public Piece(Color color) : this()
        {
            Color = color;
        }

        public int NumberOfMoves { get; set; } = 0;

        public Position Position { get; set; }

        public Color Color { get; set; }

        public abstract IEnumerable<Position> Directions { get; }

        public abstract string Abbrevation { get; }

        public string Name => GetType().Name.ToString();

        private Position GetRightDirection(Square end)
        {
            return Directions.FirstOrDefault(x => Position.IsInDirection(x, end.Position));
        }

        public bool CanBeMovedTo(Square end, Board board)
        {
            var dir = GetRightDirection(end);

            var allMoves = Position.AllMovesFromStartToEnd(dir, end.Position);

            if (end.Piece?.Color == Color || !Position.IsInDirection(dir, end.Position))
                return false;

            if (IsReachable(dir, end))
            {
                foreach (var move in allMoves)
                    if (board.GetSquareAtPosition(move).Piece != null)
                        return false;

                return true;
            }

            return false;
        }

        public abstract bool IsReachable(Position dir, Square end);

        public override string ToString()
        {
            return Position.ToString();
        }

        public virtual object Clone()
        {
            return Clone();
        }
    }
}
