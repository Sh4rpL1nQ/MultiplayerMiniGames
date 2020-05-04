using System;

namespace LudoLib
{
    public class Game
    {
        public string Id { get; set; }

        public Game(Player p1, Player p2, Player p3, Player p4)
        {
            LudoBoard = new Board();
            LudoBoard.MakeBoard();

            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;

            Id = Guid.NewGuid().ToString("d");

            P1.GameId = Id;
            P2.GameId = Id;
            P3.GameId = Id;
            P4.GameId = Id;
        }

        public Player P1 { get; set; }

        public Player P2 { get; set; }

        public Player P3 { get; set; }

        public Player P4 { get; set; }

        public Board LudoBoard { get; set; }
    }
}
