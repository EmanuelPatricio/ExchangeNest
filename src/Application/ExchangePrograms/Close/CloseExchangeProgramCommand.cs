using Application.Abstractions.Messaging;

namespace Application.ExchangePrograms.Close;
public sealed record CloseExchangeProgramCommand(int Id) : ICommand;