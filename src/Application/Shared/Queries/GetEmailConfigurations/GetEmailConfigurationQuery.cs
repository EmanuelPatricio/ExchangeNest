using Application.Abstractions.Messaging;
using Domain.Configurations;

namespace Application.Shared.Queries.GetEmailConfigurations;
public sealed record GetEmailConfigurationQuery : IQuery<Configuration>;