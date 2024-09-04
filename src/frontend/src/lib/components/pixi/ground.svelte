<script lang="ts">
	import {
		Assets,
		Sprite,
		Spritesheet,
		Texture,
		type SpritesheetData,
		type SpritesheetFrameData
	} from 'pixi.js';
	import { getOrCreateLayer } from './pixi.svelte';
	import { onDestroy } from 'svelte';

	const atlasRows = 4;
	const atlasCols = 10;
	const atlasTileSize = 64;
	const sheetData = GenerateSheetData();
	const assetId = 'ground';

	export let mapTiles: number[] = [];
	export let tileSize: number;
	export let tileRows: number;

	const texture = Assets.get<Texture>(assetId);
	const sheet = new Spritesheet(texture, sheetData);
	const loadProm = sheet.parse();
	const sprites: Sprite[] = [];

	const layer = getOrCreateLayer(0);
	mapTiles.forEach((e, i) => {
		const x = i % tileRows;
		const y = Math.floor(i / tileRows);

		const sprite = new Sprite();
		sprite.width = tileSize;
		sprite.height = tileSize;
		sprite.x = x * tileSize;
		sprite.y = y * tileSize;

		sprites.push(sprite);
		layer.addChild(sprite);
	});

	loadProm.then(() => sprites.forEach((s, i) => (s.texture = sheet.textures[mapTiles[i] - 1])));

	function GenerateSheetData() {
		const data: SpritesheetData = { meta: { scale: 1 }, frames: {} };
		for (let i = 0; i < atlasRows; i++) {
			for (let j = 0; j < atlasCols; j++) {
				const frame: SpritesheetFrameData = {
					frame: { x: j * atlasTileSize, y: i * atlasTileSize, h: atlasTileSize, w: atlasTileSize },
					spriteSourceSize: { x: 0, y: 0, h: atlasTileSize, w: atlasTileSize },
					sourceSize: { h: atlasTileSize, w: atlasTileSize }
				};
				data.frames[i * atlasCols + j] = frame;
			}
		}
		return data;
	}

	onDestroy(() => {
		sprites.forEach((s) => {
			s.removeFromParent();
			s.destroy();
		});
		sprites.length = 0;
	});
</script>

<div>
	{#await loadProm then}
		<span>Maptiles: {mapTiles.length}</span>
		<slot />
	{/await}
</div>
