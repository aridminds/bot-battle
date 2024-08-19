<script lang="ts">
	import { Label } from 'bits-ui';

	export let onRegister: () => void;
	export let onSwitchToLogin: () => void;
</script>

<div class="flex flex-col h-full justify-between gap-4 p-6">
	<h1 class="text-2xl">Register</h1>
	<div class="flex flex-col h-full gap-4">
		<div>
			<Label.Root for="username" class="text-sm font-medium">Username</Label.Root>
			<div class="relative w-full">
				<input
					id="username"
					class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
					placeholder="Username"
				/>
			</div>
		</div>
		<div>
			<Label.Root for="password" class="text-sm font-medium">Password</Label.Root>
			<div class="relative w-full">
				<input
					id="password"
					class="rounded-md border w-full border-zinc-900 h-min px-2 py-1"
					placeholder="Password"
					type="password"
				/>
			</div>
		</div>
		<div>
			<!-- register -->
			<button class="text-sm text-blue-500" on:click={onSwitchToLogin}>
				Already have an account? Login here!
			</button>
		</div>
		<span class="flex flex-grow"></span>
		<button class="rounded-md border border-zinc-900 h-min px-2 py-1" on:click={onRegister}
			>Register</button
		>
	</div>
</div>
