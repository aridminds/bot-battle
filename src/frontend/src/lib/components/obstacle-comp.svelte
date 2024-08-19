<script lang="ts">
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import { calculateRotationRadians } from '$lib/types/tank';
	import { ObstacleType, type Obstacle } from '$lib/types/obstacle';

	export let obstacle: Obstacle;
	export let tileSize: number;
	export let arenaWidth: number;
	export let arenaHeight: number;

	let stoneImage: CanvasImageSource;
	let treeImage: CanvasImageSource;
	let treeLeafImage: CanvasImageSource;
	let treeSmallImage: CanvasImageSource;
	let destroyedImage: CanvasImageSource;

	onMount(() => {
		stoneImage = new Image();
		stoneImage.src = `stone_large.png`;

		treeImage = new Image();
		treeImage.src = 'treeGreen_large.png';

		treeLeafImage = new Image();
		treeLeafImage.src = 'treeGreen_leaf.png';

		treeSmallImage = new Image();
		treeSmallImage.src = 'treeGreen_small.png';

		destroyedImage = new Image();
		destroyedImage.src = 'treeBrown_twigs.png';
	});

	$: render = ({
		context,
		width,
		height
	}: {
		context: CanvasRenderingContext2D;
		width: number;
		height: number;
	}) => {
		const x = obstacle.Position.X * (width / arenaWidth);
		const y = obstacle.Position.Y * (height / arenaHeight);

		var htmlImageElement: CanvasImageSource = destroyedImage;
		var scale = 1;

		switch (obstacle.Type) {
			case ObstacleType.Stone:
				scale = 1.25;
				htmlImageElement = stoneImage;
				break;
			case ObstacleType.TreeLarge:
				scale = 1.5;
				htmlImageElement = treeImage;
				break;
			case ObstacleType.TreeLeaf:
				scale = 0.25;
				htmlImageElement = treeLeafImage;
				break;
			case ObstacleType.TreeSmall:
				scale = 1;
				htmlImageElement = treeSmallImage;
				break;
			default:
			case ObstacleType.Destroyed:
				htmlImageElement = destroyedImage;
				break;
		}

		context.drawImage(
			htmlImageElement as HTMLImageElement,
			0,
			0,
			(htmlImageElement as HTMLImageElement).width,
			(htmlImageElement as HTMLImageElement).height,
			x + tileSize / 2 - (tileSize * scale) / 2,
			y + tileSize / 2 - (tileSize * scale) / 2,
			tileSize * scale,
			tileSize * scale
		);
	};
</script>

<Layer {render} />
