using SkribblLib;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessWebApp.Hubs
{
    public class ScribbleHub : Hub
    {
        #region Private Variables
        private readonly ConcurrentDictionary<string, Player> players =
            new ConcurrentDictionary<string, Player>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, Game> games =
            new ConcurrentDictionary<string, Game>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentQueue<Player> waitingPlayers =
            new ConcurrentQueue<Player>();
        #endregion

        #region Private Methods
        private Game GetGame(Player player, out Player opponent)
        {
            opponent = null;
            Game foundGame = games.Values.FirstOrDefault(g => g.Id == player.GameId);

            if (foundGame == null)
                return null;

            //opponent = (player.Id == foundGame.Player1.Id) ? foundGame.Player2 : foundGame.Player1;
            return foundGame;
        }

        private void RemoveGame(string gameId)
        {
            Game foundGame;
            if (!this.games.TryRemove(gameId, out foundGame))
                return;

            //players.TryRemove(foundGame.Player1.Id, out Player foundPlayer1);
            //players.TryRemove(foundGame.Player2.Id, out Player foundPlayer2);
        }
        #endregion



        public override Task OnDisconnectedAsync(Exception exception)
        {
            Player leavingPlayer;
            players.TryGetValue(Context.ConnectionId, out leavingPlayer);

            if (leavingPlayer != null)
            {
                Player opponent;
                Game ongoingGame = GetGame(leavingPlayer, out opponent);
                if (ongoingGame != null)
                {
                    Clients.Group(ongoingGame.Id).SendAsync("OpponentLeft", leavingPlayer, opponent);
                    RemoveGame(ongoingGame.Id);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
