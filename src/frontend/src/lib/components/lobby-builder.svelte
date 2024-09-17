<script lang="ts">
	import { getApiPath } from '$lib/base-path';
	import { getTile } from '$lib/types/lobby';
	import { onMount } from 'svelte';
	import { Canvas } from 'svelte-canvas';
	import Arena from './arena.svelte';
	import { Label } from 'bits-ui';
	import Ground from './pixi/ground.svelte';
	import Pixi from './pixi/pixi.svelte';
	import Assetloader from './pixi/assetloader.svelte';
	import type { ApplicationOptions, UnresolvedAsset } from 'pixi.js';

	let lobbySize: number = 10;
	let roundDuration: number = 200;
	let arenaWidth: number = 40;
	let arenaHeight: number = 20;
	let arenaWidthTemp: number = arenaWidth;
	let arenaHeightTemp: number = arenaHeight;
	let mapTiles: number[] = [];
	let canvasContainer;

	const options: Partial<ApplicationOptions> = {
		backgroundColor: 'white'
	};
	const assets: UnresolvedAsset[] = [
		{
			alias: 'ground',
			src: '/terrainTiles_default.png',
			data: { scaleMode: 'nearest' }
		}
	];

	export let onCreateLobby: () => void;

	onMount(async () => {
		await generateMap();
	});

	async function generateMap() {
		arenaWidthTemp = arenaWidth;
		arenaHeightTemp = arenaHeight;

		const res = await fetch(`${getApiPath()}/map/generate/${arenaWidthTemp}/${arenaHeightTemp}`);
		mapTiles = (await res.json()) as number[];
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
			<canvas bind:this={canvasContainer} />
			{#if canvasContainer !== undefined && mapTiles.length > 0}
				<Pixi
					options={{
						...options,
						height,
						width,
						canvas: canvasContainer
						// resizeTo: canvasContainer,
					}}
					dynHeight={height}
					dynWidth={width}
				>
					<Assetloader bundleId="botbattle" {assets}>
						<Ground {mapTiles} {tileSize} tileRows={arenaWidthTemp} />
					</Assetloader>
				</Pixi>
			{/if}
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
