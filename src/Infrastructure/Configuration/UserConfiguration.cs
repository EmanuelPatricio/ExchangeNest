using Domain.Shared;
using Domain.Users;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new UserId(g)
        );

        builder.Property(x => x.FirstName)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new UserFirstName(value)
            );

        builder.Property(x => x.LastName)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new UserLastName(value)
            );

        builder.Property(x => x.NIC)
            .HasConversion(
                c => c.Value,
                value => new UserNIC(value)
            );

        builder.Property(x => x.Email)
            .HasMaxLength(400)
            .HasConversion(
                c => c.Value,
                value => Email.Create(value).Value
            );

        builder.Property(x => x.Password)
            .HasMaxLength(200)
            .HasConversion(
                c => c.Value,
                value => new UserPassword(value)
            );

        builder.Property(x => x.BirthDate)
            .HasConversion(
                c => c.Value,
                value => new UserBirthDate(value)
            );

        builder.Property(x => x.ImageUrl)
            .IsRequired(false)
            .HasConversion(
                c => c!.Value,
                value => new UserImageUrl(value)
            );

        builder.Property(x => x.BirthDate)
            .HasConversion(
                c => c.Value,
                value => new UserBirthDate(value)
            );

        builder.Property(x => x.RoleId).IsRequired();

        builder.Property(x => x.StatusId).IsRequired();

        builder.Property(x => x.OrganizationId).IsRequired();

        builder.Property(x => x.CountryId).IsRequired();

        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(x => x.LastModifiedOn);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}