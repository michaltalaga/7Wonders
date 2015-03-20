using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using _7Wonders.Infrastructure;
using _7Wonders.Models.Game;
using Newtonsoft.Json;
using System.IO;
using TypeLite;
using TypeLite.Net4;
using Newtonsoft.Json.Schema;
using System.Web.Hosting;
using System.Threading.Tasks;

namespace _7Wonders.Hubs
{

	[Authorize]
	public class GameHub : BaseHub<IGameClient>
	{
		public void Rejoin()
		{
			var game = Game.FindGameUserPlays(UserId);
			Clients.Caller.GameDetailsChanged(new GameDetails(game, UserId));
			var player = game.Players.Single(p => p.UserId == UserId);
			var playerState = game.GetPlayerState(player);
			var turn = new GameTurn()
			{
				Age = game.CurrentAge,
				TurnNumber = game.CurrentTurn,
				CardsInHand = playerState.CardsInHand,
				CardsPlayed = playerState.CardsPlayed,
				Gold = playerState.Gold,
			};
			Clients.Caller.NewTurn(turn);
		}
		public void WatchGame(Guid id)
		{
			var gameBeingWatched = Game.FindGameUserWatches(UserId);
			if (gameBeingWatched != null)
			{
				gameBeingWatched.RemoveWatcher(UserId);
			}
			var game = Game.FindById(id);
			game.AddWatcher(UserId);
			Clients.Caller.GameDetailsChanged(new GameDetails(game, UserId));
		}
		public void StopWatchingGame()
		{
			var game = Game.FindGameUserWatches(UserId);
			game.RemoveWatcher(UserId);
			Clients.Caller.GameDetailsChanged(null);
		}
		public void JoinGame(Guid id)
		{
			var game = Game.FindById(id);
			game.Join(UserId);
			foreach (var watchingUserId in game.WatchingUsersIds)
			{
				Clients.Client(GetConnectionId(watchingUserId)).GameDetailsChanged(new GameDetails(game, watchingUserId));
			}
		}
		public void LeaveGame()
		{
			var game = Game.FindGameUserPlays(UserId);
			game.Leave(UserId);
			foreach (var watchingUserId in game.WatchingUsersIds)
			{
				Clients.Client(GetConnectionId(watchingUserId)).GameDetailsChanged(new GameDetails(game, watchingUserId));
			}
		}

		//public void GetGameDetails(Guid id)
		//{
		//	var game = Game.Games.Single(g => g.Id == id);
		//	Clients.Caller.GameDetailsChanged(new GameDetails(game, UserId));
		//}

		public void StartGame()
		{
			var game = Game.Games.Single(g => g.Players.Any(p => p.UserId == UserId));
			game.Start();
			foreach (var watchingUserId in game.WatchingUsersIds)
			{
				Clients.Client(GetConnectionId(watchingUserId)).GameDetailsChanged(new GameDetails(game, watchingUserId));
			}
			foreach (var player in game.Players)
			{
				var playerState = game.GetPlayerState(player);
				var turn = new GameTurn()
				{
					Age = game.CurrentAge,
					TurnNumber = game.CurrentTurn,
					CardsInHand = playerState.CardsInHand,
					CardsPlayed = playerState.CardsPlayed,
					Gold = playerState.Gold,
				};

				Clients.Client(GetConnectionId(player.UserId)).NewTurn(turn);
			}
		}


		public void PlayCard(Guid id)
		{
			var game = Game.Games.Single(g => g.Players.Any(p => p.UserId == UserId));
			var card = Cards.GetCards().Single(c => c.Id == id);
			game.PlayCard(UserId, card);
			foreach (var player in game.Players)
			{
				var playerState = game.GetPlayerState(player);
				var turn = new GameTurn()
				{
					Age = game.CurrentAge,
					TurnNumber = game.CurrentTurn,
					CardsInHand = playerState.CardsInHand,
					CardsPlayed = playerState.CardsPlayed,
					Gold = playerState.Gold,
				};

				Clients.Client(GetConnectionId(player.UserId)).NewTurn(turn);
			}
		}
	}




