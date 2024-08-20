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
	import PixiGame from '$lib/components/pixi-game.svelte';

	let tanks: Tank[] = [];
	let bullets: Bullet[] = [];

	const boardState = writable<BoardState>();

	const evtSource = new EventSource(
		`${getApiPath()}/matchmaking/lobbies/${$page.params.lobby}/sse`
	);
	evtSource.onmessage = function (event) {
		var bla: BoardState = JSON.parse(event.data) as BoardState;
		boardState.set(bla);
		tanks = $boardState.Tanks;
		bullets = $boardState.Bullets;
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
				<PixiGame
					{height}
					{width}
					arenaWidth={lobby.width}
					arenaHeight={lobby.height}
					{tileSize}
					mapTiles={lobby.mapTiles}
					{tanks}
					{bullets}
				/>
			</Arena>
		</div>
		<LobbyStats {lobby} boardState={$boardState}></LobbyStats>
	</div>
{:catch error}
	<p style="color: red">{error.message}</p>
{/await}
