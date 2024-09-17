<script lang="ts">
	import { Assets, type UnresolvedAsset } from 'pixi.js';

	export let bundleId: string;
	export let assets: UnresolvedAsset[];

	async function init() {
		Assets.addBundle(bundleId, assets);
		await Assets.loadBundle(bundleId);
	}
</script>

<div class="hidden">
	{#await init()}
		<slot name="loading" />
	{:then}
		<slot />
	{/await}
</div>
