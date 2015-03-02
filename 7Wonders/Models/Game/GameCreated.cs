using _7Wonders.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
    public class GameCreated : Event
    {
        public Game Game { get; set; }
    }
}