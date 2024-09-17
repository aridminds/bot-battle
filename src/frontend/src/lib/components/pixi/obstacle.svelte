<script lang="ts">
	import { type Obstacle, ObstacleType } from '$lib/types/obstacle';
	import { Sprite, Assets } from 'pixi.js';
	import { getOrCreateLayer } from './pixi.svelte';

	export let obstacle: Obstacle;
	export let tileSize: number;

	const sprite = new Sprite();
	sprite.anchor.set(0.5);
	sprite.angle = Math.floor(Math.random() * 360);
	sprite.x = obstacle.Position.X * tileSize + tileSize / 2;
	sprite.y = obstacle.Position.Y * tileSize + tileSize / 2;
	let currentType = obstacle.Type;

	setSprite(currentType);

	getOrCreateLayer(1).addChild(sprite);

	function setSprite(type: ObstacleType) {
		switch (type) {
			case ObstacleType.Stone:
				sprite.width = sprite.height = 1.25 * tileSize;
				sprite.texture = Assets.get('stone');
				break;
			case ObstacleType.TreeLarge:
				sprite.width = sprite.height = 1.5 * tileSize;
				sprite.texture = Assets.get('tree');
				break;
			case ObstacleType.TreeLeaf:
				sprite.width = sprite.height = 0.25 * tileSize;
				sprite.texture = Assets.get('leaf');
				break;
			case ObstacleType.TreeSmall:
				sprite.width = sprite.height = 1 * tileSize;
				sprite.texture = Assets.get('tree-small');
				break;
			case ObstacleType.OilBarrel:
				sprite.width = sprite.height = 0.6 * tileSize;
				sprite.texture = Assets.get('red-barrel');
				break;
			case ObstacleType.OilStain:
				sprite.width = sprite.height = 1.5 * tileSize;
				sprite.texture = Assets.get('oil-stain');
				break;
			default:
			case ObstacleType.Destroyed:
				sprite.width = sprite.height = 0.6 * tileSize;
				sprite.texture = Assets.get('tree-destroyed');
				break;
		}
	}

	$: if (currentType !== obstacle.Type) setSprite((currentType = obstacle.Type));
</script>

<span>Obstacle!</span>
