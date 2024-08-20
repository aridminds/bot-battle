import type { Container, TickerCallback } from "pixi.js";

export interface IPixiComponent {
    Preload(): Promise<void>;
    Setup(stage: Container): void;
    IsDynamic(): boolean;
    GetUpdateCallback(): [TickerCallback<any>, any] | void;
}