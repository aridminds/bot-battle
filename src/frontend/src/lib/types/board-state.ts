import type { Bullet } from './bullet';
import type { EventLog } from './eventLog';
import type { GameStatus } from './gameStatus';
import type { Tank } from './tank';

export type BoardState = {
	Width: number;
	Height: number;
	Tanks: Tank[];
	Bullets: Bullet[];
	Status: GameStatus;
	EventLogs: EventLog[];
	Turns: number;
};
