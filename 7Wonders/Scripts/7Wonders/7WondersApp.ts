var app = angular.module("7Wonders", []);

interface SignalR {
    gameHub: GameHub;
    lobbyHub: LobbyHub;
}
interface GameHub {
    client: {
        newTurn(turn: _7Wonders.Models.Game.GameTurn);
    };
    server: {

        getGameDetails(id): JQueryPromise<void>;
        joinGame(id): JQueryPromise<void>;
    };
}
interface LobbyHub {
    client: {
        gameListChanged(games);
    };
    server: {
        createGame(name: string): JQueryPromise<void>;
        start(): JQueryPromise<void>;
    };
}

function uncapitaliseFirstLetter(str: string) {
    if (!str) return str;
    return str.charAt(0).toLowerCase() + str.slice(1);
}

function toArrayWithLength(num: number) {
    return new Array(num);
}