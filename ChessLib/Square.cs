using System;

namespace ChessLib
{
    public class Square : ICloneable
    {
        private Piece piece;

        public Color Color { get; set; }

        public Piece Piece
        {
            get { return piece; }
            set
            {
                piece = value;
                if (piece != null)
                {
                    piece.Position.PosX = Position.PosX;
                    piece.Position.PosY = Position.PosY;
                }
            }
        }

        public bool PossibleMove { get; set; }

        public bool IsSelected { get; set; }

        public Position Position { get; set; }

        public string Name { get; set; }

        public object Clone()
        {
            return new Square()
            {
                Color = Color,
                Name = Name,
                Position = Position.Clone() as Position,
                Piece = Piece?.Clone() as Piece
            };
        }

        public override string ToString()
        {
            return Piece?.Abbrevation + Name;
        }
    }
}
