<script lang="ts">
	import { Dialog } from 'bits-ui';
	import '../../app.css';
	import { fade } from 'svelte/transition';
	import LoginForm from '$lib/components/login-form.svelte';
	import { getApiPath } from '$lib/base-path';
	import RegisterForm from '$lib/components/register-form.svelte';
	import { page } from '$app/stores';
	import { userStore } from '$lib/stores/user-store';

	let loginOrRegisterDialogOpen = false;
	let isRegister = false;

	function onLogin(username: string, password: string) {
		loginOrRegisterDialogOpen = false;

		let postData = {
			username: username,
			password: password,
			returnUrl: $page.url
		};

		fetch(`${getApiPath()}/account/login`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(postData)
		})
			.then((res) => {
				if (res.ok) {
					me();
				} else {
				}
			})
			.catch((err) => {});
	}

	function onRegister(username: string, password: string) {
		loginOrRegisterDialogOpen = false;

		let postData = {
			username: username,
			password: password,
			returnUrl: $page.url
		};

		fetch(`${getApiPath()}/account/register`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(postData)
		})
			.then((res) => {
				if (res.ok) {
					me();
				} else {
				}
			})
			.catch((err) => {});
	}

	function me() {
		fetch(`${getApiPath()}/account/me`, {
			method: 'GET',
			headers: {
				'Content-Type': 'application/json'
			},
			credentials: 'include'
		})
			.then((res) => {
				if (res.ok) {
					return res.json();
				} else {
				}
			})
			.then((data) => {
				userStore.set(data);
			})
			.catch((err) => {});
	}

	function logout() {
		fetch(`${getApiPath()}/account/logout`, {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			credentials: 'include'
		})
			.then((res) => {
				if (res.ok) {
					userStore.set(null);
				} else {
				}
			})
			.catch((err) => {});
	}
</script>

<div class="w-screen h-screen font-mono p-8">
	<div class="flex flex-row justify-between mb-8">
		<h1 class="text-2xl">bot-battle <span class="text-sm">v0.0.1</span></h1>
		{#if $userStore?.username}
			<div class="flex flex-row items-center gap-2">
				<p class="mr-4">Logged in as {$userStore.username}</p>
				<a href="/account" class="rounded-md border border-zinc-900 px-2 py-1">Account</a>
				<button class="rounded-md border border-zinc-900 px-2 py-1" on:click={logout}>Logout</button
				>
			</div>
		{:else}
			<button
				class="rounded-md border border-zinc-900 h-min px-2 py-1"
				on:click={() => (loginOrRegisterDialogOpen = true)}>Login</button
			>
		{/if}
	</div>
	<slot />
</div>
<Dialog.Root
	bind:open={loginOrRegisterDialogOpen}
	onOpenChange={() => {
		isRegister = false;
	}}
>
	<Dialog.Portal>
		<Dialog.Overlay
			transition={fade}
			transitionConfig={{ duration: 150 }}
			class="fixed inset-0 z-50 bg-black/80"
		/>
		<Dialog.Content
			class="fixed left-[50%] top-[50%] max-w-96 max-h-96 z-50 w-full h-full translate-x-[-50%] translate-y-[-50%] bg-white shadow outline-none font-mono"
		>
			{#if isRegister}
				<RegisterForm
					{onRegister}
					onSwitchToLogin={() => {
						isRegister = false;
					}}
				/>
			{:else}
				<LoginForm
					{onLogin}
					onSwitchToRegister={() => {
						isRegister = true;
					}}
				/>
			{/if}
		</Dialog.Content>
	</Dialog.Portal>
</Dialog.Root>
