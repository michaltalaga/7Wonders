using _7Wonders.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _7Wonders.Models.Game
{
    public class GameCommandHandler : ICommandHandler<CreateGameCommand>
    {
        EventBus eventBus;
        public GameCommandHandler(EventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public static List<Game> games = new List<Game>();
        public void Handle(CreateGameCommand command)
        {
            games.Add(new Game()
                {
                    Id = Guid.NewGuid(),
                    Name = command.Name,
                    TimeOfCreation = DateTime.Now,
                });

            var hub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<_7Wonders.Hubs.LobbyHub, _7Wonders.Hubs.ILobbyClient>();
            hub.Clients.All.GameListChanged(games.Select(g => new GameListItem(g)));
            

        }
    }
    
    public class GameListItem
    {
        public GameListItem(Game game)
        {
            Id = game.Id;
            Name = game.Name;
            TimeOfCreation = game.TimeOfCreation;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeOfCreation { get; set; }
    }

}