import type { Position } from './position';
import type { Tank } from './tank';

export type Bullet = {
	Target: Position;
	Source: Position;
	Shooter: Tank;
};
