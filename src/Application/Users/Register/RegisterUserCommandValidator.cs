using Application.Users.Create;
using FluentValidation;

namespace Application.Users.Register;
internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();

        RuleFor(c => c.LastName).NotEmpty();

        RuleFor(c => c.Nic).NotEmpty().Length(11);

        RuleFor(c => c.Email).EmailAddress();

        RuleFor(c => c.Password).NotEmpty().MinimumLength(8);

        RuleFor(c => c.BirthDate).NotEmpty();
    }
}