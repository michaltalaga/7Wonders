class GameController {
    static $inject = ['$scope', '$rootScope'];
    constructor(private $scope: IGameScope, private $rootScope: ng.IRootScopeService) {
        var gameHub = $.connection.gameHub;
        $rootScope.$on("game.selected",(e, game) => {
            $scope.game = game;
        });

        $scope.join = () => {
            if (!$scope.game) return;
            gameHub.server.joinGame($scope.game.id);
        }
        gameHub.client.newTurn = (turn) => {
            $scope.$apply(() => {
                $scope.turn = turn;
            });

        }
    }
}
interface IGameScope extends ng.IScope {
    game: any;
    join();
    turn: _7Wonders.Models.Game.GameTurn;
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
    static $inject = ['$scope'];
    constructor($scope) {
        $scope.numberOfCosts = 0;
        var card = <_7Wonders.Models.Game.Card>$scope.card;
        if (card.requiredResources) $scope.numberOfCosts++;
        if (card.previousStructure) $scope.numberOfCosts++;
        if (card.price) $scope.numberOfCosts++;

        $scope.getIconSizeClsName = (items: any[]) => {
            if (!items || items.length > 3) return null;

            return 'large';
        }
        $scope.getResourceName = (resourceId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ResourceType[resourceId]);
        }
        $scope.getScienceSymbolName = (symbolId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.ScienceSymbol[symbolId]);
        }
        $scope.getBonusTypeName = (bonus: _7Wonders.Models.Game.Bonus) => {
            if (typeof bonus.rewardForCardType !== 'undefined') return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[bonus.rewardForCardType]);
            if (bonus.rewardForMilitaryVictories) return "militaryVictory";
            if (bonus.rewardForWonderStages) return "wonder";

            //return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        }
        $scope.getCardTypeName = (cardTypeId) => {
            return uncapitaliseFirstLetter(_7Wonders.Models.Game.CardType[cardTypeId]);
        }
        $scope.toArrayWithLength = toArrayWithLength;
        $scope.select = () => {

        }
        $scope.test = (card) => {
        }
    }
}
class IconController {
    static $inject = ['$scope'];
    constructor($scope) {

        if (typeof $scope.size !== 'undefined') {
            if ($scope.size <= 3) {
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
        templateUrl: "/Templates/Icon",
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
