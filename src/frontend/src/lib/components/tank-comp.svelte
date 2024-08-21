<script lang="ts">
	import type { Tank } from '$lib/types/tank';
	import { calculateRotationRadians } from '$lib/types/tank';
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import { TankStatus } from '$lib/types/tankStatus';

	export let tank: Tank;
	export let tileSize: number;
	export let canvasWidth: number;
	export let canvasHeight: number;
	export let arenaWidth: number;
	export let arenaHeight: number;

	let logo: CanvasImageSource;

	onMount(() => {
		logo = new Image();
		logo.src = `tank_dark.svg`;
	});

	const position = tweened(
		[tank.Position.X * (canvasWidth / arenaWidth), tank.Position.Y * (canvasHeight / arenaHeight)],
		{ duration: 500, easing }
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
		const x = tank.Position.X * (width / arenaWidth);
		const y = tank.Position.Y * (height / arenaHeight);
		position.set([x, y]);
		const [xx, yy] = $position;

		context.font = `${14}px sans-serif`;
		context.textAlign = 'center';
		context.textBaseline = 'middle';
		context.fillStyle = tank.Health > 0 ? 'black' : 'red';

		var color = 'black';

		switch (tank.Status) {
			case TankStatus.Dead:
				color = 'red';
				break;
			case TankStatus.Winner:
				color = 'green';
				break;
			default:
				color = 'black'; // Standardfarbe
				break;
		}

		let halfTileSize = tileSize / 2;

		context.fillStyle = color;
		context.fillText(tank.Name, xx + halfTileSize, yy - 20);

		context.font = `${14}px sans-serif`;
		context.textAlign = 'center';
		context.textBaseline = 'middle';
		context.fillStyle = color;
		context.fillText(tank.Health.toString(), xx + halfTileSize, yy - 35);

		context.save();
		context.translate(xx + halfTileSize, yy + halfTileSize);
		context.rotate(calculateRotationRadians(tank.Position, -90));
		context.drawImage(logo, -halfTileSize, -halfTileSize, tileSize, tileSize);
		context.restore();
	};
</script>

<Layer {render} />
