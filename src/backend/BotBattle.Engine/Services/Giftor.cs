using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Helper;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class Giftor
{
    public static CollectibleItem? GetPossibleGiftType(BoardState boardState)
    {
        if (Random.Shared.NextDouble() < .5d)
        {
            var position = StartPositionService.SetStartPosition(boardState);
            if (position == null) return null;
            return new CollectibleItem()
            {
                Id = Guid.NewGuid(),
                Position = position,
                Type = EnumHelper.GetRandomEnumValue<CollectibleItemType>()
            };
        }
        
        return null;
    }
    
    public static void CheckForGift(BoardState boardState)
    {
        var collectedItems = new List<CollectibleItem>();
        foreach (var tank in boardState.Tanks)
        {
            foreach (var item in boardState.CollectibleItems.Where(item => tank.Position.Equals(item.Position)))
            {
                tank.Inventory.Add(item.Type);
                collectedItems.Add(item);
                boardState.EventLogs.Add(EventLogExtensions.CreateHasHasCollectedEventLog(boardState.Turns, tank, item.Type));
            }
        }
        
        foreach (var item in collectedItems)
        {
            boardState.CollectibleItems.Remove(item);
        }
    }
}