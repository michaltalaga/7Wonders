using _7Wonders.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
	public class Game
	{
		const int MinPlayers = 1;
		const int MaxPlayers = 7;
		const int InitialCards = 7;

		private static List<Game> games = new List<Game>();
		public static IEnumerable<Game> Games
		{
			get
			{
				return games.AsReadOnly();
			}
		}
		public static Game CreateGame(string name)
		{
			var game = new Game()
			{
				Id = Guid.NewGuid(),
				Name = name,
				TimeOfCreation = DateTime.Now,
			};

			games.Add(game);
			return game;
		}

		List<string> watchingUsersIds = new List<string>();
		public IEnumerable<string> WatchingUsersIds { get { return watchingUsersIds.AsReadOnly(); } }
		public void AddWatcher(string userId)
		{
			if (watchingUsersIds.Contains(userId)) throw new Exception();
			if (players.Any(p => p.UserId == userId)) throw new Exception();
			watchingUsersIds.Add(userId);
		}
		public void RemoveWatcher(string userId)
		{
			if (!watchingUsersIds.Contains(userId)) throw new Exception(userId + " is not watching this game.");
			if (Players.Any(p => p.UserId == userId)) throw new Exception(userId + " is playing this game.");
			watchingUsersIds.Remove(userId);
		}
		public static Game FindGameUserWatches(string userId)
		{
			return Games.SingleOrDefault(g => g.WatchingUsersIds.Contains(userId));
		}
		public static Game FindGameUserPlays(string userId)
		{
			return Games.Single(g => g.Players.Any(p => p.UserId == userId));
		}

		public static Game FindById(Guid id)
		{
			return Games.Single(g => g.Id == id);
		}

		public static Game GetUserGame(string userId)
		{
			return Games.SingleOrDefault(g => g.Players.Any(p => p.Name == userId));
		}


		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime TimeOfCreation { get; set; }
		public DateTime? TimeOfStart { get; private set; }

		public void Join(string userId)
		{
			if (players.Any(p => p.UserId == userId)) throw new Exception();
			if (!watchingUsersIds.Contains(userId)) watchingUsersIds.Add(userId);
			players.Add(new Player(userId));
		}
		public void Leave(string userId)
		{
			if (!players.Any(p => p.UserId == userId)) throw new Exception();
			players.RemoveAll(p => p.UserId == userId);
		}


		public bool CanPlayerJoin(string userId)
		{
			return !IsStarted && players.Count < MaxPlayers && !players.Any(p => p.UserId == userId);

		}
		public bool CanPlayerLeave(string userId)
		{
			return !IsStarted && players.Any(p => p.UserId == userId);
		}
		public bool CanPlayerStart(string userId)
		{
			return !IsStarted && players.Any(p => p.UserId == userId) && players.Count >= MinPlayers;
		}
		public bool CanPlayerStopWatching(string userId)
		{
			return WatchingUsersIds.Contains(userId) && !Players.Any(p => p.UserId == userId);
		}
        public bool IsStarted
		{
			get
			{
				return TimeOfStart != null;
			}
		}
		public int CurrentAge { get; private set; }
		public int CurrentTurn { get; private set; }
		public void Start()
		{
			if (IsStarted || players.Count < MinPlayers || players.Count > MaxPlayers)
			{
				throw new Exception();

			}
			CurrentAge = 1;
			CurrentTurn = 1;
			TimeOfStart = DateTime.Now;
			var cards = Cards.GetShuffledCards(7);
			ageCards = new Dictionary<int, List<Card>>();
			ageCards[1] = new List<Card>(cards.Where(c => c.Age == 1));
			ageCards[2] = new List<Card>(cards.Where(c => c.Age == 2));
			ageCards[3] = new List<Card>(cards.Where(c => c.Age == 3));

			DealCards();

		}


		private Dictionary<int, List<Card>> ageCards;

		private void DealCards()
		{
			var cards = new Stack<Card>(ageCards[CurrentAge]);
			//var allCards = ageCards[1].Union(ageCards[2]).Union(ageCards[3]);
			//allCards = allCards.Where(c=>c.Name == "traders guild");
            //var cards = new Stack<Card>(allCards);
			playersStates = new Dictionary<Player, PlayerState>();
			foreach (var player in players)
			{
				var playerState = new PlayerState()
				{
					Player = player,
					Gold = 3,
					CardsInHand = new List<Card>(),
					CardsPlayed = new List<Card>(),
				};

				for (int i = 0; i < InitialCards; i++)
				{
					playerState.CardsInHand.Add(cards.Pop());
				}
				playersStates[player] = playerState;
			}
		}

		private List<Player> players = new List<Player>();
		public IEnumerable<Player> Players
		{
			get
			{
				return players.AsReadOnly();
			}
		}

		Dictionary<Player, PlayerState> playersStates;
		public PlayerState GetPlayerState(Player player)
		{
			return playersStates[player];
		}
		public void PlayCard(string userId, Card card)
		{
			var player = Players.Single(p => p.UserId == userId);
			var playerState = playersStates[player];
			playerState.Play(card);
			//if (!playersCards[player].Contains(card)) throw new Exception();
			//var playerCardsPlayed = cardsPlayed[player];


		}
	}
}