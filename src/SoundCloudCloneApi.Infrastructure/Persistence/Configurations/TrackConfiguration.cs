using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Infrastructure.Persistence.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable("Tracks");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(1000);
        builder.Property(t => t.Genre).HasMaxLength(50);
        builder.Property(t => t.AudioFileUrl).IsRequired();
        builder.Property(t => t.CoverImageUrl);
        builder.Property(t => t.PlaysCount).HasDefaultValue(0);

        builder.HasIndex(t => t.Genre);
        builder.HasIndex(t => t.Title);
    }
}