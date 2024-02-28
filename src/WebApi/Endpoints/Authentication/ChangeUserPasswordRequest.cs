namespace WebApi.Endpoints.Authentication;

public sealed record ChangeUserPasswordRequest(string Token, string NewPassword);