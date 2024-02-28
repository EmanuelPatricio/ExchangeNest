using Domain.Shared;
using Domain.UnitTests.Infrastructure;
using Domain.Users;
using Domain.Users.Events;
using Domain.Users.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.Users;
public class UserBaseTests : BaseTest
{
    [Fact]
    public void CreateShouldRaiseUseCreatedDomainEvent()
    {
        // Arrange
        var id = new UserId(10);
        var firstName = new UserFirstName("Juan");
        var lastName = new UserLastName("Perez");
        var nic = new UserNIC("12345678901");
        var email = Email.Create("juan.perez@ejemplo.com");
        var password = new UserPassword("contraseña_prueba");
        var birthDate = new UserBirthDate(new DateTime(2000, 5, 23));
        UserImageUrl? imageUrl = null;
        var roleId = ((int)Domain.Shared.Enums.Roles.Student);
        var statusId = 1;
        var organizationId = 1;
        var countryId = 1;

        // Act
        var user = User.Create(id, firstName, lastName, nic, email.Value, password, birthDate, imageUrl, roleId, statusId, organizationId, countryId);

        // Assert
        var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        userCreatedDomainEvent.Id.Should().Be(user.Id);
    }
    [Fact]
    public void CreateUserWithInvalidEmailShouldRaiseError()
    {
        // Arrange
        var id = new UserId(10);
        var firstName = new UserFirstName("Juan");
        var lastName = new UserLastName("Perez");
        var nic = new UserNIC("12345678901");
        var email = Email.Create("juan.perez");
        var password = new UserPassword("contraseña_prueba");
        var birthDate = new UserBirthDate(new DateTime(2000, 5, 23));
        var roleId = ((int)Domain.Shared.Enums.Roles.Student);
        var statusId = 1;
        var organizationId = 1;
        var countryId = 1;

        // Act

        // Assert
        email.IsFailure.Should().BeTrue();
        email.Error.Should().Be(Email.InvalidFormat);
        Assert.Throws<InvalidOperationException>(() => email.Value);
    }
}
