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
        public static IEnumerable<Card> GetCards()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter { CamelCaseText = true });
            settings.NullValueHandling = NullValueHandling.Ignore;
            var fileName = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/cards.json");
            var serialized = File.ReadAllText(fileName);

            return JsonConvert.DeserializeObject<Card[]>(serialized, settings);

            //var cardsCsvFilePath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/cards.csv");
            //var lines = File.ReadAllLines(cardsCsvFilePath);
            //foreach (var line in lines.Skip(1))
            //{
            //    var fields = line.Split(';');
            //    //Name;Age;NumberOfPlayers;CardType;RequiredResources;Price;PreviousStructure;EitherOrResourcesProduced;AllResourcesProduced;AllowTrade;MoneyWorth;VictoryPoints;MilitaryPoints;EitherOrScienceSymbolsProduced;Bonuses
            //    //apothecary;1;5;ScientificStructure;Textiles;null;null;null;null;null;null;null;null;Compass;null
            //    //workers guild;3;3;Guild;Ore|Ore|Clay|Stone|Wood;null;null;null;null;null;null;null;null;null;null|1:RawMaterial|null|null:true|false|true
            //    //shipowners guild;3;3;Guild;Wood|Wood|Wood|Papyrus|Glass;null;null;null;null;null;null;null;null;null;null|1:RawMaterial|null|null:false|true|false/null|1:ManufacturedGoods|null|null:false|true|false/null|1:Guild|null|null:false|true|false

            //    //var name = fields[0];
            //    //var age = int.Parse(fields[1]);
            //    //var numberOfPlayers = int.Parse(fields[2]);
            //    //var type = (CardType)Enum.Parse(typeof(CardType), fields[3]);
            //    //var requiredResources = GetEnums<ResourceType>(fields[4]);
            //    //var price = GetInt(fields[5]);
            //    //var previousStructure = GetString(fields[6]);
            //    //var eitherOrResourcesProduced = GetEnums<ResourceType>(fields[7]);
            //    //var allResourcesProduced = GetEnums<ResourceType>(fields[8]);
            //    //var allowTrade = GetAllowTrade(fields[9]);
            //    //var moneyWorth = GetInt(fields[10]);
            //    //var victoryPoints = GetInt(fields[11]);
            //    //var militaryPoints = GetInt(fields[12]);
            //    //var eitherOrScienceSymbolsProduced = GetEnums<ScienceSymbol>(fields[13]);
            //    //var bonuses = GetBonuses(fields[14]);


            //    yield return new Card()
            //    {
            //        Name = fields[0],
            //        Age = int.Parse(fields[1]),
            //        NumberOfPlayers = int.Parse(fields[2]),
            //        Type = (CardType)Enum.Parse(typeof(CardType), fields[3]),
            //        RequiredResources = GetEnums<ResourceType>(fields[4]),
            //        Price = GetInt(fields[5]),
            //        PreviousStructure = GetString(fields[6]),
            //        EitherOrResourcesProduced = GetEnums<ResourceType>(fields[7]),
            //        AllResourcesProduced = GetEnums<ResourceType>(fields[8]),
            //        AllowTrade = GetAllowTrade(fields[9]),
            //        MoneyWorth = GetInt(fields[10]),
            //        VictoryPoints = GetInt(fields[11]),
            //        MilitaryPoints = GetInt(fields[12]),
            //        EitherOrScienceSymbolsProduced = GetEnums<ScienceSymbol>(fields[13]),
            //        Bonuses = GetBonuses(fields[14]),
            //    };

            //}



            //yield break;
        }

        private static IEnumerable<Bonus> GetBonuses(string field)
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            //null|1:RawMaterial|null|null:false|true|false/null|1:ManufacturedGoods|null|null:false|true|false/null|1:Guild|null|null:false|true|false
            var bonusesSplit = field.Split('/');
            return bonusesSplit.Select(GetBonus);
            //foreach (var bonusSplit in bonusesSplit)
            //{
            //    yield return GetBonus(bonusSplit);
            //}
        }

        private static Bonus GetBonus(string field)
        {
            //null|1:RawMaterial|null|null:false|true|false
            var split = field.Split(':');

            var effectSplit = split[0].Split('|');
            var conditionSpli = split[1].Split('|');
            var directionSplit = split[2].Split('|');

            return new Bonus()
            {
                Money = GetInt(effectSplit[0]),
                VictoryPoints = GetInt(effectSplit[1]),
                RewardForCardType = GetEnum<CardType>(conditionSpli[0]),
                RewardForWonderStages = GetBool(conditionSpli[1]),
                RewardForMilitaryVictories = GetBool(conditionSpli[2]),
                Left = bool.Parse(directionSplit[0]),
                Self = bool.Parse(directionSplit[1]),
                Right = bool.Parse(directionSplit[2]),

            };
        }

        private static AllowTrade GetAllowTrade(string field)
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            //Clay|Stone|Wood|Ore:false|true
            var split = field.Split(':');
            var leftRightSplit = split[1].Split('|');
            return new AllowTrade()
            {
                Resources = GetEnums<ResourceType>(split[0]),
                Left = bool.Parse(leftRightSplit[0]),
                Right = bool.Parse(leftRightSplit[1]),

            };
        }

        private static string GetString(string field)
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            return field;
        }

        private static int? GetInt(string field)
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            return int.Parse(field);
        }
        private static bool? GetBool(string field)
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            return bool.Parse(field);
        }
        private static IEnumerable<T> GetEnums<T>(string field) where T : struct
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            var items = field.Split('|');
            return items.Select(i => GetEnum<T>(i).Value);
        }
        private static Nullable<T> GetEnum<T>(string field) where T : struct
        {
            if (string.IsNullOrEmpty(field) || field == "null") return null;
            return (T)Enum.Parse(typeof(T), field);
        }
        private static Card CreateCard(string name, CardType type)
        {
            return new Card()
                {
                    Name = name,
                    CardType = type,

                };
        }
    }
}

// ScienceSymbol
//    Compass = 1,
//    Tablet = 2,
//    Gear = 4,
// ResourceType
//    Wood = 1,
//    Ore = 2,
//    Stone = 4,
//    Clay = 8,
//    Glass = 16,
//    Papyrus = 32,
//    Textiles = 64,
// CardType
//    RawMaterial = 1,
//    ManufacturedGood = 2,
//    CivilianStructure = 4,
//    ScientificStructure = 8,
//    CommercialStructure = 16,
//    MilitaryStructure = 32,
//    Guild = 64,