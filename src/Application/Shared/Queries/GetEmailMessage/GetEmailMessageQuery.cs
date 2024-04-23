using Application.Abstractions.Messaging;
using static Domain.Shared.Enums;

namespace Application.Shared.Queries.GetEmailMessage;
public sealed record GetEmailMessageQuery(EmailHtmlFile File) : IQuery<string>;