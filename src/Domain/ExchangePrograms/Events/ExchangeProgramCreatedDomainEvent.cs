using Domain.Abstractions;

namespace Domain.ExchangePrograms.Events;
public record ExchangeProgramCreatedDomainEvent(ExchangeProgramId Id) : IDomainEvent;