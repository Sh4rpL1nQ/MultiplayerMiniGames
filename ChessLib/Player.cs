using ChessLib.EventArguments;
using System;
using System.Threading;

namespace ChessLib
{
    public class Player
    {
        private TimeSpan startingTime;
        private Timer timer;
        private string timeRemaining;

        public string Id { get; set; }

        public string GameId { get; set; }

        public string UserName { get; set; }

        public int Points { get; set; }

        public Color Color { get; set; }

        public bool HasToMove { get; set; }

        public string TimeRemaining
        {
            get { return timeRemaining; }
            set { timeRemaining = value; }
        }


        public event EventHandler OnTimeIsOver;

        public Player(Color color)
        {
            Color = color;
            InitializeMove();
        }

        public Player(string userName, string connectionId)
        {
            UserName = userName;
            Id = connectionId;
            InitializeMove();
        }


        public void InitializeMove()
        {
            if (Color == Color.White)
                HasToMove = true;
        }
    }
}
