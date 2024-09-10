<script lang="ts">
	import { getApiPath } from '$lib/base-path';
	import type { Lobby } from '$lib/types/lobby';
	import { onMount } from 'svelte';
	import { writable } from 'svelte/store';
	import { base } from '$app/paths';
	import { Dialog } from 'bits-ui';
	import { fade } from 'svelte/transition';
	import LobbyBuilder from '$lib/components/lobby-builder.svelte';
	import { userStore } from '$lib/stores/user-store';
	import AuthGuard from '$lib/components/auth-guard.svelte';

	let lobbies = writable<Lobby[]>([]);
	let createLobbyDialogOpen = false;

	onMount(() => {
		fetch(`${getApiPath()}/matchmaking/lobbies`)
			.then((res) => res.json() as Promise<Lobby[]>)
			.then((data) => lobbies.set(data));
	});

	function onCreateLobby() {
		createLobbyDialogOpen = false;

		fetch(`${getApiPath()}/matchmaking/lobbies`)
			.then((res) => res.json() as Promise<Lobby[]>)
			.then((data) => lobbies.set(data));
	}
</script>

<div class="w-full">
	<AuthGuard>
		<div slot="authorized">
			<button
				class="rounded-md border border-zinc-900 px-2 py-1"
				on:click={() => (createLobbyDialogOpen = true)}>New match</button
			>
			<Dialog.Root bind:open={createLobbyDialogOpen}>
				<Dialog.Portal>
					<Dialog.Overlay
						transition={fade}
						transitionConfig={{ duration: 150 }}
						class="fixed inset-0 z-50 bg-black/80"
					/>
					<Dialog.Content
						class="fixed left-[50%] top-[50%] max-w-[90%] max-h-[90%] z-50 w-full h-full translate-x-[-50%] translate-y-[-50%] bg-white shadow outline-none font-mono"
					>
						<LobbyBuilder {onCreateLobby} />
					</Dialog.Content>
				</Dialog.Portal>
			</Dialog.Root>
		</div>
	</AuthGuard>
	<div class="grid grid-cols-5 gap-2 my-4">
		{#each $lobbies as lobby}
			<a href={`${base}/${lobby.name}`} class=" rounded-md">
				<div class=" text-lg px-4 py-2 border-t border-l border-r rounded-t-md border-gray-600">
					{lobby.name}
				</div>
				<div
					class="flex flex-col justify-center text-sm h-32 p-2 border-b border-l border-r rounded-b-md border-gray-600"
				></div>
			</a>
		{/each}
	</div>
</div>
