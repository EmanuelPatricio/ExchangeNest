using Domain.Abstractions;
using Domain.Shared;
using Domain.Users.ValueObjects;

namespace Domain.Users;
public interface IUserRepository
{
    void Create(User user);
    Task<User?> LoginAsync(Email email, UserPassword password, CancellationToken cancellationToken);
    Task<bool> IsEmailUniqueAsync(Email email, UserId? userId = null);
    Task<Result> ChangePasswordAsync(Email email, UserPassword newPassword);
    Task<User?> GetById(UserId id);
    Task<User?> GetByEmail(Email email);
    bool DoesDatabaseHasChanges();
    Task<List<User>> GetAll();
}
