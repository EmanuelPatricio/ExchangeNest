using Domain.Abstractions;

namespace Domain.Organizations.Events;
public sealed record OrganizationUpdatedDomainEvent(OrganizationId Id) : IDomainEvent;