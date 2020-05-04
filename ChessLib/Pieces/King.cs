using System.Collections.Generic;

namespace ChessLib.Pieces
{
    public class King : Piece
    {
        public override string Abbrevation => "K";

        public override IEnumerable<Position> Directions =>
            new List<Position>() { new Position(1, -1), new Position(1, 1), new Position(0, -1), new Position(-1, 0),
                                   new Position(0, 1), new Position(1, 0), new Position(-1, -1), new Position(-1, 1) };

        public King(Color color) : base(color)
        {

        }

        public override bool IsReachable(Position dir, Square square)
        {
            return Position + dir == square.Position;
        }

        public override object Clone()
        {
            return new King(Color)
            {
                NumberOfMoves = NumberOfMoves,
                Position = Position.Clone() as Position
            };
        }
    }
}
