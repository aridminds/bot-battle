<script lang="ts">
	import type { Tank } from '$lib/types/tank';
	import { calculateRotationRadians } from '$lib/types/tank';
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import Logo from '$lib/images/tank_dark.svg?raw';
	import { TankStatus } from '$lib/types/tankStatus';

	export let tank: Tank;
	export let canvasWidth: number;
	export let canvasHeight: number;
	export let arenaWidth: number;
	export let arenaHeight: number;

	let logo: CanvasImageSource;

	onMount(() => {
		logo = new Image();
		logo.src = `data:image/svg+xml,${encodeURIComponent(Logo)}`;
	});

	const position = tweened([canvasWidth / 2, canvasHeight / 2], { duration: 500, easing });

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

		context.fillStyle = color;
		context.fillText(tank.Name, xx + 15, yy - 20);

		context.font = `${14}px sans-serif`;
		context.textAlign = 'center';
		context.textBaseline = 'middle';
		context.fillStyle = color;
		context.fillText(tank.Health.toString(), xx + 15, yy - 35);

		context.save();
		context.translate(xx + 15, yy + 15);
		context.rotate(calculateRotationRadians(tank.Position, -90));
		context.drawImage(logo, -15, -15, 30, 30);
		context.restore();
	};
</script>

<Layer {render} />
