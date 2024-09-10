<script lang="ts">
	import { writable } from 'svelte/store';
	import type { BoardState } from '$lib/types/board-state';
	import type { Tank } from '$lib/types/tank';
	import type { Bullet } from '$lib/types/bullet';
	import { page } from '$app/stores';
	import { onDestroy } from 'svelte';
	import Arena from '$lib/components/arena.svelte';
	import { type Lobby } from '$lib/types/lobby';
	import LobbyStats from '$lib/components/lobby-stats.svelte';
	import { getApiPath } from '$lib/base-path';
	import type { Obstacle } from '$lib/types/obstacle';
	import type { ApplicationOptions, UnresolvedAsset } from 'pixi.js';
	import TankAsset from '$lib/components/pixi/tank.svelte';
	import BulletAsset from '$lib/components/pixi/bullet.svelte';
	import Ground from '$lib/components/pixi/ground.svelte';
	import ObstacleAsset from '$lib/components/pixi/obstacle.svelte';
	import Assetloader from '$lib/components/pixi/assetloader.svelte';
	import Pixi from '$lib/components/pixi/pixi.svelte';

	let tanks: Tank[] = [];
	let bullets: Bullet[] = [];
	let obstacles: Obstacle[] = [];
	let canvasContainer;

	const options: Partial<ApplicationOptions> = {
		backgroundColor: 'chocolate'
	};
	const assets: UnresolvedAsset[] = [
		{
			alias: 'tank',
			src: '/tank_dark.svg'
		},
		{
			alias: 'bullet',
			src: '/bullet_dark_outline.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'ground',
			src: '/terrainTiles_default.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'bullet-boom',
			src: '/explosion.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'stone',
			src: '/stone_large.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'tree',
			src: '/treeGreen_large.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'leaf',
			src: '/treeGreen_leaf.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'tree-small',
			src: '/treeGreen_small.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'tree-destroyed',
			src: '/treeBrown_twigs.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'red-barrel',
			src: '/barrelBlack_top.png',
			data: { scaleMode: 'nearest' }
		},
		{
			alias: 'oil-stain',
			src: '/oilSpill_large.png',
			data: { scaleMode: 'nearest' }
		}
	];

	const boardState = writable<BoardState>();

	const evtSource = new EventSource(
		`${getApiPath()}/matchmaking/lobbies/${$page.params.lobby}/sse`
	);
	evtSource.onmessage = function (event) {
		var bla: BoardState = JSON.parse(event.data) as BoardState;
		boardState.set(bla);
		tanks = $boardState.Tanks;
		bullets = $boardState.Bullets;
		obstacles = $boardState.Obstacles;
	};

	async function updateBoardState() {
		const res = await fetch(`${getApiPath()}/matchmaking/lobbies/${$page.params.lobby}`);
		const data = (await res.json()) as Lobby;
		return data;
	}

	onDestroy(() => {
		evtSource.close();
	});
</script>

{#await updateBoardState()}
	<div class="h-full w-full flex justify-center items-center">
		<p>Loading lobby...</p>
	</div>
{:then lobby}
	<div class="grid grid-cols-3 h-full">
		<div class="col-span-2 p-4 h-full bg-black">
			<Arena arenaHeight={lobby.height} arenaWidth={lobby.width} let:height let:width let:tileSize>
				<canvas bind:this={canvasContainer} />
				{#if canvasContainer !== undefined}
					<Pixi
						options={{ ...options, height, width, canvas: canvasContainer }}
						dynHeight={height}
						dynWidth={width}
					>
						<Assetloader bundleId="botbattle" {assets}>
							<Ground mapTiles={lobby.mapTiles} {tileSize} tileRows={lobby.width}>
								{#each tanks as tankInfo (tankInfo.Name)}
									<TankAsset {tankInfo} {tileSize} roundDuration={1000} />
								{/each}
								{#each bullets as bullet (bullet.Id)}
									<BulletAsset {bullet} {tileSize} roundDuration={1000} />
								{/each}
								{#each obstacles as obstacle}
									<ObstacleAsset {tileSize} {obstacle} />
								{/each}
							</Ground>
						</Assetloader>
					</Pixi>
				{/if}
			</Arena>
		</div>
		<LobbyStats {lobby} boardState={$boardState}></LobbyStats>
	</div>
{:catch error}
	<p style="color: red">{error.message}</p>
{/await}
