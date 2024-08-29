using System.Security.Cryptography;
using BotBattle.Engine;
using BotBattle.Engine.Helper;
using BotBattle.Engine.Models;
using BotBattle.Engine.Services;
using Tank = BotBattle.Core.Tank;

namespace BotBattle.LobbyServer;

public class Lobby
{
    private readonly GameMaster _gameMaster = new();
    private readonly WasmRunner _wasmRunner;
    private Tank _currentTank;
    private int _roundDuration;
    private BoardState BoardState { get; set; }

    public Lobby(Player[] players, int arenaWidth, int arenaHeight, int roundDuration)
    {
        BoardState = new BoardState(arenaWidth, arenaHeight);

        foreach (var player in players)
            BoardState.Tanks.Add(new Tank { Name = player.Name, WeaponSystem = new WeaponSystem { FireCooldown = 2 } });

        foreach (var tank in BoardState.Tanks)
        {
            tank.Position = StartPositionService.SetStartPosition(BoardState);
        }

        for (var i = 0; i < (arenaWidth * arenaHeight * 0.05d); i++)
        {
            BoardState.Obstacles.Add(new Obstacle
            {
                Position = StartPositionService.SetStartPosition(BoardState),
                Type = EnumHelper.GetRandomEnumValue(ObstacleType.Destroyed, ObstacleType.OilStain)
                UpdateTurn = Random.Shared.Next(5, 40)
            });
        }

        _currentTank = BoardState.Tanks.First();
        _roundDuration = roundDuration;
        _wasmRunner = new WasmRunner(players);
    }

    public async Task Run(Action<BoardState> onNewBoardState, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (BoardState.Status == GameStatus.GameOver) return;

            if (_currentTank.Status != TankStatus.Dead)
            {
                GameMaster.NextRound(BoardState, _currentTank, _wasmRunner);

                await Task.Delay(200, ct);

                onNewBoardState?.Invoke(BoardState);
            }

            NextPlayer();
        }
    }

    private void NextPlayer()
    {
        var currentPlayerIndex = BoardState.Tanks.IndexOf(_currentTank);

        var nextPlayerIndex = (currentPlayerIndex + 1) % BoardState.Tanks.Count;
        _currentTank = BoardState.Tanks[nextPlayerIndex];
    }
}