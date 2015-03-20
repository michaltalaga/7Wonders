using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{

    public class Player
    {
        static List<Player> players = new List<Player>();

        public static IEnumerable<Player> Players
        {
            get
            {
                return players.AsReadOnly();
            }
        }
		public string Name { get; set; }
		public string UserId { get; set; }
		public Player(string name)
		{
			Name = name;
			UserId = name;
		}


        public static void Create(string name)
        {
            players.Add(new Player(name));
        }
    }

}