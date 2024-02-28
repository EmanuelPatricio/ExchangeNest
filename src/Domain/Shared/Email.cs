using Domain.Abstractions;

namespace Domain.Shared;
public sealed record Email
{
    public static readonly Error Empty = new("Email.Empty", "Email is empty");
    public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email format is invalid");
    public static readonly Error NotFound = new("Email.NotFound", "Email not associated with any user, please verify and try again");
    public static readonly Error NotSended = new("EmailSender.NotSended", "An error prevented the email to be sended, please try again later.");

    private Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<Email>(Empty);
        }

        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(InvalidFormat);
        }

        return new Email(email);
    }
}
