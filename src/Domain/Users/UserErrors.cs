using Domain.Abstractions;

namespace Domain.Users;
public static class UserErrors
{
    public static Error NotFound(int id) => new(
        "User.NotFound",
        $"The user with the identifier {id} was not found");

    public static readonly Error NotFoundEmail = new(
        "User.NotFoundEmail",
        "The provided email doesn't have a user associated");

    public static readonly Error IncorrectPassword = new(
        "User.IncorrectPassword",
        "The password it's incorrect, please check and try again");

    public static readonly Error EmailNotUnique = new(
        "User.EmailNotUnique",
        "The provided email it's already in use");

    public static readonly Error JwtUnexpectedError = new(
        "User.JwtUnexpectedError",
        "The access token couldn't be created");
}
