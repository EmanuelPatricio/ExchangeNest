using Domain.Abstractions;

namespace Domain.Applications;
public class ApplicationErrors
{
    public static Error NotFound(int id) => new(
        "Application.NotFound",
        $"The application with the identifier {id} was not found");
    public static Error DocumentNotFound(int id) => new(
        "ApplicationDocument.NotFound",
        $"The application document with the identifier {id} was not found");
}
