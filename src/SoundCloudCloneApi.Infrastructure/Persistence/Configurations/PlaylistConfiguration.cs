using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Infrastructure.Persistence.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> builder)
    {
        builder.ToTable("Playlists");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(150);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}