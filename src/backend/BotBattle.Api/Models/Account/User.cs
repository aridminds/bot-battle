namespace BotBattle.Api.Models.Account;

public class User
{
    internal User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}