import { Assets, Container, Sprite, Ticker, type TickerCallback } from "pixi.js";
import type { IPixiComponent } from "./IPixiComponent";
import { BulletStatus, type Bullet } from "$lib/types/bullet";
import { calculateRotationDegrees } from "$lib/types/tank";

class BulletNode {
    public readonly id: string;
    private sprite: Sprite | null = null;
    private lastBulletInfo: Bullet;
    private rotationOffset: number;

    constructor(bullet: Bullet, rotationOffset: number = 0) {
        this.id = bullet.Id;
        this.lastBulletInfo = bullet;
        this.rotationOffset = rotationOffset;
    }

    Init(asset: string): void {
        if (this.sprite === null) {
            this.sprite = new Sprite(Assets.get(asset))
            this.sprite.anchor.set(0.5);
        }
        else
            this.sprite.texture = Assets.get(asset);
        this.sprite.scale.set(0.25);
        this.sprite.angle = calculateRotationDegrees(this.lastBulletInfo.CurrentPosition, this.rotationOffset);
        this.sprite.x = this.lastBulletInfo.CurrentPosition.X;
        this.sprite.y = this.lastBulletInfo.CurrentPosition.Y;
    }

    Setup(parent: Container): void {
        parent.addChild(this.sprite!);
    }

    UpdateBullet(bulletInfo: Bullet): void {
        if (this.sprite === null) {
            this.lastBulletInfo = bulletInfo;
            return;
        }

        // TODO: Boom on start and hit
        // TODO: animate
        if (bulletInfo.CurrentPosition.X !== this.lastBulletInfo.CurrentPosition.X || bulletInfo.CurrentPosition.Y !== this.lastBulletInfo.CurrentPosition.Y)
            this.sprite!.position = { x: bulletInfo.CurrentPosition.X, y: bulletInfo.CurrentPosition.Y };

        if (bulletInfo.CurrentPosition.Direction !== this.lastBulletInfo.CurrentPosition.Direction)
            this.sprite!.angle = calculateRotationDegrees(bulletInfo.CurrentPosition, this.rotationOffset);

        if (bulletInfo.Status === BulletStatus.Hit)
            this.Destroy();

        this.lastBulletInfo = bulletInfo;
    }

    UpdateAnimation(delta: number) {
        // TODO: animation/tweening
    }

    Destroy(): void {
        this.sprite?.removeFromParent();
        this.sprite?.destroy();
    }
}

function Animate(this: BulletNode[], ticker: Ticker) {
    this.forEach(t => t.UpdateAnimation(ticker.deltaTime));
}

export class BulletHandler implements IPixiComponent {
    private container: Container = new Container;
    private bulletNodes: BulletNode[] = [];

    async Preload(): Promise<void> {
        await Assets.load({
            alias: "bullet",
            src: "/botbattle/cannon_ball.svg"
        });
        this.bulletNodes.forEach(b => {
            b.Init("bullet");
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
        return [Animate, this.bulletNodes];
    }

    UpdateBullets(tanks: Bullet[]) {
        tanks.forEach(t => {
            let found = this.bulletNodes.find(f => f.id === t.Id);
            if (!found) {
                found = new BulletNode(t);
                this.bulletNodes.push(found);
                if (Assets.get("bullet") === undefined) return;
                found.Init("bullet");
                found.Setup(this.container);
            }
            else
                found.UpdateBullet(t);
        })
    }
}