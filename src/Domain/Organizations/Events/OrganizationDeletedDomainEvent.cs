using Domain.Abstractions;

namespace Domain.Organizations.Events;
public sealed record OrganizationDeletedDomainEvent(OrganizationId Id) : IDomainEvent;
