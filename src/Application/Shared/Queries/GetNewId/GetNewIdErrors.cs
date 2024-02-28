using Domain.Abstractions;

namespace Application.Shared.Queries.GetNewId;
public static class GetNewIdErrors
{
    public static Error FieldOrTableNameNotDefined = new(
        "NewId.FieldOrTableNameNotDefined",
        "The field name or the table name were not given to perform operations");
}
