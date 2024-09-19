<script lang="ts">
	import { Sprite, Assets } from 'pixi.js';
	import { getOrCreateLayer } from './pixi.svelte';
	import type { CollectibleItem } from '$lib/types/collectibleItem';
	import { onDestroy } from 'svelte';

	const assetId = 'crate';
	export let collectibleItem: CollectibleItem;
	export let tileSize: number;

	const sprite = new Sprite(Assets.get(assetId));
	sprite.anchor.set(0.5);
	sprite.scale = 0.8;
	sprite.angle = Math.floor(Math.random() * 360);
	sprite.x = collectibleItem.Position.X * tileSize + tileSize / 2;
	sprite.y = collectibleItem.Position.Y * tileSize + tileSize / 2;

	getOrCreateLayer(1).addChild(sprite);

	onDestroy(() => {
		sprite.removeFromParent();
		sprite.destroy();
	});
</script>

<span>CollectibleItem!</span>
