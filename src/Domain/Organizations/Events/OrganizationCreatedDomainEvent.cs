using Domain.Abstractions;

namespace Domain.Organizations.Events;
public sealed record OrganizationCreatedDomainEvent(OrganizationId Id) : IDomainEvent;