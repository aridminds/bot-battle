import type { Position } from './position';

export type Airplane = {
	IsFlying: boolean;
	Position: Position;
	ParachuteStatus: ParachuteStatus;
	DroppingGiftPosition: Position;
};

export enum ParachuteStatus {
	OnThePlane,
	InAirHigh,
	InAirMiddle,
	OnGround,
	PackedTogether,
	Delivered
}
