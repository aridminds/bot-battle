using BotBattle.Core;
using BotBattle.Core.Enums;
using BotBattle.Engine.Models;

namespace BotBattle.Engine.Services;

public static class AirTrafficController
{
    public static void StartAirplane(BoardState boardState, Position droppingGiftPosition, CollectibleItemType droppingGiftType)
    {
        boardState.Airplane.IsFlying = true;
        boardState.Airplane.DroppingGiftPosition = droppingGiftPosition;
        boardState.Airplane.DroppingGiftType = droppingGiftType;
        boardState.Airplane.Position = new Position(0, droppingGiftPosition.Y, Direction.East);
        boardState.EventLogs.Add(EventLogExtensions.CreatePlainHasStartedEventLog(boardState.Turns));
    }
    
    public static void MoveAirplane(BoardState boardState)
    {
        if (boardState.Airplane.ParachuteStatus == ParachuteStatus.Delivered)
        {
            var collectibleItem = Giftor.GetPossibleGiftType(boardState);
            if (collectibleItem == null) return;
            StartAirplane(boardState, collectibleItem.Position, collectibleItem.Type);
            boardState.Airplane.ParachuteStatus = ParachuteStatus.OnThePlane;
            return;
        }
        
        switch (boardState.Airplane.ParachuteStatus)
        {
            case ParachuteStatus.InAirHigh:
                boardState.Airplane.ParachuteStatus = ParachuteStatus.InAirMiddle;
                break;
            case ParachuteStatus.InAirMiddle:
                boardState.Airplane.ParachuteStatus = ParachuteStatus.OnGround;
                break;
            case ParachuteStatus.OnGround:
                boardState.Airplane.ParachuteStatus = ParachuteStatus.PackedTogether;
                break;
            case ParachuteStatus.PackedTogether:
                boardState.Airplane.ParachuteStatus = ParachuteStatus.Delivered;
                boardState.CollectibleItems.Add(new CollectibleItem()
                {
                    Id = Guid.NewGuid(),
                    Position = boardState.Airplane.DroppingGiftPosition,
                    Type = boardState.Airplane.DroppingGiftType
                });
                
                boardState.EventLogs.Add(new EventLog{Turn = boardState.Turns, Message = $"Gift has been delivered at {boardState.Airplane.DroppingGiftPosition.X}, {boardState.Airplane.DroppingGiftPosition.Y}"});
                
                break;
        }
        
        if (boardState.Airplane.Position.Equals(boardState.Airplane.DroppingGiftPosition))
        {
            boardState.Airplane.ParachuteStatus = ParachuteStatus.InAirHigh;
        }
        
        if(boardState.Airplane.Position.X > boardState.Width)
        {
            boardState.Airplane.IsFlying = false;
        }
        else
        {
            boardState.Airplane.Position = new Position(boardState.Airplane.Position.X + 1, boardState.Airplane.Position.Y, Direction.East);
        }
    }
}