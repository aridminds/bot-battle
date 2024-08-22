﻿using BotBattle.Brain;
using BotBattle.Brain.Models;
using BotBattle.Engine.Models.States;

namespace BotBattle.Engine.Models;

public class Tank
{
    public required string Name { get; init; }
    public Position Position { get; set; } = new(0, 0);
    public int Health { get; set; } = 1000;
    public TankStatus Status { get; set; } = TankStatus.Alive;
    public int PointRegister { get; set; } = 0;
    public int DiedInTurn { get; set; } = -1;
    public WeaponSystem WeaponSystem { get; set; }
}