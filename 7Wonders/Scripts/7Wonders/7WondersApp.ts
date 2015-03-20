var app = angular.module("7Wonders", []);

interface SignalR {
    gameHub: GameHub;
    lobbyHub: LobbyHub;
}
interface GameHub {
    client: {
        newTurn(turn: _7Wonders.Models.Game.GameTurn);
		gameDetailsChanged(game: _7Wonders.Hubs.GameDetails);
		gameOptionsChanged(gameOptions: _7Wonders.Hubs.GameOptions);
    };
    server: {

        watchGame(id): JQueryPromise<void>;
		stopWatchingGame(): JQueryPromise<void>;
        joinGame(id): JQueryPromise<void>;
		leaveGame(): JQueryPromise<void>;
		startGame(): JQueryPromise<void>;
		playCard(id): JQueryPromise<void>;
		rejoin(): JQueryPromise<void>;
    };
}
interface LobbyHub {
    client: {
        gameListChanged(games: _7Wonders.Hubs.GameListItem[]);
        playerListChanged(players: _7Wonders.Hubs.PlayerListItem[]);
		error(message: string);
    };
    server: {
        createGame(name: string): JQueryPromise<void>;
        initialize(): JQueryPromise<void>;
    };
}

function uncapitaliseFirstLetter(str: string) {
    if (!str) return str;
    return str.charAt(0).toLowerCase() + str.slice(1);
}

function toArrayWithLength(num: number) {
    return new Array(num);
}