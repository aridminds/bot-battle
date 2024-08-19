namespace BotBattle.Api.Models.Account;

public class User
{
    internal User(string username, string passwordHash, byte[] salt)
    {
        Username = username;
        PasswordHash = passwordHash;
        Salt = salt;
    }

    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
}