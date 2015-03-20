
class LobbyController {
    static $inject = ['$scope', '$rootScope'];
    constructor(private $scope: ILobbyScope, private $rootScope: ng.IRootScopeService) {



        var lobbyHub = $.connection.lobbyHub;


        lobbyHub.client.gameListChanged = (games) => {
            $scope.$apply(() => {
				$scope.games = games;
			});
        }
		lobbyHub.client.playerListChanged = (players) => {
			$scope.$apply(() => {
				$scope.players = players;
			});
		}
		lobbyHub.client.error = (message) => {
			alert(message);
		}

        $scope.createGame = (name) => {
            lobbyHub.server.createGame(name);
        }
        $scope.selectGame = (game) => {
            $rootScope.$broadcast("game.selected", game);
        }
		$scope.rejoin = () => {
			$.connection.gameHub.server.rejoin();
		}
        $.connection.hub.start().done(() => {
            lobbyHub.server.initialize();

            //bootstrap();
        });

        
     
        //var bootstrap = () => {
        //    setTimeout(() => {
        //        if (!$scope.games || $scope.games.length == 0) {

        //            $scope.gameName = 'test1';
        //            $scope.createGame();
        //            $scope.gameName = null;
        //            bootstrap();
        //        }
        //        else {
        //            $scope.selectGame($scope.games[0]);
        //            $.connection.gameHub.server.joinGame($scope.games[0].id);
        //        }
        //    }, 1000);
        //}

        
    }
}
interface ILobbyScope extends ng.IScope {
    games: _7Wonders.Hubs.GameListItem[];
	players: _7Wonders.Hubs.PlayerListItem[];
    createGame(name: string);
    selectGame(game: _7Wonders.Hubs.GameListItem);
	rejoin();
}
app.controller("LobbyController", LobbyController);
