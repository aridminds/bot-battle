import { Assets, Container, Sprite, Text, TextStyle, Ticker, type TickerCallback } from "pixi.js";
import type { IPixiComponent } from "./IPixiComponent";
import { calculateRotationDegrees, type Tank } from "$lib/types/tank";
import { TankStatus } from "$lib/types/tankStatus";

class TankNode {
    public readonly name: string;
    private tankContainer: Container = new Container({ label: "tank-container" });
    private label: Text;
    private sprite: Sprite | null = null;
    private lastTankInfo: Tank;
    private rotationOffset: number;

    constructor(tankInfo: Tank, rotationOffset: number = 0) {
        this.name = tankInfo.Name;
        this.lastTankInfo = tankInfo;
        this.label = new Text({
            text: `0\n${this.name}`,
            style: new TextStyle({ fill: 'black', fontSize: 14, fontFamily: "sans-serif", align: "center" }),
            y: -10,
            anchor: { x: 0.5, y: 1 }
        })
        this.rotationOffset = rotationOffset;
    }

    Init(asset: string): void {
        if (this.sprite === null) {
            this.sprite = new Sprite(Assets.get(asset))
            this.sprite.anchor.set(0.5);
        }
        else
            this.sprite.texture = Assets.get(asset);

        this.label.text = `${this.lastTankInfo.Health}\n${this.name}`
        this.tankContainer.position = { x: this.lastTankInfo.Position.X, y: this.lastTankInfo.Position.Y };
        this.sprite!.angle = calculateRotationDegrees(this.lastTankInfo.Position, this.rotationOffset);

    }

    Setup(parent: Container): void {
        this.label.y -= this.sprite!.height / 2;
        this.tankContainer.addChild(this.sprite!);
        this.tankContainer.addChild(this.label);

        parent.addChild(this.tankContainer);
    }

    UpdateTank(tankinfo: Tank): void {
        if (tankinfo.Health !== this.lastTankInfo?.Health)
            this.label.text = `${tankinfo.Health}\n${this.name}`

        if (tankinfo.Status !== this.lastTankInfo?.Status) {
            let color = 'black';

            switch (tankinfo.Status) {
                case TankStatus.Dead:
                    color = 'red';
                    break;
                case TankStatus.Winner:
                    color = 'green';
                    break;
                default:
                    color = 'black'; // Standardfarbe
                    break;
            }
            this.label.style.fill = color;
        }

        // TODO: animate
        if (tankinfo.Position.X !== this.lastTankInfo?.Position.X || tankinfo.Position.Y !== this.lastTankInfo.Position.Y)
            this.tankContainer.position = { x: tankinfo.Position.X, y: tankinfo.Position.Y };

        if (tankinfo.Position.Direction !== this.lastTankInfo?.Position.Direction)
            this.sprite!.angle = calculateRotationDegrees(tankinfo.Position, this.rotationOffset);

        this.lastTankInfo = tankinfo;
    }

    UpdateAnimation(delta: number) {
        // TODO: animation/tweening
    }

    Destroy(): void {
        this.tankContainer.removeFromParent();
        this.tankContainer.destroy();
    }
}

function Animate(this: TankNode[], ticker: Ticker) {
    this.forEach(t => t.UpdateAnimation(ticker.deltaTime));
}

export class TankHandler implements IPixiComponent {
    private container: Container = new Container;
    private readonly tankNodes: TankNode[] = [];

    async Preload(): Promise<void> {
        await Assets.load({
            alias: "tank",
            src: "/botbattle/tank_dark.svg"
        });
        this.tankNodes.forEach(b => {
            b.Init("tank");
            b.Setup(this.container);
        })
    }

    Setup(stage: Container): void {
        stage.addChild(this.container);
    }

    IsDynamic(): boolean {
        return true;
    }

    GetUpdateCallback(): [TickerCallback<any>, any] | void {
        return [Animate, this.tankNodes];
    }

    UpdateTanks(tanks: Tank[]) {
        tanks.forEach(t => {
            let found = this.tankNodes.find(f => f.name === t.Name);
            if (!found) {
                found = new TankNode(t, -90);
                this.tankNodes.push(found);
                if (Assets.get("tank") === undefined) return;
                found.Init("tank");
                found.Setup(this.container);
            }
            found.UpdateTank(t);
        })
    }
}