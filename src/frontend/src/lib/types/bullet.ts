import type { Position } from './position';
import type { Tank } from './tank';

export type Bullet = {
	Id: string;
	Shooter: Tank;
	CurrentPosition: Position;
	Status: BulletStatus;
};

export enum BulletStatus {
	ShotStart,
	InFlight,
	Hit,
	SuperHit
}
