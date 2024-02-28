using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstractions;

namespace Application.Shared.Queries.GetNewId;
internal sealed class GetNewIdQueryHandler : IQueryHandler<GetNewIdQuery, int>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetNewIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Result<int>> Handle(GetNewIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FieldName) || string.IsNullOrWhiteSpace(request.TableName))
        {
            return Result.Failure<int>(GetNewIdErrors.FieldOrTableNameNotDefined);
        }

        using var connection = _dbConnectionFactory.CreateOpenConnection();

        var query = $"SELECT ISNULL(MAX({request.FieldName}), 0) + 1 FROM {request.TableName}";

        return await connection.ExecuteScalarAsync<int>(query);
    }
}
