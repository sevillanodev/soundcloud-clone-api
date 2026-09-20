using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.DisplayName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Bio).HasMaxLength(500);
        builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(20);

        builder.HasMany(u => u.Tracks)
            .WithOne(t => t.Artist)
            .HasForeignKey(t => t.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Playlists)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Followers)
            .WithOne(f => f.Following)
            .HasForeignKey(f => f.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Following)
            .WithOne(f => f.Follower)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}