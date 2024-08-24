using BotBattle.Api.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace BotBattle.Api;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}