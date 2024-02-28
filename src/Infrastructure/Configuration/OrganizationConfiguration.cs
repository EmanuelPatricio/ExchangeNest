using Domain.Organizations;
using Domain.Organizations.ValueObjects;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(organization => organization.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new OrganizationId(g)
        );

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new OrganizationName(value)
            );

        builder.Property(x => x.Description)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new OrganizationDescription(value)
            );

        builder.Property(x => x.Email)
            .HasMaxLength(400)
            .HasConversion(
                c => c.Value,
                value => Email.Create(value).Value
            );

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new OrganizationPhoneNumber(value)
            );

        builder.Property(x => x.Address)
            .HasConversion(
                c => c.Value,
                value => new OrganizationAddress(value)
            );

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasConversion(
                c => c!.Value,
                value => new OrganizationImageUrl(value)
            );

        builder.Property(x => x.OrganizationTypeId).IsRequired();

        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(x => x.LastModifiedOn);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
