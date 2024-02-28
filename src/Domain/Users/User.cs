using Domain.Abstractions;
using Domain.Shared;
using Domain.Users.Events;
using Domain.Users.ValueObjects;
using static Domain.Shared.Enums;

namespace Domain.Users;
public sealed class User : Entity<UserId>
{
    private User() { }

    private User(
        UserId id,
        UserFirstName firstName,
        UserLastName lastName,
        UserNIC nic,
        Email email,
        UserPassword password,
        UserBirthDate birthDate,
        UserImageUrl? imageUrl,
        int roleId,
        int statusId,
        int organizationId,
        int countryId,
        DateTime createdOn,
        DateTime? lastModifiedOn)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        NIC = nic;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        ImageUrl = imageUrl;
        RoleId = roleId;
        StatusId = statusId;
        OrganizationId = organizationId;
        CountryId = countryId;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public UserFirstName FirstName { get; private set; }
    public UserLastName LastName { get; private set; }
    public UserNIC NIC { get; private set; }
    public Email Email { get; private set; }
    public UserPassword Password { get; private set; }
    public UserBirthDate BirthDate { get; private set; }
    public UserImageUrl? ImageUrl { get; private set; }
    public int RoleId { get; private set; }
    public int StatusId { get; private set; }
    public int OrganizationId { get; private set; }
    public int CountryId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    public static User Create(
        UserId id,
        UserFirstName firstName,
        UserLastName lastName,
        UserNIC nic,
        Email email,
        UserPassword password,
        UserBirthDate birthDate,
        UserImageUrl? imageUrl,
        int roleId,
        int statusId,
        int organizationId,
        int countryId)
    {
        var user = new User(id, firstName, lastName, nic, email, password, birthDate, imageUrl, roleId, statusId, organizationId, countryId, DateTime.Now, null);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public static void Update(
        User user,
        UserFirstName firstName,
        UserLastName lastName,
        UserNIC nic,
        Email email,
        UserBirthDate birthDate,
        UserImageUrl? imageUrl,
        int statusId,
        int roleId,
        int countryId)
    {
        user.FirstName = firstName;
        user.LastName = lastName;
        user.NIC = nic;
        user.Email = email;
        user.BirthDate = birthDate;
        user.ImageUrl = imageUrl;
        user.StatusId = statusId;
        user.LastModifiedOn = DateTime.Now;
        user.RoleId = roleId;
        user.CountryId = countryId;

        user.RaiseDomainEvent(new UserUpdatedDomainEvent(user.Id));
    }

    public static void Deactivate(User user)
    {
        user.StatusId = (int)Statuses.Deleted;
        user.LastModifiedOn = DateTime.Now;

        user.RaiseDomainEvent(new UserDeactivatedDomainEvent(user.Id));
    }

    public static void ChangePassword(User user, UserPassword newPassword)
    {
        user.Password = newPassword;
        user.RaiseDomainEvent(new UserPasswordChangedDomainEvent(user.Id));
    }
}
