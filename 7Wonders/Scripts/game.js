var app = angular.module("7Wonders", []);
function uncapitaliseFirstLetter(str) {
    if (!str)
        return str;
    return str.charAt(0).toLowerCase() + str.slice(1);
}
function toArrayWithLength(num) {
    return new Array(num);
}
var GameController = (function () {
    function GameController($scope, $rootScope) {
        this.$scope = $scope;
        this.$rootScope = $rootScope;
        var gameHub = $.connection.gameHub;
        $rootScope.$on("game.selected", function (e, game) {
            gameHub.server.watchGame(game.id);
        });
        $rootScope.$on("card.play", function (e, card) {
            gameHub.server.playCard(card.id);
        });
        $scope.join = function (gameId) {
            gameHub.server.joinGame(gameId);
        };
        $scope.leave = function () {
            gameHub.server.leaveGame();
        };
        $scope.start = function () {
            gameHub.server.startGame();
        };
        $scope.stopWatching = function () {
            gameHub.server.stopWatchingGame();
        };
        gameHub.client.newTurn = function (turn) {
            $scope.$apply(function () {
                $scope.turn = turn;
                $scope.state = new PlayerState(turn);
            });
        };
        gameHub.client.gameDetailsChanged = function (game) {
            $scope.$apply(function () {
                $scope.game = game;
            });
        };
        gameHub.client.gameOptionsChanged = function (gameOptions) {
            $scope.$apply(function () {
                $scope.gameOptions = gameOptions;
            });
        };
    }
    GameController.$inject = ['$scope', '$rootScope'];
    return GameController;
})();
var PlayerState = (function () {
    function PlayerState(turn) {
        this.turn = turn;
    }
    PlayerState.prototype.canPlayCard = function (card) {
        return true;
    };
    return PlayerState;
})();
app.controller("GameController", GameController);
app.directive('hand', function () {
    return {
        replace: true,
        restrict: 'E',
        scope: { cards: '=' },
        templateUrl: '/Templates/Hand'
    };
});
var CardController = (function () {
    function CardController($scope, $rootScope) {
        $scope.numberOfCosts = 0;
        var card = $scope.card;
        if (card.requiredResources)
            $scope.numberOfCosts++;
        if (card.previousStructure)
            $scope.numberOfCosts++;
        if (card.price)
            $scope.numberOfCosts++;
        $scope.getIconSizeClsName = function (items) {
            if (!items || items.length > 3)
                return null;
            return 'large';
        };
        $scope.getResourceName = function (resourceId) {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ResourceType[resourceId]);
        };
        $scope.getScienceSymbolName = function (symbolId) {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ScienceSymbol[symbolId]);
        };
        //$scope.getBonusTypeName = (bonus: _7Wonders.Models.Game.Bonus) => {
        //    if (typeof bonus.rewardForCardType !== 'undefined') return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[bonus.rewardForCardType]);
        //    if (bonus.rewardForMilitaryVictories) return "militaryVictory";
        //    if (bonus.rewardForWonderStages) return "wonder";
        //    //return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        //};
        $scope.getCardTypeName = function (cardTypeId) {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        };
        $scope.toArrayWithLength = toArrayWithLength;
        $scope.select = function () {
        };
        $scope.test = function (card) {
        };
        $scope.play = function (card) {
            $rootScope.$broadcast("card.play", card);
        };
        $scope.effectsSize = function (effect) {
            var size = 0;
            if (effect.left)
                size++;
            if (effect.right)
                size++;
            if (effect.self)
                size++;
            if (effect.allowTradeResources)
                size += effect.allowTradeResources.length;
            if (effect.allResourcesProduced)
                size += effect.allResourcesProduced.length;
            if (effect.eitherOrResourcesProduced)
                size += effect.eitherOrResourcesProduced.length;
            if (effect.eitherOrScienceSymbolsProduced)
                size += effect.eitherOrScienceSymbolsProduced.length;
            if (effect.rewardForSomething || effect.money || effect.victoryPoints)
                size++;
            if (effect.militaryPoints)
                size += effect.militaryPoints;
            return size;
        };
    }
    CardController.$inject = ['$scope', '$rootScope'];
    return CardController;
})();
var IconController = (function () {
    function IconController($scope) {
        if (typeof $scope.size !== 'undefined') {
            if ($scope.size <= 4) {
                $scope.sizeName = 'large';
            }
            else if ($scope.size <= 5) {
                $scope.sizeName = 'medium';
            }
        }
        //if ($scope.type === 'resource') {
        //    $scope.className = 'resource ' + uncapitaliseFirstLetter(_7Wonders.Models.Game.ResourceType[$scope.value]);
        //}
        //else if ($scope.type === 'science') {
        //    $scope.className = 'science ' + uncapitaliseFirstLetter(_7Wonders.Models.Game.ScienceSymbol[$scope.value]);
        //}
        //else if ($scope.type === 'card') {
        //    $scope.className = 'card ' + uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[$scope.value]);
        //}
        //else {
        //    $scope.className = $scope.type;
        //}
    }
    IconController.$inject = ['$scope'];
    return IconController;
})();
app.directive('icon', function () {
    return {
        replace: true,
        restrict: 'E',
        scope: { kind: '@', value: '=', text: '=', size: '=', money: '=', victory: '=' },
        controller: IconController,
        templateUrl: "/Templates/Icon"
    };
});
app.directive('card', function () {
    return {
        replace: true,
        restrict: 'E',
        controller: CardController,
        scope: { card: '=' },
        templateUrl: "/Templates/Card"
    };
});
var LobbyController = (function () {
    function LobbyController($scope, $rootScope) {
        this.$scope = $scope;
        this.$rootScope = $rootScope;
        var lobbyHub = $.connection.lobbyHub;
        lobbyHub.client.gameListChanged = function (games) {
            $scope.$apply(function () {
                $scope.games = games;
            });
        };
        lobbyHub.client.playerListChanged = function (players) {
            $scope.$apply(function () {
                $scope.players = players;
            });
        };
        lobbyHub.client.error = function (message) {
            alert(message);
        };
        $scope.createGame = function (name) {
            lobbyHub.server.createGame(name);
        };
        $scope.selectGame = function (game) {
            $rootScope.$broadcast("game.selected", game);
        };
        $scope.rejoin = function () {
            $.connection.gameHub.server.rejoin();
        };
        $.connection.hub.start().done(function () {
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
    LobbyController.$inject = ['$scope', '$rootScope'];
    return LobbyController;
})();
app.controller("LobbyController", LobbyController);
var _7Wonders;
(function (_7Wonders) {
    var Models;
    (function (Models) {
        var Game;
        (function (Game) {
            (function (ResourceType) {
                ResourceType[ResourceType["Wood"] = 1] = "Wood";
                ResourceType[ResourceType["Ore"] = 2] = "Ore";
                ResourceType[ResourceType["Stone"] = 4] = "Stone";
                ResourceType[ResourceType["Clay"] = 8] = "Clay";
                ResourceType[ResourceType["Glass"] = 16] = "Glass";
                ResourceType[ResourceType["Papyrus"] = 32] = "Papyrus";
                ResourceType[ResourceType["Textiles"] = 64] = "Textiles";
            })(Game.ResourceType || (Game.ResourceType = {}));
            var ResourceType = Game.ResourceType;
            (function (ScienceSymbol) {
                ScienceSymbol[ScienceSymbol["Compass"] = 1] = "Compass";
                ScienceSymbol[ScienceSymbol["Tablet"] = 2] = "Tablet";
                ScienceSymbol[ScienceSymbol["Gear"] = 4] = "Gear";
            })(Game.ScienceSymbol || (Game.ScienceSymbol = {}));
            var ScienceSymbol = Game.ScienceSymbol;
            (function (CardType) {
                CardType[CardType["RawMaterial"] = 1] = "RawMaterial";
                CardType[CardType["ManufacturedGood"] = 2] = "ManufacturedGood";
                CardType[CardType["CivilianStructure"] = 4] = "CivilianStructure";
                CardType[CardType["ScientificStructure"] = 8] = "ScientificStructure";
                CardType[CardType["CommercialStructure"] = 16] = "CommercialStructure";
                CardType[CardType["MilitaryStructure"] = 32] = "MilitaryStructure";
                CardType[CardType["Guild"] = 64] = "Guild";
            })(Game.CardType || (Game.CardType = {}));
            var CardType = Game.CardType;
        })(Game = Models.Game || (Models.Game = {}));
    })(Models = _7Wonders.Models || (_7Wonders.Models = {}));
})(_7Wonders || (_7Wonders = {}));
//# sourceMappingURL=game.js.map