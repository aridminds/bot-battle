<script lang="ts">
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import Logo from '$lib/images/cannon_ball.svg?raw';
	import type { Bullet } from '$lib/types/bullet';

	export let bullet: Bullet;
	export let tileSize: number;
	export let canvasWidth: number;
	export let canvasHeight: number;
	export let arenaWidth: number;
	export let arenaHeight: number;

	let logo: CanvasImageSource;

	onMount(() => {
		logo = new Image();
		logo.src = `data:image/svg+xml,${encodeURIComponent(Logo)}`;
	});

	const position = tweened(
		[
			bullet.Shooter.Position.X * (canvasHeight / arenaHeight),
			bullet.Shooter.Position.Y * (canvasWidth / arenaWidth)
		],
		{ duration: 100, easing }
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
		const [xx, yy] = $position;

		let halfTileSize = tileSize / 2;
		let bulletSize = tileSize / 4;

		context.drawImage(logo, xx - halfTileSize, yy - halfTileSize, bulletSize, bulletSize);
	};
</script>

<Layer {render} />
