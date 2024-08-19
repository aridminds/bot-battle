// using BotBattle.Api.Models.Account;
// using Isopoh.Cryptography.Argon2;
//
// namespace BotBattle.Api.Services.Account;
//
// internal class UserManager
// {
//     private readonly UsersDbContext _context;
//
//     public UserManager(UsersDbContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<bool> CheckPasswordAsync(Guid id, string password)
//     {
//         var user = await FindByIdAsync(id);
//
//         if (user == null)
//             return false;
//
//         var passwordMatch = Argon2.Verify(user.PasswordHash, password);
//
//         return passwordMatch;
//     }
//
//     public async Task<bool> ChangePasswordAsync(Guid id, string oldPassword, string newPassword)
//     {
//         var user = await FindByIdAsync(id);
//
//         if (user == null) return false;
//
//         if (!Argon2.Verify(user.PasswordHash, oldPassword)) return false;
//
//         user.PasswordHash = Argon2.Hash(newPassword);
//
//         await _context.SaveChangesAsync();
//
//         return false;
//     }
//
//     public Task<User?> FindByUsernameAsync(string email)
//     {
//         var user = _context.Users
//             .FirstOrDefault(u => u.Email == email);
//
//         return Task.FromResult(user);
//     }
//
//     public Task<User?> FindByIdAsync(Guid id)
//     {
//         var user = _context.Users
//             .Find(id);
//
//         return Task.FromResult(user);
//     }
// }