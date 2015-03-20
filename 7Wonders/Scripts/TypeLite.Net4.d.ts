
 
 

 

/// <reference path="Enums.ts" />

declare module _7Wonders.Models.Game {
	interface Card {
		id: System.Guid;
		name: string;
		code: string;
		numberOfPlayers: number;
		age: number;
		cardType: string;
		requiredResources?: string[];
		price?: number;
		previousStructure?: string;
		effects?: _7Wonders.Models.Game.Effect;
	}
	interface Effect {
		left: boolean;
		self: boolean;
		right: boolean;
		money?: number;
		victoryPoints?: number;
		militaryPoints?: number;
		rewardForCardType?: string;
		rewardForWonderStages?: boolean;
		rewardForMilitaryVictories?: boolean;
		eitherOrResourcesProduced?: string[];
		allResourcesProduced?: string[];
		eitherOrScienceSymbolsProduced?: string[];
		allowTradeResources?: string[];
		rewardForSomething: boolean;
	}
	interface GameTurn {
		age: number;
		turnNumber: number;
		cardsInHand: _7Wonders.Models.Game.Card;
		cardsPlayed: _7Wonders.Models.Game.Card;
		gold: number;
	}
}
declare module System {
	interface Guid {
	}
}
declare module _7Wonders.Hubs {
	interface GameOptions {
		canJoin: boolean;
		canLeave: boolean;
		canStart: boolean;
		canStopWatching: boolean;
	}
	interface GameDetails {
		id: System.Guid;
		name: string;
		isStarted: boolean;
		players: _7Wonders.Hubs.PlayerListItem;
		options: _7Wonders.Hubs.GameOptions;
	}
	interface PlayerListItem {
		name: string;
	}
	interface GameListItem {
		id: System.Guid;
		name: string;
		timeOfCreation: Date;
		players: _7Wonders.Hubs.PlayerListItem;
	}
}


