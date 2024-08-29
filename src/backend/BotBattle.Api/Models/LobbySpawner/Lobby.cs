using BotBattle.Core;

namespace BotBattle.Api.Models.LobbySpawner;

public class Lobby
{
    public Guid LobbyId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Player[] Players { get; set; }
    public int[] ArenaDimension { get; set; }
    public int[] MapTiles { get; set; }
    public event EventHandler<Lobby> LobbyFinished;

    public void OnLobbyFinished()
    {
        LobbyFinished?.Invoke(this, this);
    }
}