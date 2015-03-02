using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using _7Wonders.Infrastructure;
using _7Wonders.Models.Game;
using Newtonsoft.Json;
using System.IO;
using TypeLite;
using TypeLite.Net4;
using Newtonsoft.Json.Schema;
using System.Web.Hosting;

namespace _7Wonders.Hubs
{
    public class GameHub : Hub<IGameClient>
    {
        public void JoinGame(Guid id)
        {
            var turn = new GameTurn()
            {
                TurnNumber = 1,
                Cards = Cards.GetCards(),
            };

  
//var jsonSchemaGenerator = new JsonSchemaGenerator();
//var myType = typeof(Card[]);
//var schema = jsonSchemaGenerator.Generate(myType);
//schema.Title = myType.Name; // this doesn't seem to get done within the generator
//var writer = new StringWriter();
//var jsonTextWriter = new JsonTextWriter(writer);
//schema.WriteTo(jsonTextWriter);
//dynamic parsedJson = JsonConvert.DeserializeObject(writer.ToString());
//var prettyString = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
//var fileWriter = new StreamWriter(HostingEnvironment.MapPath("~/App_Data/CardsSchema.json"));
//fileWriter.WriteLine(schema.Title);
//fileWriter.WriteLine(new string('-', schema.Title.Length));
//fileWriter.WriteLine(prettyString);
//fileWriter.Close();

            Clients.Caller.NewTurn(turn);
        }
        public GameDetails GetGameDetails(Guid id)
        {
            return new GameDetails() { Name = id.ToString() };
        }

