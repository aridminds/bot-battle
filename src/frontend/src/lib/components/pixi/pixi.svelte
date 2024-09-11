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
	export let dynWidth: number;
	export let dynHeight: number;

	const app = new Application();
	setContext('pixi/stage', () => app.stage);
	setContext('pixi/ticker', () => app.ticker);

	async function initState(): Promise<void> {
		await app.init(options);
		app.stage.sortableChildren = true;
	}

	const initProm = initState();

	$: initProm.then(() => {
		app.renderer.resize(dynWidth, dynHeight);
	});
</script>

<div class="hidden">
	{#await initProm}
		<slot name="loading" />
	{:then}
		<slot />
	{/await}
</div>
