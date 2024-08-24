<script lang="ts">
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import { BulletStatus, type Bullet } from '$lib/types/bullet';
	import { calculateRotationRadians } from '$lib/types/tank';

	export let bullet: Bullet;
	export let tileSize: number;
	export let canvasWidth: number;
	export let canvasHeight: number;
	export let arenaWidth: number;
	export let arenaHeight: number;

	let cannonBallImage: CanvasImageSource;
	let explosionImage: CanvasImageSource;

	onMount(() => {
		cannonBallImage = new Image();
		cannonBallImage.src = `bullet_dark_outline.png`;

		explosionImage = new Image();
		explosionImage.src = 'explosion.png';
	});

	const position = tweened(
		[
			bullet.Shooter.Position.X * (canvasHeight / arenaHeight),
			bullet.Shooter.Position.Y * (canvasWidth / arenaWidth)
		],
		{ duration: 50, easing }
	);

	$: render = ({
		context,
		width,
		height
	}: {
		context: CanvasRenderingContext2D;
		width: number;
		height: number;
	}) => {
		const x = bullet.CurrentPosition.X * (width / arenaWidth);
		const y = bullet.CurrentPosition.Y * (height / arenaHeight);
		position.set([x, y]);
		let [xx, yy] = $position;

		if (bullet.Status === BulletStatus.Hit || bullet.Status === BulletStatus.SuperHit) {
			var scale = 3;

			if (bullet.Status === BulletStatus.SuperHit) {
				scale = 5;
			}

			context.drawImage(
				explosionImage as HTMLImageElement,
				0,
				0,
				(explosionImage as HTMLImageElement).width,
				(explosionImage as HTMLImageElement).height,
				xx + tileSize / 2 - (tileSize * scale) / 2,
				yy + tileSize / 2 - (tileSize * scale) / 2,
				tileSize * scale,
				tileSize * scale
			);
			return;
		} else {
			const scaleX = 0.25;
			const scaleY = 0.4;

			context.save();
			context.translate(xx + tileSize / 2, yy + tileSize / 2);
			context.rotate(calculateRotationRadians(bullet.CurrentPosition, 90));

			context.drawImage(
				cannonBallImage as HTMLImageElement,
				0,
				0,
				(cannonBallImage as HTMLImageElement).width,
				(cannonBallImage as HTMLImageElement).height,
				(-tileSize * scaleX) / 2,
				(-tileSize * scaleY) / 2,
				tileSize * scaleX,
				tileSize * scaleY
			);
			context.restore();
		}
	};
</script>

<Layer {render} />
