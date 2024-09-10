<script lang="ts">
	import { getApiPath } from '$lib/base-path';
	import AuthGuard from '$lib/components/auth-guard.svelte';

	function onUpload() {
		const file = document.getElementById('file') as HTMLInputElement;

		if (!file?.files?.length || !file?.files[0]) {
			return;
		}

		const formData = new FormData();
		formData.append('WasmFile', file?.files[0]);

		fetch(`${getApiPath()}/account/wasm`, {
			method: 'POST',
			body: formData,
			credentials: 'include'
		})
			.then((res) => res.json())
			.then((data) => {
				console.log(data);
			});
	}
</script>

<div class="w-full">
	<AuthGuard>
		<div slot="authorized">
			<form on:submit|preventDefault={onUpload} class="w-min">
				<p class="text-lg">Webassembly agent file:</p>
				<div
					class="flex flex-row border border-zinc-900 rounded-md px-2 py-1 justify-center items-center"
				>
					<input id="file" type="file" class="px-2 py-1" />
					<button class="border-l border-zinc-900 px-2 py-1" type="submit">Upload</button>
				</div>
			</form>
		</div>
	</AuthGuard>
</div>
