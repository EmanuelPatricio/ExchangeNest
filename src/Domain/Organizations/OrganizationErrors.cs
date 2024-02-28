using Domain.Abstractions;

namespace Domain.Organizations;
public static class OrganizationErrors
{
    public static Error NotFound(int id) => new(
        "User.NotFound",
        $"The user with the identifier {id} was not found");

    public static readonly Error EmailNotUnique = new(
        "User.EmailNotUnique",
        "The provided email it's already in use");
}
