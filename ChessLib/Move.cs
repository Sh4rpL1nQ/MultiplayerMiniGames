using ChessLib.Pieces;
using System;
using System.Linq;

namespace ChessLib
{
    public class Move : ICloneable
    {
        private Board board;
        private MoveType moveType;

        public Move(Board board, Square start, Square end, MoveType moveType)
        {
            Start = start;
            End = end;
            this.board = board;
            this.moveType = moveType;
        }

        public Move()
        {

        }

        public event EventHandler OnPieceCaputured;

        public Square Start { get; set; }

        public Square End { get; set; }

        public string Do()
        {
            Piece lostPiece = null;
            //En passant
            if (moveType == MoveType.EnPassant)
            {
                board.ShiftPiece(Start, End);
                var square = End.Color == Color.White ?
                            board.GetSquareAtPosition(End.Position + new Position(0, 1)) :
                            board.GetSquareAtPosition(End.Position + new Position(0, -1));

                OnPieceCaputured?.Invoke(square.Piece, new EventArgs());
                lostPiece = square.Piece;
                square.Piece = null;
            }

            //Castle
            if (moveType == MoveType.Castle)
            {
                var dir = Start.Position.GetDirection(End.Position);
                var rooks = board.GetAllSquaresWithPieces(Start.Piece.Color).Where(x => x.Piece is Rook);
                board.ShiftPiece(Start, End);
                board.ShiftPiece(rooks.FirstOrDefault(x => End.Position.IsInDirection(dir, x.Position)),
                    board.GetSquareAtPosition(End.Position + new Position(dir.PosX * -1, dir.PosY)));
            }

            //Standard
            if (moveType == MoveType.Normal)
            {
                lostPiece = board.ShiftPiece(Start, End);
                if (lostPiece != null)
                    OnPieceCaputured?.Invoke(lostPiece, new EventArgs());
            }

            End.Piece.NumberOfMoves += 1;

            return lostPiece?.GetType().Name;
        }

        public void Undo()
        {

            Start.Piece.NumberOfMoves -= 1;
        }

        public Board Predict()
        {
            var predictionBoard = board.Clone() as Board;
            predictionBoard.ShiftPiece(predictionBoard.Squares.FirstOrDefault(x => x.Position == Start.Position), predictionBoard.Squares.FirstOrDefault(x => x.Position == End.Position));
            return predictionBoard;
        }

        public override string ToString()
        {
            return Start.ToString() + "->" + End.ToString();
        }

        public object Clone()
        {
            return new Move()
            {
                Start = Start.Clone() as Square,
                End = End.Clone() as Square
            };
        }
    }

    public enum MoveType
    {
        Normal,
        Castle,
        EnPassant
    }
}
