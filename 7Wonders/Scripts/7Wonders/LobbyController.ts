
class LobbyController {
    static $inject = ['$scope', '$rootScope'];
    constructor(private $scope, private $rootScope: ng.IRootScopeService) {

   
        var lobbyHub = $.connection.lobbyHub;


        lobbyHub.client.gameListChanged = (games) => {
            $scope.games = games;
            $scope.$apply();
        }


        $scope.createGame = () => {
            lobbyHub.server.createGame($scope.gameName);
        }
        $scope.selectGame = (game) => {
            $rootScope.$broadcast("game.selected", game);
        }


        $.connection.hub.start().done(() => {
            lobbyHub.server.start();

            bootstrap();
        });

        
     
        var bootstrap = () => {
            setTimeout(() => {
                if (!$scope.games || $scope.games.length == 0) {

                    $scope.gameName = 'test1';
                    $scope.createGame();
                    $scope.gameName = null;
                    bootstrap();
                }
                else {
                    $scope.selectGame($scope.games[0]);
                    $.connection.gameHub.server.joinGame($scope.games[0].id);
                }
            }, 1000);
        }

        
    }
}

app.controller("LobbyController", LobbyController);
