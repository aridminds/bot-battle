<script lang="ts">
	import { Layer } from 'svelte-canvas';
	import { tweened } from 'svelte/motion';
	import { quadOut as easing } from 'svelte/easing';
	import { onMount } from 'svelte';
	import Atlas from '$lib/images/terrainTiles_default.png?raw';
	import type { Bullet } from '$lib/types/bullet';

	export let tileSize: number;
	export let tile: number;
	export let column: number;
	export let row: number;

	let atlas: CanvasImageSource;
	const atlasTileSize = 64;
	// $: {
	// 	console.log('tileSize', tileSize);
	// 	console.log('tile', tile);
	// 	console.log('column', column);
	// 	console.log('row', row);
	// }

	onMount(() => {
		atlas = new Image();
		atlas.src = 'terrainTiles_default.png';
	});

	const render = ({
		context,
		width,
		height
	}: {
		context: CanvasRenderingContext2D;
		width: number;
		height: number;
	}) => {
		const atlasColumn = (tile - 1) % 10;
		const atlasRow = Math.floor((tile - 1) / 10);

		const boolee = context.imageSmoothingEnabled;
		context.imageSmoothingEnabled = false;

		context.drawImage(
			atlas, // image
			atlasColumn * atlasTileSize, // source x
			atlasRow * atlasTileSize, // source y
			atlasTileSize, // source width
			atlasTileSize, // source height
			column * tileSize, // target x
			row * tileSize, // target y
			tileSize, // target width
			tileSize // target height
		);

		context.imageSmoothingEnabled = boolee;
	};
</script>

<Layer {render} />
