using System.Collections.Generic;

namespace ChessLib.Pieces
{
    public class Knight : Piece
    {
        public override string Abbrevation => "Kt";

        public override IEnumerable<Position> Directions =>
            new List<Position>() { new Position(1, -2), new Position(-1, -2), new Position(1, 2), new Position(-1, 2),
                                   new Position(-2, -1), new Position(2, 1), new Position(2, -1), new Position(-2, 1) };

        public Knight(Color color) : base(color)
        {

        }

        public override bool IsReachable(Position dir, Square square)
        {
            return Position + dir == square.Position;
        }

        public override object Clone()
        {
            return new Knight(Color)
            {
                NumberOfMoves = NumberOfMoves,
                Position = Position.Clone() as Position
            };
        }
    }
}
