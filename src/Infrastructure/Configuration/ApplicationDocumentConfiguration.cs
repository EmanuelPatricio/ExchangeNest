using Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
public class ApplicationDocumentConfiguration : IEntityTypeConfiguration<Domain.Applications.ApplicationDocument>
{
    public void Configure(EntityTypeBuilder<ApplicationDocument> builder)
    {
        builder.ToTable("ApplicationDocuments");

        builder.HasKey(applicationDocument => applicationDocument.Id);
        builder.Property(x => x.Id).HasConversion(
            (x) => x.Value,
            (g) => new ApplicationDocumentId(g)
        );

        builder.Property(x => x.DocumentType).IsRequired();

        builder.Property(x => x.DocumentUrl).IsRequired();

        builder.Property(x => x.CreatedOn).IsRequired();

        builder.Property(x => x.LastModifiedOn);

        builder
            .HasOne(e => e.Application)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.ApplicationId)
            .IsRequired();
    }
}
