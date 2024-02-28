using Domain.States;

namespace Domain.Countries;
public sealed class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<State> States { get; } = new List<State>();
}