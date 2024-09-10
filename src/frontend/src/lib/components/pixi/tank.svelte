<script lang="ts">
	import { calculateRotationDegrees, type Tank } from '$lib/types/tank';
	import { Assets, Container, Sprite, Text, TextStyle, Ticker } from 'pixi.js';
	import { getOrCreateLayer, getTicker } from './pixi.svelte';
	import { onDestroy } from 'svelte';
	import { backIn, backOut } from 'svelte/easing';
	import { TankStatus } from '$lib/types/tankStatus';

	const assetId = 'tank';
	const rotationOffset = -90;

	export let roundDuration: number;
	export let tileSize: number;
	export let tankInfo: Tank;

	let lastTankInfo: Tank | null = null;

	const animationState = ['tank', tankInfo.Name];
	const tankContainer = new Container({ label: 'tank-container' });
	const sprite = new Sprite(Assets.get(assetId));
	sprite.anchor.set(0.5);
	sprite.width = (Math.sqrt(2) * tileSize) / 2;
	sprite.height = (Math.sqrt(2) * tileSize) / 2;
	const label = new Text({
		style: new TextStyle({
			fill: 'black',
			fontSize: 14,
			fontFamily: 'sans-serif',
			align: 'center'
		}),
		y: -10 - sprite.height / 2,
		anchor: { x: 0.5, y: 1 },
		text: `${tankInfo.Health}\n${tankInfo.Name}`
	});
	tankContainer.position = {
		x: tankInfo.Position.X * tileSize + tileSize / 2,
		y: tankInfo.Position.Y * tileSize + tileSize / 2
	};

	tankContainer.addChild(sprite, label);
	getOrCreateLayer(5).addChild(tankContainer);

	let totalRoundTime = 0;
	const rotationDuration = roundDuration / 3;
	let distanceRotation = 0;
	let startRotation = calculateRotationDegrees(tankInfo.Position.Direction, rotationOffset);
	let targetRotation = startRotation;
	let rotationTime = 0;

	const moveDuration = roundDuration / 3;
	let targetPosition = {
		X: tankInfo.Position.X * tileSize + tileSize / 2,
		Y: tankInfo.Position.Y * tileSize + tileSize / 2
	};
	let startPosition = targetPosition;
	let moveTime = 0;

	const ticker = getTicker();
	ticker.add(animate, animationState);

	function animate(ticker: Ticker) {
		totalRoundTime += ticker.deltaMS;
		if (totalRoundTime > roundDuration) totalRoundTime -= roundDuration;
		if (distanceRotation != 0 && rotationTime < rotationDuration) {
			rotationTime += ticker.deltaMS;
			sprite.angle =
				(startRotation +
					distanceRotation * backOut(Math.min(rotationTime, rotationDuration) / rotationDuration)) %
				360;
			return;
		}

		if (totalRoundTime < rotationDuration) return;

		if (
			(targetPosition.X !== tankContainer.x || targetPosition.Y !== tankContainer.y) &&
			moveTime < moveDuration
		) {
			moveTime += ticker.deltaMS;
			const factor = backIn(Math.min(moveTime, moveDuration) / moveDuration);
			tankContainer.x = startPosition.X + (targetPosition.X - startPosition.X) * factor;
			tankContainer.y = startPosition.Y + (targetPosition.Y - startPosition.Y) * factor;
		}
	}

	onDestroy(() => {
		ticker.remove(animate, animationState);
		tankContainer.removeFromParent();
		tankContainer.destroy();
	});

	$: {
		if (lastTankInfo?.Name != tankInfo.Name || lastTankInfo?.Health != tankInfo.Health)
			label.text = `${tankInfo.Health}\n${tankInfo.Name}`;

		if (
			lastTankInfo?.Position.X != tankInfo.Position.X ||
			lastTankInfo?.Position.Y != tankInfo.Position.Y
		) {
			const newPosition = {
				X: tankInfo.Position.X * tileSize + tileSize / 2,
				Y: tankInfo.Position.Y * tileSize + tileSize / 2
			};
			if (newPosition.X != targetPosition.X || newPosition.Y != targetPosition.Y) {
				moveTime = 0;
				startPosition = { X: tankContainer.x, Y: tankContainer.y };
				targetPosition = newPosition;
			}
		}

		if (lastTankInfo?.Position.Direction != tankInfo.Position.Direction) {
			const newRotation = calculateRotationDegrees(tankInfo.Position.Direction, rotationOffset);
			if (newRotation != targetRotation) {
				startRotation = sprite.angle;
				targetRotation = newRotation;
				const l = targetRotation - startRotation;
				const r = l - 360;
				distanceRotation = Math.abs(l) < Math.abs(r) ? l : r;
				rotationTime = 0;
			}
		}

		if (lastTankInfo?.Status != tankInfo.Status) {
			switch (tankInfo.Status) {
				case TankStatus.Dead:
					label.style.fill = 'red';
					break;
				case TankStatus.Winner:
					label.style.fill = 'green';
					break;
				case TankStatus.IsStucked:
					break;
				case TankStatus.Alive:
					if (lastTankInfo?.Status == TankStatus.IsStucked) {
						startRotation = sprite.angle;
						const l = targetRotation - startRotation;
						const r = l - 360;
						distanceRotation = 360 + (Math.abs(l) < Math.abs(r) ? l : r);
						rotationTime = 0;
					}
					break;
				default:
					break;
			}
		}
		lastTankInfo = tankInfo;
	}
</script>

<span>Tank {tankInfo.Name}</span>
