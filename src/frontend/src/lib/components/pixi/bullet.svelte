<script lang="ts">
	import { BulletStatus, type Bullet } from '$lib/types/bullet';
	import { Assets, Sprite, type Ticker } from 'pixi.js';
	import { getOrCreateLayer, getTicker } from './pixi.svelte';
	import { calculateRotationDegrees } from '$lib/types/tank';
	import { onDestroy } from 'svelte';
	import { linear } from 'svelte/easing';
	const assetId = 'bullet';
	const boomAssetId = 'bullet-boom';
	const rotationOffset = 90;

	export let bullet: Bullet;
	export let tileSize: number;
	export let roundDuration: number;

	let lastState: BulletStatus | null = null;
	let bulletExploded = false;

	const sprite = new Sprite(Assets.get(assetId));
	sprite.anchor.set(0.5);
	sprite.scale = 0.8;
	sprite.angle = calculateRotationDegrees(bullet.CurrentPosition.Direction, rotationOffset);
	sprite.x = bullet.Shooter.Position.X * tileSize + tileSize / 2;
	sprite.y = bullet.Shooter.Position.Y * tileSize + tileSize / 2;

	getOrCreateLayer(10).addChild(sprite);

	const moveDuration = roundDuration;
	let targetPosition = {
		X: bullet.CurrentPosition.X * tileSize + tileSize / 2,
		Y: bullet.CurrentPosition.Y * tileSize + tileSize / 2
	};
	let startPosition = {
		X: sprite.x,
		Y: sprite.y
	};
	let moveTime = 0;

	const ticker = getTicker();
	const ctx = ['bullet', bullet.Id];
	ticker.add(animate, ctx);

	function animate(ticker: Ticker) {
		if (
			(targetPosition.X !== sprite.x || targetPosition.Y !== sprite.y) &&
			moveTime < moveDuration
		) {
			moveTime += ticker.deltaMS;
			const factor = linear(Math.min(moveTime, moveDuration) / moveDuration);
			sprite.x = startPosition.X + (targetPosition.X - startPosition.X) * factor;
			sprite.y = startPosition.Y + (targetPosition.Y - startPosition.Y) * factor;

			return;
		}

		if (
			!bulletExploded &&
			(bullet.Status == BulletStatus.Hit || bullet.Status == BulletStatus.SuperHit)
		) {
			bulletExploded = true;
			const sizeFactor = bullet.Status == BulletStatus.Hit ? 3 : 5;
			sprite.width = sprite.height = sizeFactor * tileSize;
			sprite.texture = Assets.get(boomAssetId);
			sprite.angle = Math.floor(Math.random() * 360);
		}
	}

	onDestroy(() => {
		ticker.remove(animate, ctx);
		sprite.removeFromParent();
		sprite.destroy();
	});

	$: {
		const newPosition = {
			X: bullet.CurrentPosition.X * tileSize + tileSize / 2,
			Y: bullet.CurrentPosition.Y * tileSize + tileSize / 2
		};
		switch (bullet.Status) {
			case BulletStatus.ShotStart: {
				if (newPosition.X != targetPosition.X || newPosition.Y != targetPosition.Y) {
					moveTime = 0;
					startPosition = { X: sprite.x, Y: sprite.y };
					targetPosition = newPosition;
				}

				break;
			}
			case BulletStatus.InFlight:
				break;
			case BulletStatus.Hit:
				break;
			case BulletStatus.SuperHit:
				break;
		}
		lastState = bullet.Status;
	}
</script>

<span>Bullet {bullet.Id}</span>
