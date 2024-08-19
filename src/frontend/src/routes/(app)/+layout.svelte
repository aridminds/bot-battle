<script>
	import { Dialog } from 'bits-ui';
	import '../../app.css';
	import { fade } from 'svelte/transition';
	import LoginForm from '$lib/components/login-form.svelte';
	import { getApiPath } from '$lib/base-path';
	import RegisterForm from '$lib/components/register-form.svelte';

	let loginOrRegisterDialogOpen = false;
	let isRegister = false;

	function onLogin() {
		loginOrRegisterDialogOpen = false;
	}

	function onRegister() {
		loginOrRegisterDialogOpen = false;
	}
</script>

<div class="w-screen h-screen font-mono p-8">
	<div class="flex flex-row justify-between">
		<h1 class="text-2xl mb-8">bot-battle <span class="text-sm">v0.0.1</span></h1>
		<button
			class="rounded-md border border-zinc-900 h-min px-2 py-1"
			on:click={() => (loginOrRegisterDialogOpen = true)}>Login</button
		>
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
