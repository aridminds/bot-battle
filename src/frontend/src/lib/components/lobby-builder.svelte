<script lang="ts">
	import { getApiPath } from '$lib/base-path';
	import { getTile } from '$lib/types/lobby';
	import { onMount } from 'svelte';
	import { Canvas } from 'svelte-canvas';
	import Arena from './arena.svelte';
	import GroundTile from './ground-tile.svelte';
	import { Label } from 'bits-ui';

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
	<div class="flex flex-col h-full justify-between gap-2 p-6">
		<h1 class="text-2xl">Lobbybuilder</h1>
		<div class="flex flex-col h-full gap-2">
			<div>
				<p class="font-bold">Lobby settings</p>
				<Label.Root for="lobbySize" class="text-sm font-medium">Max players</Label.Root>
				<div class="relative w-full">
					<input
						id="lobbySize"
						class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
						placeholder="Max players"
						type="number"
						min="2"
						bind:value={lobbySize}
					/>
				</div>
				<Label.Root for="roundDuration" class="text-sm font-medium">Round duration</Label.Root>
				<div class="relative w-full">
					<input
						id="roundDuration"
						class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
						placeholder="Round duration (milliseconds)"
						type="number"
						min="50"
						bind:value={roundDuration}
					/>
				</div>
			</div>

			<div>
				<p class="font-bold">Map settings</p>
				<Label.Root for="arenaWidth" class="text-sm font-medium">Width</Label.Root>
				<div class="relative w-full">
					<input
						id="arenaWidth"
						class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
						placeholder="Width"
						type="number"
						min="2"
						bind:value={arenaWidth}
					/>
				</div>
				<Label.Root for="arenaHeight" class="text-sm font-medium">Height</Label.Root>
				<div class="relative w-full">
					<input
						id="arenaHeight"
						class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
						placeholder="Height"
						type="number"
						min="2"
						bind:value={arenaHeight}
					/>
				</div>
			</div>
			<button class="rounded-md border border-zinc-900 h-min px-2 py-1" on:click={generateMap}
				>Generate map</button
			>
			<span class="flex flex-grow"></span>
			<button class="rounded-md border border-zinc-900 h-min px-2 py-1" on:click={createLobby}
				>Create match</button
			>
		</div>
	</div>
</div>
