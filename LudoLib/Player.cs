using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LudoLib
{
    public class Player
    {
        public string Username { get; set; }

        public string Id { get; set; }

        public string GameId { get; set; }

        public Color Color { get; set; }

        public Player(string username, string connectionId)
        {
            Username = username;
            GameId = connectionId;
        }

        public int ThrowDice()
        {
            var random = new Random();
            return random.Next(RuleSet.MinDiceValue, RuleSet.MaxDiceValue);
        }

        public void MakeMove()
        {
            var steps = ThrowDice();
        }
    }
}
