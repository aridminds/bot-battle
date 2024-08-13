<script lang="ts">
	import { getApiPath, getAppPath } from '$lib/base-path';
	import type { Lobby } from '$lib/types/lobby';
	import { onMount } from 'svelte';
	import { writable } from 'svelte/store';
	import { base } from '$app/paths';

	let lobbies = writable<Lobby[]>([]);

	onMount(async () => {
		const res = await fetch(`${getApiPath()}/matchmaking/lobbies`);
		const data = (await res.json()) as Lobby[];
		lobbies.set(data);
	});
</script>

<div class="w-full p-8">
	<h1 class="text-2xl">bot-battle <span class="text-sm">v0.0.0</span></h1>
	<div class="grid grid-cols-3 gap-2 my-4">
		{#each $lobbies as lobby}
			<a href={`${base}/${lobby.name}`} class=" rounded-md">
				<div
					class=" text-lg bg-slate-200 px-4 py-2 border-t border-l border-r rounded-t-md border-gray-600"
				>
					{lobby.name}
				</div>
				<div
					class="flex flex-col justify-center text-sm h-32 p-2 border-b border-l border-r rounded-b-md border-gray-600"
				></div>
			</a>
		{/each}
	</div>
</div>
