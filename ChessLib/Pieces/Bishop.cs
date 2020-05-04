using System.Collections.Generic;

namespace ChessLib.Pieces
{
    public class Bishop : Piece
    {
        public override string Abbrevation => "B";

        public override IEnumerable<Position> Directions =>
            new List<Position>() { new Position(1, -1), new Position(1, 1), new Position(-1, -1), new Position(-1, 1) };

        public Bishop(Color color) : base(color)
        {
        }

        public override bool IsReachable(Position dir, Square square)
        {
            return dir != null;
        }

        public override object Clone()
        {
            return new Bishop(Color)
            {
                NumberOfMoves = NumberOfMoves,
                Position = Position.Clone() as Position
            };
        }
    }
}
