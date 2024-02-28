using Domain.Abstractions;

namespace Domain.Users.Events;

public class UserUpdatedDomainEvent(UserId Id) : IDomainEvent;