using System.Security.Cryptography;
using BotBattle.Engine;
using BotBattle.Engine.Helper;
using BotBattle.Engine.Models;
using BotBattle.Engine.Models.States;
using BotBattle.Engine.Services;
using Tank = BotBattle.Engine.Models.Tank;

namespace BotBattle.LobbyServer;

public class Lobby
{
    private readonly GameMaster _gameMaster = new();
    private Tank _currentTank;
    private int _roundDuration;

    public Lobby(string[] playerNames, int arenaWidth, int arenaHeight, int roundDuration)
    {
        BoardState = new BoardState(arenaWidth, arenaHeight);

        foreach (var playerName in playerNames)
            BoardState.Tanks.Add(new Tank { Name = playerName, WeaponSystem = new WeaponSystem { FireCooldown = 2 } });

        foreach (var tank in BoardState.Tanks)
        {
            tank.Position = StartPositionService.SetStartPosition(BoardState);
        }

        for (var i = 0; i < (arenaWidth * arenaHeight * 0.05d); i++)
        {
            BoardState.Obstacles.Add(new Obstacle
            {
                Position = StartPositionService.SetStartPosition(BoardState),
                Type = EnumHelper.GetRandomEnumValue<ObstacleType>(ObstacleType.Destroyed, ObstacleType.OilStain)
            });
        }

        _currentTank = BoardState.Tanks.First();
        _roundDuration = roundDuration;
    }

    private BoardState BoardState { get; set; }

    public async Task Run(Action<BoardState> onNewBoardState, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (BoardState.Status == GameStatus.GameOver) return;

            if (_currentTank.Status != TankStatus.Dead)
            {
                var data = await CallDataSource();
                var payloadHash = MD5.HashData(BitConverter.GetBytes(data.Item1 * data.Item2));
                var payloadHashString = BitConverter.ToString(payloadHash).Replace("-", "");

                GameMaster.NextRound(payloadHashString, BoardState, _currentTank);
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

    private async Task<(long, long)> CallDataSource()
    {
        await Task.Delay(_roundDuration);
        return (new Random().Next(0, 5000), new Random().Next(0, 5000));
    }
}