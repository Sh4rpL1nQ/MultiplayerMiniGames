using System;

namespace MiniGameLib
{
    public class Player
    {
        public string UserName { get; private set; }

        public string PlayerId { get; private set; }

        public string GameId { get; set; }

        public Player()
        {

        }

        public Player(string userName, string connectionId)
        {
            UserName = userName;
            PlayerId = connectionId;
        }
    }
}
