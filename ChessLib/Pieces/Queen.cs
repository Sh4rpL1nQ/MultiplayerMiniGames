using System.Collections.Generic;

namespace ChessLib.Pieces
{
    public class Queen : Piece
    {
        public override string Abbrevation => "Q";

        public override IEnumerable<Position> Directions =>
            new List<Position>() { new Position(1, -1), new Position(1, 1), new Position(0, -1), new Position(-1, 0),
                                   new Position(0, 1), new Position(1, 0), new Position(-1, -1), new Position(-1, 1) };

        public Queen(Color color) : base(color)
        {

        }

        public override bool IsReachable(Position dir, Square square)
        {
            return dir != null;
        }

        public override object Clone()
        {
            return new Queen(Color)
            {
                NumberOfMoves = NumberOfMoves,
                Position = Position.Clone() as Position
            };
        }
    }
}
