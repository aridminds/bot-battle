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

	let lastState: BulletStatus | null = null;

	const sprite = new Sprite(Assets.get(assetId));
	sprite.anchor.set(0.5);
	sprite.scale = 0.8;
	sprite.angle = calculateRotationDegrees(bullet.CurrentPosition, rotationOffset);
	sprite.x = bullet.CurrentPosition.X * tileSize + tileSize / 2;
	sprite.y = bullet.CurrentPosition.Y * tileSize + tileSize / 2;

	getOrCreateLayer(10).addChild(sprite);

	const moveDuration = 1000;
	let targetPosition = {
		X: bullet.CurrentPosition.X * tileSize + tileSize / 2,
		Y: bullet.CurrentPosition.Y * tileSize + tileSize / 2
	};
	let startPosition = targetPosition;
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
			case BulletStatus.ShotStart:
			case BulletStatus.InFlight:
				if (newPosition.X != targetPosition.X || newPosition.Y != targetPosition.Y) {
					moveTime = 0;
					startPosition = { X: sprite.x, Y: sprite.y };
					targetPosition = newPosition;
				}
				break;
			case BulletStatus.Hit:
				if (lastState !== BulletStatus.Hit) {
					sprite.width = sprite.height = 3 * tileSize;
					sprite.texture = Assets.get(boomAssetId);
					sprite.angle = Math.floor(Math.random() * 360);
				}
				sprite.x = newPosition.X;
				sprite.y = newPosition.Y;
				break;
			case BulletStatus.SuperHit:
				if (lastState !== BulletStatus.Hit) {
					sprite.width = sprite.height = 5 * tileSize;
					sprite.texture = Assets.get(boomAssetId);
					sprite.angle = Math.floor(Math.random() * 360);
				}
				sprite.x = newPosition.X;
				sprite.y = newPosition.Y;
				break;
		}
		lastState = bullet.Status;
	}
</script>

<span>Bullet {bullet.Id}</span>
