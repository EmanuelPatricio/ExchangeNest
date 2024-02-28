namespace WebApi.Endpoints.Authentication;

public sealed record ForgotPasswordRequest(string Email, string Url);
