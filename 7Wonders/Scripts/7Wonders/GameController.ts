class GameController {
    static $inject = ['$scope', '$rootScope'];
    constructor(private $scope: IGameScope, private $rootScope: ng.IRootScopeService) {
        var gameHub = $.connection.gameHub;
        $rootScope.$on("game.selected",(e, game: _7Wonders.Hubs.GameListItem) => {
            gameHub.server.watchGame(game.id);
        });
		$rootScope.$on("card.play",(e, card: _7Wonders.Models.Game.Card) => {
			gameHub.server.playCard(card.id);
		});

        $scope.join = (gameId) => {
            gameHub.server.joinGame(gameId);
        };
		$scope.leave = () => {
			gameHub.server.leaveGame();
		}
		$scope.start = () => {
			gameHub.server.startGame();
		}
		$scope.stopWatching = () => {
			gameHub.server.stopWatchingGame();
		}
        gameHub.client.newTurn = (turn) => {
            $scope.$apply(() => {
                $scope.turn = turn;
				$scope.state = new PlayerState(turn);
            });
        }
		gameHub.client.gameDetailsChanged = (game) => {
			$scope.$apply(() => {
				$scope.game = game;
			});
		}
		gameHub.client.gameOptionsChanged = (gameOptions) => {
			$scope.$apply(() => {
				$scope.gameOptions = gameOptions;
			});
		}
    }
}
interface IGameScope extends ng.IScope {
    game: _7Wonders.Hubs.GameDetails;
	gameOptions: _7Wonders.Hubs.GameOptions;
    join(gameId: string);
	leave();
	start();
	stopWatching();
    turn: _7Wonders.Models.Game.GameTurn;
	state: PlayerState;
}
class PlayerState {
	constructor(private turn: _7Wonders.Models.Game.GameTurn) {
	}
	public canPlayCard(card: _7Wonders.Models.Game.Card): boolean {

		return true;
	}
}
app.controller("GameController", GameController);
app.directive('hand',() => {
    return {
        replace: true,
        restrict: 'E',
        scope: { cards: '=' },
        templateUrl: '/Templates/Hand'
    }
});
class CardController {
    static $inject = ['$scope', '$rootScope'];
    constructor($scope, $rootScope: ng.IRootScopeService) {
        $scope.numberOfCosts = 0;
        var card = <_7Wonders.Models.Game.Card>$scope.card;
        if (card.requiredResources) $scope.numberOfCosts++;
        if (card.previousStructure) $scope.numberOfCosts++;
        if (card.price) $scope.numberOfCosts++;

        $scope.getIconSizeClsName = (items: any[]) => {
            if (!items || items.length > 3) return null;

            return 'large';
        };
        $scope.getResourceName = (resourceId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ResourceType[resourceId]);
        };
        $scope.getScienceSymbolName = (symbolId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ScienceSymbol[symbolId]);
        };
        //$scope.getBonusTypeName = (bonus: _7Wonders.Models.Game.Bonus) => {
        //    if (typeof bonus.rewardForCardType !== 'undefined') return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[bonus.rewardForCardType]);
        //    if (bonus.rewardForMilitaryVictories) return "militaryVictory";
        //    if (bonus.rewardForWonderStages) return "wonder";

        //    //return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        //};
        $scope.getCardTypeName = (cardTypeId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        };
        $scope.toArrayWithLength = toArrayWithLength;
        $scope.select = () => {

        };
        $scope.test = (card) => {
        }
		$scope.play = (card) => {
			$rootScope.$broadcast("card.play", card);
		}
		$scope.effectsSize = (effect: _7Wonders.Models.Game.Effect):number => {
			var size = 0;
			if (effect.left) size++;
			if (effect.right) size++;
			if (effect.self) size++;
			if (effect.allowTradeResources) size += effect.allowTradeResources.length;
			if (effect.allResourcesProduced) size += effect.allResourcesProduced.length;
			if (effect.eitherOrResourcesProduced) size += effect.eitherOrResourcesProduced.length;
			if (effect.eitherOrScienceSymbolsProduced) size += effect.eitherOrScienceSymbolsProduced.length;
			if (effect.rewardForSomething || effect.money || effect.victoryPoints) size++;
			if (effect.militaryPoints) size += effect.militaryPoints;
			return size;

		}
    }
}
class IconController {
    static $inject = ['$scope'];
    constructor($scope) {

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
}
app.directive('icon',() => {
    return {
        replace: true,
        restrict: 'E',
        scope: { kind: '@', value: '=', text: '=', size: '=', money: '=', victory: '=' },
        controller: IconController,
        templateUrl: "/Templates/Icon"
    }
});
app.directive('card',() => {
    return {
        replace: true,
        restrict: 'E',
        controller: CardController,
        scope: { card: '=' },
        templateUrl: "/Templates/Card"
    }
});