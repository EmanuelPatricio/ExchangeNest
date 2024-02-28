namespace Application.Abstractions.Email;
public record EmailRequest(string To, string Subject, string Message);
