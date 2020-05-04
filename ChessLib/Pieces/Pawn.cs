using System.Collections.Generic;

namespace ChessLib.Pieces
{
    public class Pawn : Piece
    {
        public override string Abbrevation => "P";

        public override IEnumerable<Position> Directions => (Color == Color.White) ?
            new List<Position>() { new Position(1, 1), new Position(-1, 1), new Position(0, 1) } :
            new List<Position>() { new Position(1, -1), new Position(-1, -1), new Position(0, -1) };

        public Pawn(Color color) : base(color)
        {

        }

        public override bool IsReachable(Position dir, Square square)
        {
            var next = Position + dir;

            if (dir.PosX == 0 && (next == square.Position ^ ((next + dir) == square.Position && NumberOfMoves == 0)) && square.Piece == null)
                return true;

            if (next == square.Position && dir.PosX != 0 && square.Piece != null)
                return true;

            return false;
        }

        public override object Clone()
        {
            return new Pawn(Color)
            {
                NumberOfMoves = NumberOfMoves,
                Position = Position.Clone() as Position
            };
        }
    }
}
