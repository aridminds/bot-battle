<script lang="ts">
	import { Canvas, Layer, type Render } from 'svelte-canvas';
	import { writable } from 'svelte/store';
	import type { BoardState } from '$lib/types/board-state';
	import TankComp from '$lib/components/tank-comp.svelte';
	import BulletComp from '$lib/components/bullet-comp.svelte';
	import type { Tank } from '$lib/types/tank';
	import type { Bullet } from '$lib/types/bullet';
	import { page } from '$app/stores';
	import { onDestroy, onMount } from 'svelte';
	import Arena from '$lib/components/arena.svelte';
	import { getTile, type Lobby } from '$lib/types/lobby';
	import LobbyStats from '$lib/components/lobby-stats.svelte';
	import { getApiPath } from '$lib/base-path';
	import GroundTile from '$lib/components/ground-tile.svelte';
	import type { Obstacle } from '$lib/types/obstacle';
	import ObstacleComp from '$lib/components/obstacle-comp.svelte';

	let width: number;
	let height: number;
	let tanks: Tank[] = [];
	let bullets: Bullet[] = [];
	let obstacles: Obstacle[] = [];

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
				<Canvas {width} {height} pixelRatio="auto" class="bg-white">
					{#each Array(lobby.height) as _, row}
						{#each Array(lobby.width) as _, column}
							<GroundTile
								{tileSize}
								tile={getTile(lobby.mapTiles, lobby.width, column, row)}
								{column}
								{row}
							></GroundTile>
						{/each}
					{/each}

					{#each tanks as tank}
						<TankComp
							{tank}
							{tileSize}
							canvasHeight={height}
							canvasWidth={width}
							arenaHeight={lobby.height}
							arenaWidth={lobby.width}
						></TankComp>
					{/each}
					{#each obstacles as obstacle}
						<ObstacleComp {obstacle} {tileSize} arenaHeight={lobby.height} arenaWidth={lobby.width}
						></ObstacleComp>
					{/each}
					{#each bullets as bullet}
						<BulletComp
							{bullet}
							{tileSize}
							canvasHeight={height}
							canvasWidth={width}
							arenaHeight={lobby.height}
							arenaWidth={lobby.width}
						></BulletComp>
					{/each}
				</Canvas>
			</Arena>
		</div>
		<LobbyStats {lobby} boardState={$boardState}></LobbyStats>
	</div>
{:catch error}
	<p style="color: red">{error.message}</p>
{/await}
