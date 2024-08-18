<script lang="ts">
	import { onMount } from 'svelte';

	export let arenaWidth: number;
	export let arenaHeight: number;

	let containerHeight: number = 0;
	let containertWidth: number = 0;

	let arenaConstrains = calculateConstrains(
		containerHeight,
		containertWidth,
		arenaWidth,
		arenaHeight
	);
	$: arenaConstrains = calculateConstrains(
		containerHeight,
		containertWidth,
		arenaWidth,
		arenaHeight
	);

	onMount(() => {
		containerHeight = document.getElementById('arena-container')?.clientHeight || 0;
		containertWidth = document.getElementById('arena-container')?.clientWidth || 0;

		window.addEventListener('resize', () => {
			containerHeight = document.getElementById('arena-container')?.clientHeight || 0;
			containertWidth = document.getElementById('arena-container')?.clientWidth || 0;
		});
	});

	function calculateConstrains(
		height: number,
		width: number,
		arenaWidth: number,
		arenaHeight: number
	) {
		if (width && height) {
			const resolutionViewport = width / height;
			const resolutionArena = arenaWidth / arenaHeight;

			if (resolutionViewport > resolutionArena) {
				return {
					height: height,
					width: height * resolutionArena,
					tileSize: height / arenaHeight
				};
			} else {
				return {
					height: width / resolutionArena,
					width: width,
					tileSize: width / arenaWidth
				};
			}
		}

		return {
			height: 0,
			width: 0,
			tileSize: 0
		};
	}
</script>

<div id="arena-container" class="w-full h-full flex justify-center items-center">
	{#if arenaConstrains.height && arenaConstrains.width && arenaConstrains.tileSize}
		<slot
			width={arenaConstrains.width}
			height={arenaConstrains.height}
			tileSize={arenaConstrains.tileSize}
		></slot>
	{/if}
</div>
