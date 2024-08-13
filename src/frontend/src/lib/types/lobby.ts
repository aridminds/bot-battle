export type Lobby = {
	name: string;
	width: number;
	height: number;
	players: string[];
	mapTiles: number[];
};

export function getTile(mapTiles: number[], width: number, x: number, y: number): number {
	const index = y * width + x;	
    return mapTiles[index];
}


