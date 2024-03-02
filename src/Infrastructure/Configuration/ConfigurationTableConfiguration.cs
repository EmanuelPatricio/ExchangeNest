using Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class ConfigurationTableConfiguration : IEntityTypeConfiguration<Domain.Configurations.Configuration>
{
    public void Configure(EntityTypeBuilder<Domain.Configurations.Configuration> builder)
    {
        builder.ToTable("Configurations");

        builder.HasKey(configuration => configuration.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new ConfigurationId(g)
        );

        builder.Property(x => x.SenderMail).IsRequired();

        builder.Property(x => x.SenderPassword).IsRequired();

        builder.Property(x => x.EmailTemplate).IsRequired();
    }
}