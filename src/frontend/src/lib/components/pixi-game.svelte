<script lang="ts">
	import { BulletHandler } from '$lib/game/BulletHandler';
	import type { IPixiComponent } from '$lib/game/IPixiComponent';
	import { SurfaceHandler } from '$lib/game/SurfaceHandler';
	import { TankHandler } from '$lib/game/TankHandler';
	import type { Bullet } from '$lib/types/bullet';
	import type { Tank } from '$lib/types/tank';
	import { Application, type TickerCallback } from 'pixi.js';
	import { onMount } from 'svelte';

	export let width: number = 800;
	export let height: number = 600;
	export let arenaWidth: number;
	export let arenaHeight: number;
	export let mapTiles: number[];
	export let tileSize: number;
	export let tanks: Tank[];
	export let bullets: Bullet[];

	let canvasContainer: HTMLCanvasElement;

	// TODO: Achtung: TankHandler braucht bereits transformaierte Tanks aka die Position muss schon auf Tilesize angepasst sein
	const sh = new SurfaceHandler(mapTiles, tileSize, arenaWidth);
	const th = new TankHandler();
	const bh = new BulletHandler();

	const renderer: IPixiComponent[] = [sh, th, bh];
	const app = new Application();
	globalThis.__PIXI_APP__ = app;

	onMount(async (): Promise<void> => {
		await app.init({
			backgroundColor: 'chocolate',
			canvas: canvasContainer,
			width,
			height
		});
		for (let i = 0; i < renderer.length; i++) {
			await renderer[i].Preload();
		}
		renderer.forEach((r) => r.Setup(app.stage));
		renderer
			.filter((r) => r.IsDynamic())
			.forEach((r) => {
				const params = r.GetUpdateCallback() as [TickerCallback<any>, any];
				app.ticker.add(params[0], params[1]);
			});
	});

	$: app.renderer?.resize(width, height);
	$: sh.UpdateTiles(tileSize);
	$: th.UpdateTanks(
		tanks.map((t) => {
			t.Position.X = t.Position.X * tileSize + tileSize / 2;
			t.Position.Y = t.Position.Y * tileSize + tileSize / 2;
			return t;
		})
	);
	$: bh.UpdateBullets(
		bullets.map((t) => {
			t.CurrentPosition.X = t.CurrentPosition.X * tileSize + tileSize / 2;
			t.CurrentPosition.Y = t.CurrentPosition.Y * tileSize + tileSize / 2;
			return t;
		})
	);
</script>

<canvas bind:this={canvasContainer} />
