using Domain.Abstractions;

namespace Application.Abstractions.Authentication;
public interface IJwtService
{
    Result<string> GetAccessToken(int userId, int roleId);
}