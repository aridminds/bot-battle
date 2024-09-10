import type { Position } from './position';
import { Direction } from './direction';
import { TankStatus } from './tankStatus';

export type Tank = {
	Name: string;
	Health: number;
	Position: Position;
	Status: TankStatus;
	PointRegister: number;
	DiedInTurn: number;
};

export function calculateRotationDegrees(direction: Direction, offset: number = 0): number {
	let rotation: number = offset;

	switch (direction) {
		default:
		case Direction.North:
			rotation += 270;
			break;
		case Direction.NorthEast:
			rotation += 315;
			break;
		case Direction.East:
			rotation += 0;
			break;
		case Direction.SouthEast:
			rotation += 45;
			break;
		case Direction.South:
			rotation += 90;
			break;
		case Direction.SouthWest:
			rotation += 135;
			break;
		case Direction.West:
			rotation += 180;
			break;
		case Direction.NorthWest:
			rotation += 225;
			break;
	}

	return rotation;
}

export function calculateRotationRadians(direction: Direction, offsetDegree: number = 0): number {
	return (calculateRotationDegrees(direction, offsetDegree) * Math.PI) / 180;
}
