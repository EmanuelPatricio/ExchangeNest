using Domain.Abstractions;
using Domain.ExchangePrograms.Events;
using Domain.ExchangePrograms.ValueObjects;
using static Domain.Shared.Enums;

namespace Domain.ExchangePrograms;
public sealed class ExchangeProgram : Entity<ExchangeProgramId>
{
    private ExchangeProgram() { }

    private ExchangeProgram(
        ExchangeProgramId id,
        ExchangeProgramName name,
        ExchangeProgramDescription description,
        ExchangeProgramLimitApplicationDate limitApplicationDate,
        ExchangeProgramStartDate startDate,
        ExchangeProgramFinishDate finishDate,
        ExchangeProgramApplicationDocuments applicationDocuments,
        ExchangeProgramRequiredDocuments requiredDocuments,
        ExchangeProgramImages images,
        int organizationId,
        int countryId,
        int stateId,
        int statusId,
        DateTime createdOn,
        DateTime? lastModifiedOn)
        : base(id)
    {
        Name = name;
        Description = description;
        LimitApplicationDate = limitApplicationDate;
        StartDate = startDate;
        FinishDate = finishDate;
        ApplicationDocuments = applicationDocuments;
        RequiredDocuments = requiredDocuments;
        Images = images;
        OrganizationId = organizationId;
        CountryId = countryId;
        StateId = stateId;
        StatusId = statusId;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public ExchangeProgramName Name { get; private set; }
    public ExchangeProgramDescription Description { get; private set; }
    public ExchangeProgramLimitApplicationDate LimitApplicationDate { get; private set; }
    public ExchangeProgramStartDate StartDate { get; private set; }
    public ExchangeProgramFinishDate FinishDate { get; private set; }
    public ExchangeProgramApplicationDocuments ApplicationDocuments { get; private set; }
    public ExchangeProgramRequiredDocuments RequiredDocuments { get; private set; }
    public ExchangeProgramImages Images { get; private set; }
    public int OrganizationId { get; private set; }
    public int CountryId { get; private set; }
    public int StateId { get; private set; }
    public int StatusId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    public static ExchangeProgram Create(
        ExchangeProgramId id,
        ExchangeProgramName name,
        ExchangeProgramDescription description,
        ExchangeProgramLimitApplicationDate limitApplicationDate,
        ExchangeProgramStartDate startDate,
        ExchangeProgramFinishDate finishDate,
        ExchangeProgramApplicationDocuments applicationDocuments,
        ExchangeProgramRequiredDocuments requiredDocuments,
        ExchangeProgramImages images,
        int organizationId,
        int countryId,
        int stateId,
        int statusId)
    {
        var exchangeProgram = new ExchangeProgram(id, name, description, limitApplicationDate, startDate, finishDate, applicationDocuments, requiredDocuments, images, organizationId, countryId, stateId, statusId, DateTime.Now, null);

        exchangeProgram.RaiseDomainEvent(new ExchangeProgramCreatedDomainEvent(exchangeProgram.Id));

        return exchangeProgram;
    }

    public static void Update(
        ExchangeProgram exchangeProgram,
        ExchangeProgramName name,
        ExchangeProgramDescription description,
        ExchangeProgramLimitApplicationDate limitApplicationDate,
        ExchangeProgramStartDate startDate,
        ExchangeProgramFinishDate finishDate,
        ExchangeProgramApplicationDocuments applicationDocuments,
        ExchangeProgramRequiredDocuments requiredDocuments,
        ExchangeProgramImages images,
        int countryId,
        int stateId,
        int statusId)
    {
        exchangeProgram.Name = name;
        exchangeProgram.Description = description;
        exchangeProgram.LimitApplicationDate = limitApplicationDate;
        exchangeProgram.StartDate = startDate;
        exchangeProgram.FinishDate = finishDate;
        exchangeProgram.ApplicationDocuments = applicationDocuments;
        exchangeProgram.RequiredDocuments = requiredDocuments;
        exchangeProgram.Images = images;
        exchangeProgram.CountryId = countryId;
        exchangeProgram.StateId = stateId;
        exchangeProgram.StatusId = statusId;
        exchangeProgram.LastModifiedOn = DateTime.Now;

        exchangeProgram.RaiseDomainEvent(new ExchangeProgramUpdatedDomainEvent(exchangeProgram.Id));
    }

    public static void Close(ExchangeProgram exchangeProgram)
    {
        exchangeProgram.StatusId = (int)Statuses.Deleted;
        exchangeProgram.LastModifiedOn = DateTime.Now;

        exchangeProgram.RaiseDomainEvent(new ExchangeProgramUpdatedDomainEvent(exchangeProgram.Id));
    }
}
