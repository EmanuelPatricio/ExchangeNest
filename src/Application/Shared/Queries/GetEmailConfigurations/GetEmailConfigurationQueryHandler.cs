using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Shared;
using Dapper;
using Domain.Abstractions;
using Domain.Configurations;

namespace Application.Shared.Queries.GetEmailConfigurations;
internal sealed class GetEmailConfigurationQueryHandler : IQueryHandler<GetEmailConfigurationQuery, Configuration>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetEmailConfigurationQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<Configuration>> Handle(GetEmailConfigurationQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateOpenConnection();

        var query = "SELECT SenderMail, SenderPassword, EmailTemplate FROM Configurations";

        var emailConfigurations = await connection.QueryFirstOrDefaultAsync<Configuration>(query);

        if (emailConfigurations is null)
        {
            return Result.Failure<Configuration>(GetEmailConfigurationErrors.SettingsNotConfigured);
        }

        if (string.IsNullOrWhiteSpace(emailConfigurations.SenderMail))
        {
            return Result.Failure<Configuration>(GetEmailConfigurationErrors.SenderMailNotStablished);
        }

        if (string.IsNullOrWhiteSpace(emailConfigurations.SenderMail))
        {
            return Result.Failure<Configuration>(GetEmailConfigurationErrors.SenderPasswordNotStablished);
        }

        if (string.IsNullOrWhiteSpace(emailConfigurations.SenderMail))
        {
            return Result.Failure<Configuration>(GetEmailConfigurationErrors.EmailTemplateNotStablished);
        }

        Configuration.DecodePassword(emailConfigurations, EncodePassword.DecodeFrom64(emailConfigurations.SenderPassword));

        return emailConfigurations;
    }
}