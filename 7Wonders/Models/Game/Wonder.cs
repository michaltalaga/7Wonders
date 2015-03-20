using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
	public class Wonder
	{
		public string Name { get; set; }
		public WonderSide SideA { get; set; }
		public WonderSide SideB { get; set; }
	}
	public class WonderSide
	{
		public ResourceType ResourceProduced { get; set; }
	}
	public class WonderStage
	{
		public int Number { get; set; }
		
	}
}