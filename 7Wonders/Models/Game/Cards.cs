using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
    public static class Cards
    {
		static Card[] cards;
        public static IEnumerable<Card> GetCards()
        {
			if (cards == null)
			{
				var settings = new JsonSerializerSettings();
				settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
				settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
				settings.NullValueHandling = NullValueHandling.Ignore;
				var fileName = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/cards.json");
				var serialized = File.ReadAllText(fileName);

				cards = JsonConvert.DeserializeObject<Card[]>(serialized, settings);
				foreach (var card in cards)
				{
					card.Id = Guid.NewGuid();
				}
			}
			return cards;

        }
		public static void Shuffle<Card>(this IList<Card> list)
		{
			Random rng = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Card value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static IEnumerable<Card> GetShuffledCards(int numberOfPlayers)
		{
			var cards = GetCards().Where(c => c.NumberOfPlayers <= numberOfPlayers).ToList();
			var numberOfGuilds = numberOfPlayers + 2;
			var guilds = cards.Where(c => c.CardType == CardType.Guild).ToList();
			guilds.ForEach(g => cards.Remove(g));
			guilds.Shuffle();
			cards.AddRange(guilds.Take(numberOfGuilds));
			cards.Shuffle();
			return cards;
		}
	}
}
