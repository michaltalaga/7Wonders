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
        public int TurnNumber { get; set; }
        public IEnumerable<Card> Cards { get; set; }

    }
}