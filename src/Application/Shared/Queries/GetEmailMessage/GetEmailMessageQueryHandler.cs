using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Shared.Queries.GetEmailConfigurations;
using Dapper;
using Domain.Abstractions;

namespace Application.Shared.Queries.GetEmailMessage;
internal sealed class GetEmailMessageQueryHandler : IQueryHandler<GetEmailMessageQuery, string>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetEmailMessageQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<string>> Handle(GetEmailMessageQuery request, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateOpenConnection();

        var query = $"SELECT {Domain.Shared.Enums.EmailHtmlFilePath.GetValueOrDefault(request.File)} FROM Configurations";

        var message = await connection.QueryFirstOrDefaultAsync<string>(query);

        if (message is null)
        {
            return Result.Failure<string>(GetEmailConfigurationErrors.SettingsNotConfigured);
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return Result.Failure<string>(GetEmailConfigurationErrors.SenderMailNotStablished);
        }

        return message;
    }
}