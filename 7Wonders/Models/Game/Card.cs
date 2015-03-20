using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace _7Wonders.Models.Game
{
    [TsClass]
    public class Card
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int NumberOfPlayers { get; set; }
        public int Age { get; set; }
        public CardType CardType { get; set; }

        [TsProperty(IsOptional = true)]
        public IEnumerable<ResourceType> RequiredResources { get; set; }
        [TsProperty(IsOptional = true)]
        public int? Price { get; set; }
        [TsProperty(IsOptional = true)]
        public string PreviousStructure { get; set; }

		[TsProperty(IsOptional = true)]
		public IEnumerable<Effect> Effects { get; set; }

     

   
		public static readonly IEqualityComparer<Card> NameEqualityComparer = new CardNameEqualityComparerImpl();

		private class CardNameEqualityComparerImpl : IEqualityComparer<Card>
		{
			public bool Equals(Card x, Card y)
			{
				return x.Name == y.Name;
			}

			public int GetHashCode(Card obj)
			{
				return obj.GetHashCode();
			}
		}
		
	}
    public class Effect
    {
        public bool Left { get; set; }
        public bool Self { get; set; }
        public bool Right { get; set; }
        [TsProperty(IsOptional = true)]
        public int? Money { get; set; }
        [TsProperty(IsOptional = true)]
        public int? VictoryPoints { get; set; }
		[TsProperty(IsOptional = true)]
		public int? MilitaryPoints { get; set; }
		[TsProperty(IsOptional = true)]
        public CardType? RewardForCardType { get; set; }
        [TsProperty(IsOptional = true)]
        public bool? RewardForWonderStages { get; set; }
        [TsProperty(IsOptional = true)]
        public bool? RewardForMilitaryVictories { get; set; }


		[TsProperty(IsOptional = true)]
		public IEnumerable<ResourceType> EitherOrResourcesProduced { get; set; }
		[TsProperty(IsOptional = true)]
		public IEnumerable<ResourceType> AllResourcesProduced { get; set; }
		[TsProperty(IsOptional = true)]
		public IEnumerable<ScienceSymbol> EitherOrScienceSymbolsProduced { get; set; }
		[TsProperty(IsOptional = true)]
		public IEnumerable<ResourceType> AllowTradeResources { get; set; }


		public bool RewardForSomething
		{
			get
			{
				return RewardForCardType != null || RewardForWonderStages != null || RewardForMilitaryVictories != null;
			}
		}
	}
   
    [JsonConverter(typeof(StringEnumConverterCamelCased))]
    public enum ScienceSymbol
    {
        Compass = 1,
        Tablet = 2,
        Gear = 4,
    }
    [JsonConverter(typeof(StringEnumConverterCamelCased))]
    public enum ResourceType
    {
        Wood = 1,
        Ore = 2,
        Stone = 4,
        Clay = 8,
        Glass = 16,
        Papyrus = 32,
        Textiles = 64,
    }
    [JsonConverter(typeof(StringEnumConverterCamelCased))]
    public enum CardType
    {
        RawMaterial = 1,
        ManufacturedGood = 2,
        CivilianStructure = 4,
        ScientificStructure = 8,
        CommercialStructure = 16,
        MilitaryStructure = 32,
        Guild = 64,
    }

    public class StringEnumConverterCamelCased : StringEnumConverter
    {
        public StringEnumConverterCamelCased()
        {
            this.CamelCaseText = true;
        }
    }
    
}