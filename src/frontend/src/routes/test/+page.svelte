<script>
	import PixiGame from '$lib/components/pixi-game.svelte';
	import { Direction } from '$lib/types/direction';
	import { TankStatus } from '$lib/types/tankStatus';
</script>

<PixiGame
	height={600}
	width={800}
	arenaWidth={2}
	arenaHeight={2}
	tileSize={200}
	mapTiles={[13, 14, 15, 16]}
	tanks={[
		{
			Health: 100,
			Name: 'Thomas',
			Status: TankStatus.Alive,
			Position: { X: 1, Y: 1, Direction: Direction.NorthEast }
		}
	]}
	bullets={[
		{
			Id: 'adfs',
			CurrentPosition: { X: 1, Y: 1, Direction: Direction.West }
		}
	]}
></PixiGame>
