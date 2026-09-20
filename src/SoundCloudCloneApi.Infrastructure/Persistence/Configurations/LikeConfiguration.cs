using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Infrastructure.Persistence.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.ToTable("Likes");
        builder.HasKey(l => l.Id);

        builder.HasIndex(l => new { l.UserId, l.TrackId }).IsUnique();

        builder.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Track)
            .WithMany(t => t.Likes)
            .HasForeignKey(l => l.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}