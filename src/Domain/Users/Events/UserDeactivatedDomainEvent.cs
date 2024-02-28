using Domain.Abstractions;

namespace Domain.Users.Events;

public record UserDeactivatedDomainEvent(UserId Id) : IDomainEvent;