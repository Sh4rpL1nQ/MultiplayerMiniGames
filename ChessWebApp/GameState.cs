using ChessWebApp.Hubs;
using ChessWebApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessWebApp
{
    public class GameState
    { 

        /// <summary>
        /// A reference to all players. Key is the unique ID of the player.
        /// Note that this collection is concurrent to handle multiple threads.
        /// </summary>
        private readonly ConcurrentDictionary<string, Player> players =
            new ConcurrentDictionary<string, Player>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// A reference to all games. Key is the group name of the game.
        /// Note that this collection uses a concurrent dictionary to handle multiple threads.
        /// </summary>
        public readonly ConcurrentDictionary<string, Game> games =
            new ConcurrentDictionary<string, Game>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// A queue of players that are waiting for an opponent.
        /// </summary>
        private readonly ConcurrentQueue<Player> waitingPlayers =
            new ConcurrentQueue<Player>();

        public Player CreatePlayer(string username, string connectionId)
        {
            var player = new Player(username, connectionId);
            players[connectionId] = player;

            return player;
        }

        /// <summary>
        /// Retrieves the player that has the given ID.
        /// </summary>
        /// <param name="playerId">The unique identifier of the player to find.</param>
        /// <returns>The found player; otherwise null.</returns>
        public Player GetPlayer(string playerId)
        {
            Player foundPlayer;
            if (!this.players.TryGetValue(playerId, out foundPlayer))
            {
                return null;
            }

            return foundPlayer;
        }

        /// <summary>
        /// Retrieves the game that the given player is playing in.
        /// </summary>
        /// <param name="playerId">The player in the game.</param>
        /// <param name="opponent">The opponent of the player if there is one; otherwise null.</param>
        /// <returns>The game that the specified player is a member of if game is found; otherwise null.</returns>
        public Game GetGame(Player player, out Player opponent)
        {
            opponent = null;
            Game foundGame = this.games.Values.FirstOrDefault(g => g.Id == player.GameId);

            if (foundGame == null)
            {
                return null;
            }

            opponent = (player.Id == foundGame.Player1.Id) ?
                foundGame.Player2 :
                foundGame.Player1;

            return foundGame;
        }

        /// <summary>
        /// Retrieves a game waiting for players.
        /// </summary>
        /// <returns>Returns a pending game if any; otherwise returns null.</returns>
        public Player GetWaitingOpponent()
        {
            Player foundPlayer;
            if (!this.waitingPlayers.TryDequeue(out foundPlayer))
            {
                return null;
            }

            return foundPlayer;
        }

        /// <summary>
        /// Forgets the specified game. Use if the game is over.
        /// No need to manually remove a user from a group when the connection ends.
        /// </summary>
        /// <param name="gameId">The unique identifier of the game.</param>
        /// <returns>A task to track the asynchronous method execution.</returns>
        public void RemoveGame(string gameId)
        {
            // Remove the game
            Game foundGame;
            if (!this.games.TryRemove(gameId, out foundGame))
            {
                throw new InvalidOperationException("Game not found.");
            }

            // Remove the players, best effort
            Player foundPlayer;
            this.players.TryRemove(foundGame.Player1.Id, out foundPlayer);
            this.players.TryRemove(foundGame.Player2.Id, out foundPlayer);
        }

        /// <summary>
        /// Adds specified player to the waiting pool.
        /// </summary>
        /// <param name="player">The player to add to waiting pool.</param>
        public void AddToWaitingPool(Player player)
        {
            this.waitingPlayers.Enqueue(player);
        }

        /// <summary>
        /// Determines if the username is already taken, ignoring case.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>true if another player shares the same username; otherwise false.</returns>
        public bool IsUsernameTaken(string username)
        {
            return this.players.Values.FirstOrDefault(player => player.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase)) != null;
        }
    }
}
