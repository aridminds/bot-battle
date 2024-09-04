<script lang="ts" context="module">
	import { getContext } from 'svelte';
	import { Container, Ticker } from 'pixi.js';

	export const getStage = () => getContext<() => Container>('pixi/stage')();
	export const getTicker = () => getContext<() => Ticker>('pixi/ticker')();
	export const getOrCreateLayer = (zIndex: number) => {
		const s = getStage();
		return s.children.find((c) => c.zIndex === zIndex) || s.addChild(new Container({ zIndex }));
	};
</script>

<script lang="ts">
	import { setContext } from 'svelte';
	import { Application, type ApplicationOptions } from 'pixi.js';

	export let options: Partial<ApplicationOptions>;

	const app = new Application();
	setContext('pixi/stage', () => app.stage);
	setContext('pixi/ticker', () => app.ticker);

	async function initState(): Promise<void> {
		await app.init(options);
		app.stage.sortableChildren = true;
	}
</script>

<div class="hidden">
	{#await initState()}
		<slot name="loading" />
	{:then}
		<slot />
	{/await}
</div>
