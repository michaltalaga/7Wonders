﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game.Commands
{
	public class JoinGame
	{
		public string PlayerName { get; set; }
		public Guid GameId { get; set; }
	}
}