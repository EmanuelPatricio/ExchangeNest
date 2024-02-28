using Domain.Applications.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class ApplicationConfiguration : IEntityTypeConfiguration<Domain.Applications.Application>
{
    public void Configure(EntityTypeBuilder<Domain.Applications.Application> builder)
    {
        builder.ToTable("Applications");

        builder.HasKey(application => application.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new Domain.Applications.ApplicationId(g)
        );

        builder.Property(x => x.ProgramId).IsRequired();

        builder.Property(x => x.StudentId).IsRequired();

        builder.Property(x => x.Reason)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new ApplicationReason(value)
            )
            .IsRequired();

        builder.Property(x => x.StatusId).IsRequired();

        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(x => x.LastModifiedOn);

        builder
            .HasMany(e => e.Documents)
            .WithOne(e => e.Application)
            .HasForeignKey(e => e.ApplicationId)
            .IsRequired();
    }
}