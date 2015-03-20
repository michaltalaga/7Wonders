using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace _7Wonders.Models.Game
{
    [TsClass]
    public class GameTurn
    {
		public int Age { get; set; }
		public int TurnNumber { get; set; }
        public IEnumerable<Card> CardsInHand { get; set; }
		public IEnumerable<Card> CardsPlayed { get; set; }
		public int Gold { get; set; }

	}
}