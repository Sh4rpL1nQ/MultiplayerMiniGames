﻿using ChessWebApp.Models.Pieces;
using System;
using System.Linq;

namespace ChessWebApp.Models
{
    public class Move
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

        public event EventHandler OnPieceCaputured;

        public Square Start { get; set; }

        public Square End { get; set; }

        public void Do()
        {
            //En passant
            if (moveType == MoveType.EnPassant)
            {
                board.ShiftPiece(Start, End);
                var square = End.Color == Color.White ?
                            board.GetSquareAtPosition(End.Position + new Position(0, 1)) :
                            board.GetSquareAtPosition(End.Position + new Position(0, -1));

                OnPieceCaputured?.Invoke(square.Piece, new EventArgs());
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
                var lostPiece = board.ShiftPiece(Start, End);
                if (lostPiece != null)
                    OnPieceCaputured?.Invoke(lostPiece, new EventArgs());
            }

            End.Piece.NumberOfMoves += 1;
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
    }

    public enum MoveType
    {
        Normal,
        Castle,
        EnPassant
    }
}