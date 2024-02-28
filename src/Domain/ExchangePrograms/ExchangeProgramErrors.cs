using Domain.Abstractions;

namespace Domain.ExchangePrograms;
public class ExchangeProgramErrors
{
    public static Error NotFound(int id) => new(
        "ExchangeProgram.NotFound",
        $"The exchange program with the identifier {id} was not found");
}
