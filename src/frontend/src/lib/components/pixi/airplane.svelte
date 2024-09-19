<script lang="ts">
	import { Assets, Sprite, type Ticker } from 'pixi.js';
	import { getOrCreateLayer, getTicker } from './pixi.svelte';
	import { calculateRotationDegrees } from '$lib/types/tank';
	import { onDestroy } from 'svelte';
	import { linear } from 'svelte/easing';
	import { ParachuteStatus, type Airplane } from '$lib/types/airplane';
	import { Direction } from '$lib/types/direction';
	const assetId = 'airplane';
	const assetShadowId = 'airplaneShadow';
	const parachuteAssetId = 'parachute';
	const rotationOffset = 0;

	export let airplane: Airplane;
	export let tileSize: number;
	export let roundDuration: number;

	let lastState: ParachuteStatus | null = null;

	const sprite = new Sprite(Assets.get(assetId));
	sprite.anchor.set(0.5);
	sprite.scale = 1.2;
	sprite.angle = calculateRotationDegrees(Direction.East, rotationOffset);
	sprite.x = -100 * tileSize + tileSize / 2;
	sprite.y = 10 * tileSize + tileSize / 2;

	const shadowSprite = new Sprite(Assets.get(assetShadowId));
	shadowSprite.anchor.set(0.5);
	shadowSprite.scale = 1.2;
	shadowSprite.angle = calculateRotationDegrees(Direction.East, rotationOffset);
	shadowSprite.x = -100 * tileSize + tileSize / 2;
	shadowSprite.y = 10 * tileSize + tileSize / 2;

	const parachuteSprite = new Sprite(Assets.get(parachuteAssetId));
	parachuteSprite.anchor.set(0.5);
	parachuteSprite.scale = 0.0;
	parachuteSprite.angle = calculateRotationDegrees(Direction.East, rotationOffset);

	getOrCreateLayer(11).addChild(sprite);
	getOrCreateLayer(10).addChild(shadowSprite);
	getOrCreateLayer(9).addChild(parachuteSprite);

	const moveDuration = roundDuration;
	let targetPosition = {
		X: airplane.Position.X * tileSize + tileSize / 2,
		Y: airplane.Position.Y * tileSize + tileSize / 2
	};
	let startPosition = {
		X: sprite.x,
		Y: sprite.y
	};
	let moveTime = 0;

	const ticker = getTicker();
	const ctx = ['airplane', 'airplane'];
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

			shadowSprite.x = sprite.x - 15;
			shadowSprite.y = sprite.y + 15;

			if (airplane.ParachuteStatus != ParachuteStatus.OnThePlane) {
				parachuteSprite.x = airplane.DroppingGiftPosition.X * tileSize + tileSize / 2;
				parachuteSprite.y = airplane.DroppingGiftPosition.Y * tileSize + tileSize / 2;
			}

			return;
		}
	}

	onDestroy(() => {
		ticker.remove(animate, ctx);
		sprite.removeFromParent();
		sprite.destroy();

		shadowSprite.removeFromParent();
		shadowSprite.destroy();
	});

	$: {
		const newPosition = {
			X: airplane.Position.X * tileSize + tileSize / 2,
			Y: airplane.Position.Y * tileSize + tileSize / 2
		};
		if (airplane.IsFlying) {
			moveTime = 0;
			startPosition = { X: sprite.x, Y: sprite.y };
			targetPosition = newPosition;
		} else {
			sprite.x = -100;
			sprite.y = 10;

			shadowSprite.x = -100;
			shadowSprite.y = 10;
		}

		switch (airplane.ParachuteStatus) {
			case ParachuteStatus.OnThePlane:
				parachuteSprite.scale = 0.0;
				break;
			case ParachuteStatus.InAirHigh:
				parachuteSprite.scale = 1.1;
				break;
			case ParachuteStatus.InAirMiddle:
				parachuteSprite.scale = 0.9;
				break;
			case ParachuteStatus.OnGround:
				parachuteSprite.scale = 0.7;
				break;
			case ParachuteStatus.PackedTogether:
				parachuteSprite.scale = 0.5;
				break;
			case ParachuteStatus.Delivered:
				parachuteSprite.scale = 0.0;
				break;
		}

		lastState = airplane.ParachuteStatus;
	}
</script>

<span>Airplane</span>
