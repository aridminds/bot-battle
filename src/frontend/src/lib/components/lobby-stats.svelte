<script lang="ts">
	import type { BoardState } from '$lib/types/board-state';
	import type { Lobby } from '$lib/types/lobby';
	import type { Tank } from '$lib/types/tank';
	import { TankStatus } from '$lib/types/tankStatus';

	export let lobby: Lobby;
	export let boardState: BoardState | undefined;

	let players: Tank[] = [];

	$: {
		players = [...(boardState?.Tanks ?? [])];
		players = players.sort((a, b) => b.Health - a.Health);
	}
</script>

<div class="p-4 h-full w-full overflow-auto border-l border-stone-700">
	<div class="grid grid-rows-2 w-full">
		<div>
			<h1 class="text-2xl font-bold pb-2">{lobby.name}</h1>
			<p>Players: {lobby.players.length}</p>
		</div>
		<div>
			<h1 class="text-xl font-bold pb-2">Players</h1>
			<div class="grid grid-cols-3 auto-cols-min">
				<p class="font-bold">Name</p>
				<p class="font-bold">Health</p>
				<p class="font-bold">Position</p>
			</div>

			{#each players as player, id}
				<div class={'grid grid-cols-3 ' + (player.Status == TankStatus.Dead ? 'text-red-600' : '')}>
					<p>{id + 1} {player.Name}</p>
					<p>{player.Health}</p>
					<p>{JSON.stringify(player.Position)}</p>
				</div>
			{/each}
		</div>
	</div>
</div>
