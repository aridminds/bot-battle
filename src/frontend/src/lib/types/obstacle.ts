import type { Position } from './position';

export type Obstacle = {
	Id: String;
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
