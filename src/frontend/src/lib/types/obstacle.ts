import type { Position } from './position';

export type Obstacle = {
	Position: Position;
	Type: ObstacleType;
};

export enum ObstacleType {
	Destroyed = 0,
	TreeLeaf = 1,
	TreeSmall = 2,
	TreeLarge = 3,
	Stone = 4
}
