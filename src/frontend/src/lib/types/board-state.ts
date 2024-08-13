import type { Bullet } from './bullet';
import type { GameStatus } from './gameStatus';
import type { Tank } from './tank';
import type { Map } from './map';

export type BoardState = {
	Width: number;
	Height: number;
	Tanks: Tank[];
	Bullets: Bullet[];
	Status: GameStatus;
	Map: Map;
};
