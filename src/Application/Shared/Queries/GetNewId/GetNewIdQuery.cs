using Application.Abstractions.Messaging;

namespace Application.Shared.Queries.GetNewId;
public sealed record GetNewIdQuery(string FieldName, string TableName) : IQuery<int>;