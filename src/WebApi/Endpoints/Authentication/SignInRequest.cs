namespace WebApi.Endpoints.Authentication;

public sealed record SignInRequest(string Email, string Password);