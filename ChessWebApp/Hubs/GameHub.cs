using ChessWebApp.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessWebApp.Hubs
{
    public class GameHub : Hub
    {
        public GameState GameState { get; set; }

        public GameHub(GameState gameState)
        {
            GameState = gameState;
        }

        #region private methods

        #endregion

        public async Task FindGame(string username)
        {
            //if (gameState.IsUsernameTaken(username))
            //{
            //    Clients.Caller.usernameTaken();
            //    return;
            //}

            Player joiningPlayer =
                GameState.CreatePlayer(username, this.Context.ConnectionId);
            await Clients.Caller.SendAsync("PlayerJoined", joiningPlayer);

            // Find any pending games if any
            Player opponent = GameState.GetWaitingOpponent();
            if (opponent == null)
            {
                // No waiting players so enter the waiting pool
                GameState.AddToWaitingPool(joiningPlayer);
                await Clients.Caller.SendAsync("WaitingList");
            }
            else
            {
                joiningPlayer.Color = Color.Black;
                opponent.Color = Color.White;
                Game game = new Game(opponent, joiningPlayer);
                GameState.games[game.Id] = game;
                
                await Task.WhenAll(Groups.AddToGroupAsync(game.Player1.Id, groupName: game.Id), Groups.AddToGroupAsync(game.Player2.Id, groupName: game.Id), Clients.Group(game.Id).SendAsync("Start", game));
            }
        }

        public void MakeMove(Move move)
        {
            Player playerMakingTurn = GameState.GetPlayer(playerId: this.Context.ConnectionId);
            Player opponent;
            Game game = GameState.GetGame(playerMakingTurn, out opponent);

            // TODO: should the game check if it is the players turn?
            if (game == null || !game.MovingPlayer.Equals(playerMakingTurn))
            {
                //this.Clients.Caller.notPlayersTurn();
                return;
            }

            // Notify everyone of the valid move. Only send what is necessary (instead of sending whole board)
            move.Do();
            //Clients.Group(game.Id).piecePlaced(row, col, playerMakingTurn.Piece);

            //// check if game is over (won or tie)
            //if (!game.IsOver)
            //{
            //    // Update the turn like normal if the game is still ongoing
            //    this.Clients.Group(game.Id).updateTurn(game);
            //}
            //else
            //{
            //    // Determine how the game is over in order to display correct message to client
            //    if (game.IsTie)
            //    {
            //        // Cat's game
            //        this.Clients.Group(game.Id).tieGame();
            //    }
            //    else
            //    {
            //        // Player outright won
            //        this.Clients.Group(game.Id).winner(playerMakingTurn.Name);
            //    }

            //    // Remove the game (in any game over scenario) to reclaim resources
            //    GameState.Instance.RemoveGame(game.Id);
            //}
        }

        /// <summary>
        /// A player that is leaving should end all games and notify the opponent.
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            Player leavingPlayer = GameState.GetPlayer(playerId: this.Context.ConnectionId);

            // Only handle cases where user was a player in a game or waiting for an opponent
            if (leavingPlayer != null)
            {
                Player opponent;
                Game ongoingGame = GameState.GetGame(leavingPlayer, out opponent);
                if (ongoingGame != null)
                {
                    this.Clients.Group(ongoingGame.Id).SendAsync("OpponentLeft");
                    GameState.RemoveGame(ongoingGame.Id);
                }
            }
            return base.OnConnectedAsync();
        }

        public async Task SquareSelected(string id, int playerColor, int x, int y)
        {
            var game = GameState.games[id];
            if (!game.SetSelectedSquare(game.ChessBoard.Squares.FirstOrDefault(i => i.Position.PosX == x && i.Position.PosY == y)))
            {
                await Clients.Caller.SendAsync("UpdateBoard", game, false);
            }                
            else
            {
                game.ChessBoard.ClearBoardSelections();

                await Clients.All.SendAsync("UpdateBoard", game, true);
            }
                
        }

        private void Switch(Game game)
        {
            game.ChessBoard.Squares.Reverse();
        }

        private async void ChessBoard_MoveDone(object sender, EventArgs e)       
        {
            await Clients.All.SendAsync("UpdateBoard", sender as Board, true);
        }
    }
}
