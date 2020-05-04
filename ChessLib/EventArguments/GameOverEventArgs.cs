using System;

namespace ChessLib.EventArguments
{
    public class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(GameOver gameOver)
        {
            GameOver = gameOver;
        }

        public GameOver GameOver { get; set; }
    }

    public enum GameOver
    {
        None,
        Checkmate,
        Stalemate,
        Resigning,
        TimeIsOver,
        Draw,
        Repitition
    }
}
