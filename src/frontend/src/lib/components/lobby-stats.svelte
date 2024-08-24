<script lang="ts">
	import type { BoardState } from '$lib/types/board-state';
	import type { Lobby } from '$lib/types/lobby';
	import type { Obstacle } from '$lib/types/obstacle';
	import type { Tank } from '$lib/types/tank';
	import { TankStatus } from '$lib/types/tankStatus';

	export let lobby: Lobby;
	export let boardState: BoardState | undefined;
	let eventLogs = '';
	let textareaRef: HTMLTextAreaElement | null = null;
	let players: Tank[] = [];
	let obstacles: Obstacle[] = [];
	let turns = 0;

	$: {
		players = [...(boardState?.Tanks ?? [])];
		obstacles = [...(boardState?.Obstacles ?? [])];
		players = players.sort((a, b) => {
			if (a.DiedInTurn >= 0 && b.DiedInTurn >= 0) {
				return b.DiedInTurn - a.DiedInTurn;
			}
			if (a.DiedInTurn >= 0) {
				return 1;
			}
			if (b.DiedInTurn >= 0) {
				return -1;
			}
			return b.Health - a.Health;
		});
		eventLogs =
			boardState?.EventLogs.map((log, index) => `Turn ${log.Turn}: ${log.Message}`).join('\n') ??
			'';

		if (boardState) {
			turns = boardState.Turns;
		}
		if (textareaRef) {
			textareaRef.scrollTop = textareaRef.scrollHeight;
		}
	}
</script>

<div class="p-4 h-full gap-2 w-full flex flex-col overflow-auto">
	<div class="">
		<h1 class="text-2xl font-bold">Lobby: {lobby.name}</h1>
		<p>Players: {lobby.players.length}</p>
		<p>Turn: {turns}</p>
	</div>
	<div class="flex flex-col gap-2">
		<h1 class="text-lg font-bold">Players</h1>
		<div>
			<div class="grid grid-cols-3 auto-cols-min">
				<p class="font-bold">Name</p>
				<p class="font-bold">Health</p>
				<p class="font-bold">Points</p>
			</div>

			{#each players as player, id}
				<div class={'grid grid-cols-3 ' + (player.Status == TankStatus.Dead ? 'text-red-600' : '')}>
					<p>{id + 1} {player.Name}</p>
					<p>{player.Health}</p>
					<!-- <p>{JSON.stringify(player.Position)}</p> -->
					<p>{player.PointRegister}</p>
				</div>
			{/each}
		</div>
	</div>
	<div class="flex flex-col flex-grow overflow-auto w-full gap-2">
		<h1 class="text-lg font-bold">Events</h1>
		<textarea
			rows="4"
			bind:this={textareaRef}
			bind:value={eventLogs}
			readonly
			class="resize-none w-full h-full"
		></textarea>
	</div>
</div>
