namespace WebApi.Endpoints.Applications;

public sealed record CloseApplicationRequest(int Id, string Reason);
