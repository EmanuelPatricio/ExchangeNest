using Domain.Countries;

namespace Domain.States;
public sealed class State
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
}