using MGH.Core.Infrastructure.Securities.Identity.Entities;

namespace MGH.Core.Infrastructure.Securities.Identity.Abstract;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetByIdAsync(int userId);
    Task InsertUserRefreshToken(UserRefreshToken userRefreshToken);
    UserRefreshToken GetUserRefreshTokenByUserAndOldToken(User user, string token, string refreshToken);
    Task InvalidateRefreshToken(string refreshToken);
}