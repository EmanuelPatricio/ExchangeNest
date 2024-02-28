namespace WebApi.Endpoints.Applications;

public sealed record CancelApplicationRequest(int Id, string Reason);