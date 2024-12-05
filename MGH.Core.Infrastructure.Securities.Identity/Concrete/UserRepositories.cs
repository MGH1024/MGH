using MGH.Core.Infrastructure.Securities.Identity.Abstract;
using MGH.Core.Infrastructure.Securities.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace MGH.Core.Infrastructure.Securities.Identity.Concrete;

public class UserRepository : IUserRepository
{
    private readonly AppIdentityDbContext _appIdentityDbContext;

    public UserRepository(AppIdentityDbContext appIdentityDbContext)
    {
        _appIdentityDbContext = appIdentityDbContext;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _appIdentityDbContext.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        return await _appIdentityDbContext
            .Users
            .Where(a => a.Id == userId)
            .Select(a => a).FirstAsync();
    }

    public UserRefreshToken GetUserRefreshTokenByUserAndOldToken(User user, string token, string refreshToken)
    {
        var result = _appIdentityDbContext.UserRefreshToken.Select(a => a)
            .Where(a => a.UserId == user.Id && !a.IsInvalidated && a.Token == token
                        && a.RefreshToken == refreshToken)
            .OrderByDescending(a => a.Id)
            .FirstOrDefault();
        return result;
    }

    public async Task InvalidateRefreshToken(string refreshToken)
    {
        var userRefreshToken = await _appIdentityDbContext.UserRefreshToken
            .Select(a => a)
            .Where(a => a.RefreshToken == refreshToken)
            .FirstAsync();

        userRefreshToken.IsInvalidated = true;
        _appIdentityDbContext.UserRefreshToken.Update(userRefreshToken);
        await _appIdentityDbContext.SaveChangesAsync();
    }

    public async Task InsertUserRefreshToken(UserRefreshToken userRefreshToken)
    {
        _appIdentityDbContext.UserRefreshToken.Add(userRefreshToken);
        await _appIdentityDbContext.SaveChangesAsync();
    }
}