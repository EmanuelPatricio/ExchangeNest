using Domain.ExchangePrograms;
using Domain.ExchangePrograms.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class ExchangeProgramConfiguration : IEntityTypeConfiguration<ExchangeProgram>
{
    public void Configure(EntityTypeBuilder<ExchangeProgram> builder)
    {
        builder.ToTable("ExchangePrograms");

        builder.HasKey(exchangeProgram => exchangeProgram.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new ExchangeProgramId(g)
        );

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramName(value)
            );

        builder.Property(x => x.Description)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramDescription(value)
            );

        builder.Property(x => x.LimitApplicationDate)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramLimitApplicationDate(value)
            ).IsRequired();

        builder.Property(x => x.StartDate)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramStartDate(value)
            ).IsRequired();

        builder.Property(x => x.FinishDate)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramFinishDate(value)
            ).IsRequired();

        builder.Property(x => x.ApplicationDocuments)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramApplicationDocuments(value)
            );

        builder.Property(x => x.RequiredDocuments)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramRequiredDocuments(value)
            );

        builder.Property(x => x.Images)
            .HasConversion(
                c => c.Value,
                value => new ExchangeProgramImages(value)
            );

        builder.Property(x => x.OrganizationId).IsRequired();

        builder.Property(x => x.CountryId).IsRequired();

        builder.Property(x => x.StateId).IsRequired();

        builder.Property(x => x.StatusId).IsRequired();

        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(x => x.LastModifiedOn);
    }
}
