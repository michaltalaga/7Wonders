module _7Wonders.Models.Game {
	export enum ResourceType {
		Wood = 1,
		Ore = 2,
		Stone = 4,
		Clay = 8,
		Glass = 16,
		Papyrus = 32,
		Textiles = 64
	}
	export enum ScienceSymbol {
		Compass = 1,
		Tablet = 2,
		Gear = 4
	}
	export enum CardType {
		RawMaterial = 1,
		ManufacturedGood = 2,
		CivilianStructure = 4,
		ScientificStructure = 8,
		CommercialStructure = 16,
		MilitaryStructure = 32,
		Guild = 64
	}
}

