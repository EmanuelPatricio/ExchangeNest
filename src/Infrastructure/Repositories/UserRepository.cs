using Application.Users.Shared;
using Domain.Abstractions;
using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, UserId? userId = null)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is not null && userId is not null)
        {
            return user.Id == userId;
        }

        return user is null;
    }

    public void Create(User user)
    {
        Add(user);
    }

    public async Task<List<User>> GetAll()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<User?> GetById(UserId id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<User?> GetByEmail(Email email)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> LoginAsync(Email email, UserPassword password, CancellationToken cancellationToken)
    {
        var encodedPassword = EncodePassword.EncodeToBase64(password.Value);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == new UserPassword(encodedPassword), cancellationToken);

        return user;
    }

    public async Task<Result> ChangePasswordAsync(Email email, UserPassword newPassword)
    {
        var user = await GetByEmail(email);

        if (user is null)
        {
            return UserErrors.NotFoundEmail;
        }

        User.ChangePassword(user, new UserPassword(EncodePassword.EncodeToBase64(newPassword.Value)));

        await _db.SaveChangesAsync();

        return Result.Success();
    }

    public bool DoesDatabaseHasChanges()
    {
        return HasChanges();
    }
}