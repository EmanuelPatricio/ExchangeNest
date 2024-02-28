using Domain.Abstractions;

namespace Domain.ExchangePrograms.Events;
public record ExchangeProgramUpdatedDomainEvent(ExchangeProgramId Id) : IDomainEvent;