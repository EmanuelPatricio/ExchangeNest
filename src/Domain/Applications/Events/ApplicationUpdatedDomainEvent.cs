using Domain.Abstractions;

namespace Domain.Applications.Events;
public record ApplicationUpdatedDomainEvent(ApplicationId Id) : IDomainEvent;