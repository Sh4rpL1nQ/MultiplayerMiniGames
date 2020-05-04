using ChessLib.EventArguments;
using System;

namespace ChessLib
{
    public class Game
    {
        public Game(Player player1, Player player2)
        {
            ChessBoard = new Board();
            ChessBoard.MakeBoard();

            Player1 = player1;
            Player2 = player2;

            Id = Guid.NewGuid().ToString("d");

            Player1.GameId = Id;
            Player2.GameId = Id;

            Player1.OnTimeIsOver += Player_OnTimeIsOver;
            Player2.OnTimeIsOver += Player_OnTimeIsOver;
        }

        private void Player_OnTimeIsOver(object sender, EventArgs e)
        {
            OnGameOver?.Invoke(this, e);
        }

        public Game()
        {

        }

        public Move MoveSelected(Square start, Square end)
        {
            var move = ChessBoard.MakeMove(start, end);
            if (move != null)
                ChangeTurns();

            return move;
        }

        public event EventHandler OnGameOver;

        public Player MovingPlayer => Player1?.HasToMove ?? false ? Player1 : Player2;

        public Player WaitingPlayer => Player1?.HasToMove ?? false ? Player2 : Player1;

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
                OnGameOver?.Invoke(MovingPlayer, new GameOverEventArgs(GameOver.Draw));
        }
    }
}
