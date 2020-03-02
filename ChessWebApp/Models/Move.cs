using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessWebApp.Models.Pieces;

namespace ChessWebApp.Models
{
    public class Move
    {
        private Square start;
        private Square end;
        private Board board;
        private MoveType moveType;

        public Move(Board board, Square start, Square end, MoveType moveType)
        {
            this.start = start;
            this.end = end;
            this.board = board;
            this.moveType = moveType;
        }

        public event EventHandler OnPieceCaputured;

        public Square Start => start;

        public Square End => end;

        public void Do()
        {
            //En passant
            if (moveType == MoveType.EnPassant)
            {
                board.ShiftPiece(start, end);
                var square = end.Color == Color.White ?
                            board.GetSquareAtPosition(end.Position + new Position(0, 1)) :
                            board.GetSquareAtPosition(end.Position + new Position(0, -1));

                OnPieceCaputured?.Invoke(square.Piece, new EventArgs());
                square.Piece = null;
            }

            //Castle
            if (moveType == MoveType.Castle)
            {
                var dir = start.Position.GetDirection(end.Position);
                var rooks = board.GetAllSquaresWithPieces(start.Piece.Color).Where(x => x.Piece is Rook);
                board.ShiftPiece(start, end);
                board.ShiftPiece(rooks.FirstOrDefault(x => end.Position.IsInDirection(dir, x.Position)), 
                    board.GetSquareAtPosition(end.Position + new Position(dir.PosX * -1, dir.PosY)));
            }

            //Standard
            if (moveType == MoveType.Normal)
            {
                var lostPiece = board.ShiftPiece(start, end);
                if (lostPiece != null)
                    OnPieceCaputured?.Invoke(lostPiece, new EventArgs());
            }            

            end.Piece.NumberOfMoves += 1;
        }

        public void Undo()
        {

            start.Piece.NumberOfMoves -= 1;
        }

        public Board Predict()
        {
            var predictionBoard = board.Clone() as Board;
            predictionBoard.ShiftPiece(predictionBoard.Squares.FirstOrDefault(x => x.Position == start.Position), predictionBoard.Squares.FirstOrDefault(x => x.Position == end.Position));
            return predictionBoard;
        }

        public override string ToString()
        {
            return start.ToString() + "->" + end.ToString();
        }
    }

    public enum MoveType
    {
        Normal,
        Castle,
        EnPassant
    }
}
