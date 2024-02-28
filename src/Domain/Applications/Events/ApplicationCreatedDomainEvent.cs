using Domain.Abstractions;

namespace Domain.Applications.Events;
public record ApplicationCreatedDomainEvent(ApplicationId Id) : IDomainEvent;