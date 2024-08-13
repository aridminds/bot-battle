namespace BotBattle.Api.Matchmaking;

public class MatchmakingHostedService : BackgroundService
{
    private readonly Matchmaking _matchmaking;

    public MatchmakingHostedService(Matchmaking matchmaking)
    {
        _matchmaking = matchmaking;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _matchmaking.QueueNewLobbies(stoppingToken);
            await Task.Delay(2000, CancellationToken.None);
        }
    }
}