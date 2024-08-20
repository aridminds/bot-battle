import { Assets, Container, Sprite, Spritesheet, Texture, type SpritesheetData, type SpritesheetFrameData, type TickerCallback } from "pixi.js";
import type { IPixiComponent } from "./IPixiComponent";

const atlasRows = 4;
const atlasCols = 10;
const atlasTileSize = 64;

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

export class SurfaceHandler implements IPixiComponent {
    private tileSize: number;
    private mapTiles: number[];
    private arenaWidth: number;
    private sprites: Sprite[] = [];
    private spritesheetData: SpritesheetData;
    private sheet: Spritesheet | null = null;
    private container: Container = new Container;
    private textureAtlas: Texture | null = null;

    constructor(mapTiles: number[], tileSize: number, arenaWidth: number) {
        this.tileSize = tileSize;
        this.mapTiles = mapTiles;
        this.arenaWidth = arenaWidth;
        this.spritesheetData = GenerateSheetData();
    }

    async Preload(): Promise<void> {
        this.textureAtlas = await Assets.load('/botbattle/terrainTiles_default.png');
        this.textureAtlas!.source.scaleMode = "nearest";
        this.sheet = new Spritesheet(this.textureAtlas!, this.spritesheetData);
        await this.sheet!.parse();
    }

    Setup(stage: Container): void {
        const sheet = this.sheet!;

        this.mapTiles.forEach((e, i) => {
            const x = i % this.arenaWidth;
            const y = Math.floor(i / this.arenaWidth);

            const sprite = new Sprite(sheet.textures[e - 1]);
            sprite.width = this.tileSize;
            sprite.height = this.tileSize;
            sprite.x = x * this.tileSize;
            sprite.y = y * this.tileSize

            this.sprites.push(sprite);
            this.container.addChild(sprite);
        });
        stage.addChild(this.container);
    }

    IsDynamic(): boolean {
        return false;
    }

    GetUpdateCallback(): [TickerCallback<any>, any] | void {
    }

    UpdateTiles(tileSize: number) {
        this.tileSize = tileSize;
        this.sprites!.forEach((s, i) => {
            const x = i % this.arenaWidth;
            const y = Math.floor(i / this.arenaWidth);

            s.width = this.tileSize;
            s.height = this.tileSize;
            s.x = x * this.tileSize;
            s.y = y * this.tileSize
        })
    }
}