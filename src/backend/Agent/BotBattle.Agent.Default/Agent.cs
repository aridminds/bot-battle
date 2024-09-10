using System.Globalization;
using BotBattle.AgentLib;
using BotBattle.AgentLib.Enums;
using Action = BotBattle.AgentLib.Action;

namespace BotBattle.Agent.Default;

public class Agent : IAgent
{
    private readonly Stack<Action> _actions = new();

    public AgentResponse CalculateAction(AgentRequest request)
    {
        if (_actions.Count > 0)
        {
            return new AgentResponse
            {
                Action = _actions.Pop()
            };
        }

        var myTank = request.MyTank;

        var payloadHashString = BitConverter.ToString(request.Hash).Replace("-", "");

        var direction = payloadHashString[..2];
        var shootingRange = payloadHashString[2..4];
        var actionDecision = payloadHashString[4..5];

        var rawShootingRange = int.Parse(shootingRange, NumberStyles.HexNumber);

        if (actionDecision[0] % 2 == 0 && (Random.Shared.NextDouble() < 0.1d || myTank.WeaponSystem.CanShoot))
            _actions.Push(new Shoot { Power = rawShootingRange, Weapon = myTank.WeaponSystem.Id });

        _actions.Push(new Drive());

        var rotateAction = new Rotate
        {
            Direction = MapToDirection(int.Parse(direction, NumberStyles.HexNumber))
        };

        return new AgentResponse
        {
            Action = rotateAction
        };
    }

    private static Direction MapToDirection(int value)
    {
        return value switch
        {
            < 32 => Direction.North,
            < 64 => Direction.NorthEast,
            < 96 => Direction.East,
            < 128 => Direction.SouthEast,
            < 160 => Direction.South,
            < 192 => Direction.SouthWest,
            < 224 => Direction.West,
            _ => Direction.NorthWest
        };
    }
}