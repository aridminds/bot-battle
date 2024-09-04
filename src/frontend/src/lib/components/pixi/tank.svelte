<script lang="ts">
	import { calculateRotationDegrees, type Tank } from '$lib/types/tank';
	import { Assets, Container, Sprite, Text, TextStyle, Ticker } from 'pixi.js';
	import { getOrCreateLayer, getTicker } from './pixi.svelte';
	import { onDestroy } from 'svelte';
	import { backIn, backOut } from 'svelte/easing';
	import { TankStatus } from '$lib/types/tankStatus';

	const assetId = 'tank';
	const rotationOffset = -90;

	export let tankInfo: Tank;
	export let tileSize: number;

	let lastState: TankStatus | null = null;

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
		anchor: { x: 0.5, y: 1 }
	});
	tankContainer.position = {
		x: tankInfo.Position.X * tileSize + tileSize / 2,
		y: tankInfo.Position.Y * tileSize + tileSize / 2
	};

	tankContainer.addChild(sprite, label);
	getOrCreateLayer(5).addChild(tankContainer);

	const rotationDuration = 500;
	let targetRotation = calculateRotationDegrees(tankInfo.Position, rotationOffset);
	let startRotation = targetRotation;
	let rotationTime = 0;

	const moveDuration = 500;
	let targetPosition = {
		X: tankInfo.Position.X * tileSize + tileSize / 2,
		Y: tankInfo.Position.Y * tileSize + tileSize / 2
	};
	let startPosition = targetPosition;
	let moveTime = 0;

	const ticker = getTicker();
	ticker.add(animate, ['tank', tankInfo.Name]);

	function animate(ticker: Ticker) {
		if (targetRotation !== sprite.angle && rotationTime < rotationDuration) {
			rotationTime += ticker.deltaMS;
			sprite.angle =
				startRotation +
				(targetRotation - startRotation) *
					backOut(Math.min(rotationTime, rotationDuration) / rotationDuration);
			return;
		}

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
		tankContainer.removeFromParent();
		tankContainer.destroy();
		ticker.remove(animate, ['tank', tankInfo.Name]);
	});

	$: {
		label.text = `${tankInfo.Health}\n${tankInfo.Name}`;
		const newPosition = {
			X: tankInfo.Position.X * tileSize + tileSize / 2,
			Y: tankInfo.Position.Y * tileSize + tileSize / 2
		};
		if (newPosition.X != targetPosition.X || newPosition.Y != targetPosition.Y) {
			moveTime = 0;
			startPosition = { X: tankContainer.x, Y: tankContainer.y };
			targetPosition = newPosition;
		}

		const newRotation = calculateRotationDegrees(tankInfo.Position, rotationOffset);
		if (newRotation != targetRotation) {
			rotationTime = 0;
			startRotation = sprite.angle;
			targetRotation = newRotation;
		}
		switch (tankInfo.Status) {
			case TankStatus.Dead:
				label.style.fill = 'red';
				break;
			case TankStatus.Winner:
				label.style.fill = 'green';
				break;
			case TankStatus.IsStucked:
				if (lastState === TankStatus.IsStucked) {
					rotationTime = 0;
					startRotation = sprite.angle;
					targetRotation = 360 + sprite.angle;
				}
			case TankStatus.Alive:
			default:
				label.style.fill = 'black'; // Standardfarbe
				break;
		}
		lastState = tankInfo.Status;
	}
</script>

<span>Tank {tankInfo.Name}</span>
