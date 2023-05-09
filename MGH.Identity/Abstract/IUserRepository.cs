using MGH.Identity.Entities;

namespace MGH.Identity.Abstract;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetByIdAsync(int userId);
    Task InsertUserRefreshToken(UserRefreshToken userRefreshToken);
    UserRefreshToken GetUserRefreshTokenByUserAndOldToken(User user, string token, string refreshToken);
    Task InvalidateRefreshToken(string refreshToken);
}