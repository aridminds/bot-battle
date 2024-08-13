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
		let flooredTileSize = Math.floor(tileSize);
		let ceiledTileSize = Math.ceil(tileSize);
		context.drawImage(
			atlas, // image
			(tile - 1) * 64, // source x
			0, // source y
			64, // source width
			64, // source height
			column * tileSize, // target x
			row * tileSize, // target y
			tileSize, // target width
			tileSize // target height
		);
	};
</script>

<Layer {render} />
