using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
    public class Game
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeOfCreation { get; set; }
        private List<Player> players = new List<Player>();
        public IEnumerable<Player> Players
        {
            get
            {
                return players;
            }
        }
    }
}