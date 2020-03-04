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

namespace ChessWebApp.Models
{
    public class Game
    {
        public Game(Player player1, Player player2)
        {
            ChessBoard = new Board();
            ChessBoard.MakeBoard(Color.White);

            Player1 = player1;
            Player2 = player2;

            Id = Guid.NewGuid().ToString("d");

            Player1.GameId = Id;
            Player2.GameId = Id;
        }

        public Game()
        {

        }

        public bool SetSelectedSquare(Square square)
        {
            if (square.Piece != null && square.Piece.Color == MovingPlayer.Color)
                return ChessBoard.PlayerSelectedSquare(square, true);
            else
            {
                var b = ChessBoard.PlayerSelectedSquare(square, false);
                if (b)
                    ChangeTurns(); 
                return b;
            }                
        }

        public event EventHandler OnGameOver;

        public Player MovingPlayer => Player1?.HasToMove ?? false ? Player1 : Player2;

        public Board ChessBoard { get; set; }

        public string Id { get; set; }

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
