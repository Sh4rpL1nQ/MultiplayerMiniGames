using System;
using System.Collections.Generic;
using System.Text;
using ChessWebApp.Models.EventArguments;

namespace ChessWebApp.Models
{
    public class Player 
    {
        private TimeSpan timeRemaining;

        public int Id { get; set; }

        public string GameId { get; set; }

        public string UserName { get; set; }

        public int Points { get; set; }

        public Color Color { get; set; }

        public bool HasToMove { get; set; }

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
            set
            {
                timeRemaining = value;
                if (timeRemaining == TimeSpan.Zero)
                    OnTimeIsOver?.Invoke(this, new GameOverEventArgs(GameOver.TimeIsOver));
            }
        }

        public event EventHandler OnTimeIsOver;

        public Player(Color color)
        {
            Color = color;

            if (Color == Color.White)
                HasToMove = true;
        }
    }
}
