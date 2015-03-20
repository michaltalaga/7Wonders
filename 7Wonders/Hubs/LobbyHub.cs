using _7Wonders.Models.Game;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Hubs
{
	[Authorize]
	public class LobbyHub : BaseHub<ILobbyClient>
	{
		public void Initialize()
		{
			Clients.Caller.GameListChanged(Game.Games.Select(g => new GameListItem(g)));
			Clients.All.PlayerListChanged(Player.Players.Select(p => new PlayerListItem(p)));
			
		}
		public void CreateGame(string name)
		{
			Game.CreateGame(name);
			Clients.All.GameListChanged(Game.Games.Select(g => new GameListItem(g)));
		}


	}
	public interface ILobbyClient
	{
		void GameListChanged(IEnumerable<GameListItem> games);
		void PlayerListChanged(IEnumerable<PlayerListItem> players);
		void Error(string message);

	}
}