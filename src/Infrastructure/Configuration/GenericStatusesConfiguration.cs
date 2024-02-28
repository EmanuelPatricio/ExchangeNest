using Domain.GenericStatuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class GenericStatusesConfiguration : IEntityTypeConfiguration<GenericStatus>
{
    public void Configure(EntityTypeBuilder<GenericStatus> builder)
    {
        builder.ToTable("GenericStatuses");

        builder.HasKey(status => status.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Concept).IsRequired();
        builder.Property(x => x.Order).IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(50)
            .IsRequired();
    }
}
