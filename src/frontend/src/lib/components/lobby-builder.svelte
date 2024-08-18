<script lang="ts">
	import { getApiPath } from '$lib/base-path';
	import { getTile } from '$lib/types/lobby';
	import { onMount } from 'svelte';
	import { Canvas } from 'svelte-canvas';
	import Arena from './arena.svelte';
	import GroundTile from './ground-tile.svelte';

	let lobbySize: number = 10;
	let roundDuration: number = 200;
	let arenaWidth: number = 40;
	let arenaHeight: number = 20;
	let arenaWidthTemp: number = arenaWidth;
	let arenaHeightTemp: number = arenaHeight;
	let mapTiles: number[] = [];
	let canvas: Canvas;

	export let onCreateLobby: () => void;

	onMount(async () => {
		await generateMap();
	});

	async function generateMap() {
		arenaWidthTemp = arenaWidth;
		arenaHeightTemp = arenaHeight;

		const res = await fetch(`${getApiPath()}/map/generate/${arenaWidthTemp}/${arenaHeightTemp}`);
		mapTiles = (await res.json()) as number[];
		canvas.redraw();
	}

	function createLobby() {
		fetch(`${getApiPath()}/matchmaking/lobbies`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify({
				lobbySize: lobbySize,
				roundDuration: roundDuration,
				areaDimensions: [arenaWidthTemp, arenaHeightTemp],
				mapTiles: mapTiles
			})
		}).then(() => onCreateLobby());
	}
</script>

<div class="grid grid-cols-3 h-full">
	<div class="col-span-2 p-4 h-full bg-black">
		<Arena
			arenaHeight={arenaHeightTemp}
			arenaWidth={arenaWidthTemp}
			let:height
			let:width
			let:tileSize
		>
			<Canvas {width} {height} pixelRatio="auto" class="bg-white" bind:this={canvas}>
				{#each Array(arenaWidthTemp) as _, row}
					{#each Array(arenaWidthTemp) as _, column}
						<GroundTile
							{tileSize}
							tile={getTile(mapTiles, arenaWidthTemp, column, row)}
							{column}
							{row}
						></GroundTile>
					{/each}
				{/each}
			</Canvas>
		</Arena>
	</div>
	<div class="col-span-1 p-4 h-full flex flex-col gap-4">
		<div class="flex flex-col gap-2">
			<h2 class="font-bold">Lobby settings</h2>
			<input
				class="w-full rounded-md border border-zinc-900 p-2"
				type="number"
				placeholder="Max players"
				bind:value={lobbySize}
			/>
			<input
				class="w-full rounded-md border border-zinc-900 p-2"
				type="number"
				placeholder="Round duration (milliseconds)"
				bind:value={roundDuration}
			/>
		</div>
		<div class="flex flex-col gap-2">
			<h2 class="font-bold">Map settings</h2>
			<input
				class="w-full rounded-md border border-zinc-900 p-2"
				type="number"
				min="1"
				placeholder="Arena width"
				bind:value={arenaWidth}
			/>
			<input
				class="w-full rounded-md border border-zinc-900 p-2"
				type="number"
				min="1"
				placeholder="Arena height"
				bind:value={arenaHeight}
			/>
			<button class="w-full rounded-md border border-zinc-900 p-2" on:click={generateMap}
				>Regenerate
			</button>
		</div>
		<button class="w-full rounded-md border border-zinc-900 p-2" on:click={createLobby}
			>Create lobby
		</button>
	</div>
</div>
