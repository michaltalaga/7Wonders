
 
 

 

/// <reference path="Enums.ts" />

declare module _7Wonders.Models.Game {
	interface Card {
		name: string;
		numberOfPlayers: number;
		age: number;
		cardType: string;
		requiredResources?: string[];
		price?: number;
		previousStructure?: string;
		eitherOrResourcesProduced?: string[];
		allResourcesProduced?: string[];
		allowTrade?: _7Wonders.Models.Game.AllowTrade;
		moneyWorth?: number;
		victoryPoints?: number;
		militaryPoints?: number;
		eitherOrScienceSymbolsProduced?: string[];
		bonuses?: _7Wonders.Models.Game.Bonus;
		hasEffect: boolean;
	}
	interface AllowTrade {
		resources: string[];
		left: boolean;
		right: boolean;
	}
	interface Bonus {
		left: boolean;
		self: boolean;
		right: boolean;
		money?: number;
		victoryPoints?: number;
		rewardForCardType?: string;
		rewardForWonderStages?: boolean;
		rewardForMilitaryVictories?: boolean;
	}
	interface GameTurn {
		turnNumber: number;
		cards: _7Wonders.Models.Game.Card;
	}
}