	public interface IGameClient
	{
		void GameDetailsChanged(GameDetails game);
		void GameOptionsChanged(GameOptions gameOptions);
		void NewTurn(GameTurn turn);
	}
	[TsClass]
	public class GameOptions
	{
		public GameOptions(Game game, string userId)
		{
			CanJoin = game.CanPlayerJoin(userId);
			CanLeave = game.CanPlayerLeave(userId);
			CanStart = game.CanPlayerStart(userId);
			CanStopWatching = game.CanPlayerStopWatching(userId);
		}
		public bool CanJoin { get; set; }
		public bool CanLeave { get; set; }
		public bool CanStart { get; set; }
		public bool CanStopWatching { get; set; }
	}
	[TsClass]
	public class GameDetails
	{
		public GameDetails(Game game, string userId)
		{
			Id = game.Id;
			Name = game.Name;
			IsStarted = game.IsStarted;
			Players = game.Players.Select(p => new PlayerListItem(p));
			Options = new GameOptions(game, userId);
		}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool IsStarted { get; set; }
		public IEnumerable<PlayerListItem> Players { get; set; }
		public GameOptions Options { get; set; }


	}
	[TsClass]
	public class GameListItem
	{
		public GameListItem(Game game)
		{
			Id = game.Id;
			Name = game.Name;
			TimeOfCreation = game.TimeOfCreation;
			Players = game.Players.Select(p => new PlayerListItem(p));
		}
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime TimeOfCreation { get; set; }
		public IEnumerable<PlayerListItem> Players { get; set; }
	}
	[TsClass]
	public class PlayerListItem
	{
		public PlayerListItem(Player player)
		{
			Name = player.Name;
		}
		public string Name { get; set; }
	}
	public class PlayerState
	{
		public Player Player { get; set; }
		public int Gold { get; set; }
		public List<Card> CardsPlayed { get; set; }
		public List<Card> CardsInHand { get; set; }

		public void Play(Card card)
		{
			//if (!CardsInHand.Contains(card)) throw new Exception();
			//if (CardsPlayed.Contains(card, Card.NameEqualityComparer)) throw new Exception();

			//if (card.PreviousStructure != null && CardsPlayed.Any(c => c.Name == card.PreviousStructure || c.Code == card.PreviousStructure))
			//{
			//	CardsInHand.Remove(card);
			//	CardsPlayed.Add(card);
			//}
			//else
			//{

			//	if (card.Price != null && card.Price > Gold) throw new Exception();
			//	Gold -= card.Price ?? 0;
			//	if (card.RequiredResources != null)
			//	{
			//		var producedResources = GetProducedResources();
			//		foreach (var requiredResource in card.RequiredResources)
			//		{
			//			var producedResource = producedResources.First(pr => pr.Contains(requiredResource));
			//			producedResources.Remove(producedResource);
			//		}
			//	}
			//	CardsInHand.Remove(card);
			//	CardsPlayed.Add(card);
			//}

			//if (card.Effects.Any(c => c.MoneyWorth != null))
			//{
			//	Gold += card.Effects.Where(c => c.MoneyWorth != null).Sum(c => c.MoneyWorth.Value);
			//}

		}

		private List<IEnumerable<ResourceType>> GetProducedResources()
		{
			var stack = new List<IEnumerable<ResourceType>>();
			//foreach (var card in CardsPlayed)
			//{
			//	if (card.Effects.Any(e => e.AllResourcesProduced != null))
			//	{
			//		foreach (var resource in card.Effects.Where(e => e.AllResourcesProduced != null).SelectMany(e => e.AllResourcesProduced))
			//		{
			//			stack.Add(new List<ResourceType>() { resource });
			//		}
			//	}
			//	if (card.EitherOrResourcesProduced != null)
			//	{
			//		stack.Add(card.EitherOrResourcesProduced);
			//	}
			//}
			return stack;
		}
	}
}