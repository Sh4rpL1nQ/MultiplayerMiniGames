using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ChessWebApp.Models;
using ChessWebApp.Models.EventArguments;

namespace WpfChessBackend.Models
{
    public class Game
    {
        public Game()
        {
            ChessBoard = new Board();
            ChessBoard.MakeBoard(Color.White);
        }

        public void SetSelectedSquare(Square square)
        {
            if (square.Piece != null && square.Piece.Color == MovingPlayer.Color)
                ChessBoard.PlayerSelectedSquare(square, true);
            else
            {
                if (ChessBoard.PlayerSelectedSquare(square, false))
                    ChangeTurns();

                ChessBoard.ClearBoardSelections();
            }                
        }

        public event EventHandler OnGameOver;

        public Player MovingPlayer => Player1?.HasToMove ?? false ? Player1 : Player2;

        public Board ChessBoard { get; set; }

        public int Id { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        private void ChangeTurns()
        {
            if (Player1.HasToMove)
            {
                Player1.HasToMove = false;
                Player2.HasToMove = true;
            }
            else
            {

                Player2.HasToMove = false;
                Player1.HasToMove = true;
            }

            var gameOverCheck = ChessBoard.CheckMateOrStalemate(MovingPlayer.Color);
            if (gameOverCheck != GameOver.None)
                OnGameOver?.Invoke(MovingPlayer, new GameOverEventArgs(gameOverCheck));
            if (ChessBoard.CheckDraw())
                OnGameOver?.Invoke(this, new GameOverEventArgs(GameOver.Draw));
        }
    }
}
