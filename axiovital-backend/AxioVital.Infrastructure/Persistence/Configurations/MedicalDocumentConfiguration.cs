using AxioVital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AxioVital.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the MedicalDocument entity.
/// </summary>
public class MedicalDocumentConfiguration : IEntityTypeConfiguration<MedicalDocument>
{
    public void Configure(EntityTypeBuilder<MedicalDocument> builder)
    {
        builder.ToTable("medical_documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.TenantId)
            .IsRequired();

        builder.Property(d => d.FileName)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(d => d.StorageObjectName)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(d => d.StorageBucket)
            .IsRequired()
            .HasMaxLength(128)
            .HasDefaultValue("axiovital-documents");

        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.FileExtension)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(d => d.FileSizeBytes)
            .IsRequired();

        builder.Property(d => d.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(64);

        builder.Property(d => d.Description)
            .HasMaxLength(2048);

        builder.Property(d => d.Tags)
            .HasMaxLength(1024);

        builder.Property(d => d.UploadedByName)
            .HasMaxLength(256);

        // Indexes for common query patterns
        builder.HasIndex(d => d.TenantId);
        builder.HasIndex(d => d.PatientId);
        builder.HasIndex(d => d.Category);
        builder.HasIndex(d => d.CreatedAtUtc);
        builder.HasIndex(d => new { d.TenantId, d.PatientId, d.IsDeleted })
            .HasDatabaseName("IX_MedicalDocuments_Tenant_Patient_Active");
    }
}
