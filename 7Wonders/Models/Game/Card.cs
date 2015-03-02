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
        public string Name { get; set; }
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
        public IEnumerable<ResourceType> EitherOrResourcesProduced { get; set; }
        [TsProperty(IsOptional = true)]
        public IEnumerable<ResourceType> AllResourcesProduced { get; set; }

        [TsProperty(IsOptional = true)]
        public AllowTrade AllowTrade { get; set; }
        [TsProperty(IsOptional = true)]
        public int? MoneyWorth { get; set; }
        [TsProperty(IsOptional = true)]
        public int? VictoryPoints { get; set; }
        [TsProperty(IsOptional = true)]
        public int? MilitaryPoints { get; set; }
        [TsProperty(IsOptional = true)]
        public IEnumerable<ScienceSymbol> EitherOrScienceSymbolsProduced { get; set; }
        [TsProperty(IsOptional = true)]
        public IEnumerable<Bonus> Bonuses { get; set; }

     

        public bool HasEffect
        {
            get
            {
                return EitherOrResourcesProduced != null 
                    || AllResourcesProduced != null 
                    || AllowTrade != null 
                    || MoneyWorth != null
                    || VictoryPoints != null
                    || MilitaryPoints != null
                    || EitherOrScienceSymbolsProduced != null
                    || Bonuses != null
                    ;
            }
        }
        // victory/guilds <[{2}]>, <[{1}]>, < ({1}|-1|) >, [1][1][1], < V >
        // yellow bonus     [(1)]      [ ]
        //                < V >       (1){1}

    }
    public class Bonus
    {
        public bool Left { get; set; }
        public bool Self { get; set; }
        public bool Right { get; set; }
        [TsProperty(IsOptional = true)]
        public int? Money { get; set; }
        [TsProperty(IsOptional = true)]
        public int? VictoryPoints { get; set; }
        [TsProperty(IsOptional = true)]
        public CardType? RewardForCardType { get; set; }
        [TsProperty(IsOptional = true)]
        public bool? RewardForWonderStages { get; set; }
        [TsProperty(IsOptional = true)]
        public bool? RewardForMilitaryVictories { get; set; }
    }
    public class AllowTrade
    {
        public IEnumerable<ResourceType> Resources { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
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