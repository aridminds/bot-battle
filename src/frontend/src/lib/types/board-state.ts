import type { Airplane } from './airplane';
import type { Bullet } from './bullet';
import type { CollectibleItem } from './collectibleItem';
import type { EventLog } from './eventLog';
import type { GameStatus } from './gameStatus';
import type { Obstacle } from './obstacle';
import type { Tank } from './tank';

export type BoardState = {
	Width: number;
	Height: number;
	Tanks: Tank[];
	Bullets: Bullet[];
	Status: GameStatus;
	EventLogs: EventLog[];
	Turns: number;
	Obstacles: Obstacle[];
	CollectibleItems: CollectibleItem[];
	Airplane: Airplane;
};