        private Card[] GenerateHand()
        {
            var cards = new Card[] 
            { 
                    new Card() 
                    { 
                        Name = "merchants guild", 
                        CardType = CardType.Guild,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Left = true, Right = true, VictoryPoints = 1, RewardForCardType = CardType.CommercialStructure }
                        }
                    },
                    new Card() 
                    { 
                        Name = "chamber of commerce", 
                        CardType = CardType.CommercialStructure,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Self = true, VictoryPoints = 2, Money = 2, RewardForCardType = CardType.ManufacturedGood }
                        }
                    },
                    new Card() { Name = "barracks", RequiredResources = new ResourceType[] { ResourceType.Wood }, CardType = CardType.MilitaryStructure }, 
                    new Card() { Name = "wall", RequiredResources = new ResourceType[] { ResourceType.Wood, ResourceType.Stone }, Price = 1, CardType = CardType.Guild, AllResourcesProduced = new [] { ResourceType.Wood, ResourceType.Wood } }, 
                    new Card() { Name = "school", RequiredResources = new ResourceType[] { ResourceType.Textiles, ResourceType.Clay, ResourceType.Stone, ResourceType.Wood }, CardType = CardType.ScientificStructure },
                    new Card() { Name = "laboratory", CardType = CardType.CommercialStructure }, 
                    new Card() 
                    {
                        Name = "mine", 
                        Price = 1, 
                        CardType = CardType.RawMaterial, 
                        EitherOrResourcesProduced = new [] { ResourceType.Stone, ResourceType.Ore }
                    }, 
                    new Card() 
                    { 
                        Name = "ore mine", 
                        Price = 2, 
                        CardType = CardType.RawMaterial,
                        AllResourcesProduced = new [] { ResourceType.Ore, ResourceType.Ore }
                    }, 
                    new Card() 
                    { 
                        Name = "bazar",
                        CardType = CardType.CommercialStructure,
                        RequiredResources = new [] { ResourceType.Wood, ResourceType.Wood },
                        PreviousStructure = "market",
                        EitherOrResourcesProduced = new [] { ResourceType.Wood, ResourceType.Stone, ResourceType.Ore, ResourceType.Clay }
                    }, 
                    new Card() 
                    { 
                        Name = "east trading post", 
                        CardType = CardType.CommercialStructure,
                        AllowTrade = new AllowTrade()
                        {
                            Right = true,
                            Resources = new [] { ResourceType.Wood, ResourceType.Stone, ResourceType.Ore, ResourceType.Clay }
                        }
                    }, 
                    new Card() 
                    { 
                        Name = "forum", 
                        CardType = CardType.CommercialStructure,
                        AllowTrade = new AllowTrade()
                        {
                            Right = true,
                            Left = true,
                            Resources = new [] { ResourceType.Textiles, ResourceType.Glass, ResourceType.Papyrus }
                        }
                    }, 
                    new Card() { Name = "sawmill", RequiredResources = new [] { ResourceType.Wood, ResourceType.Ore }, PreviousStructure = "laboratory", Price = 3, CardType = CardType.RawMaterial },
                    new Card() { Name = "glassworks", CardType = CardType.ManufacturedGood, EitherOrResourcesProduced = new [] { ResourceType.Glass} },
                    new Card() { Name = "tavern", MoneyWorth = 5, CardType = CardType.CommercialStructure },
                    new Card() { Name = "baths", VictoryPoints = 3, CardType = CardType.CivilianStructure },
                    new Card() { Name = "barracks", MilitaryPoints = 3, CardType = CardType.MilitaryStructure },
                    new Card() { Name = "school", EitherOrScienceSymbolsProduced = new [] { ScienceSymbol.Tablet }, CardType = CardType.ScientificStructure, RequiredResources = new [] { ResourceType.Wood, ResourceType.Papyrus } },
                    new Card() { Name = "scientists guild", EitherOrScienceSymbolsProduced = new [] { ScienceSymbol.Compass, ScienceSymbol.Tablet, ScienceSymbol.Gear }, CardType = CardType.Guild, RequiredResources = new [] { ResourceType.Wood, ResourceType.Papyrus } },
                    new Card() 
                    { 
                        Name = "merchants guild", 
                        CardType = CardType.Guild,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Left = true, Right = true, VictoryPoints = 1, RewardForCardType = CardType.CommercialStructure }
                        }
                    },
                    new Card() 
                    { 
                        Name = "builders guild", 
                        CardType = CardType.Guild,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Left = true, Right = true, Self = true, VictoryPoints = 1, RewardForWonderStages = true }
                        }
                    },
                    new Card() 
                    { 
                        Name = "strategists guild", 
                        CardType = CardType.Guild,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Left = true, Right = true, VictoryPoints = 1, RewardForMilitaryVictories = true }
                        }
                    },
                    new Card() 
                    { 
                        Name = "arena", 
                        CardType = CardType.CommercialStructure,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Self = true, VictoryPoints = 1, Money = 3, RewardForWonderStages = true }
                        }
                    },
                    new Card() 
                    { 
                        Name = "chamber of commerce", 
                        CardType = CardType.CommercialStructure,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Self = true, VictoryPoints = 2, Money = 2, RewardForCardType = CardType.ManufacturedGood }
                        }
                    },
                    new Card() 
                    { 
                        Name = "shipowners guild", 
                        CardType = CardType.Guild,
                        RequiredResources = new [] { ResourceType.Glass, ResourceType.Textiles, ResourceType.Papyrus },
                        Bonuses = new [] 
                        {
                            new Bonus() { Self = true, VictoryPoints = 1, RewardForCardType = CardType.RawMaterial },
                            new Bonus() { Self = true, VictoryPoints = 1, RewardForCardType = CardType.ManufacturedGood },
                            new Bonus() { Self = true, VictoryPoints = 1, RewardForCardType = CardType.Guild },
                        }
                    },

            };


            return cards;
        }

    }

    public class GameDetails
    {
        public string Name { get; set; }
    }

    public class LobbyHub : Hub<ILobbyClient>
    {
        public void Start()
        {
            Clients.Caller.GameListChanged(GameCommandHandler.games.Select(g => new GameListItem(g)));
        }
        public void CreateGame(string name)
        {
            var createGameCommand = new CreateGameCommand()
            {
                Name = name
            };
            var bus = DependencyInjection.Container.GetInstance<CommandBus>();
            bus.Send(createGameCommand);
        }

    }
    public interface ILobbyClient
    {
        void GameListChanged(IEnumerable<GameListItem> games);

    }
    public interface IGameClient
    {
        void NewTurn(GameTurn turn);
    }




}