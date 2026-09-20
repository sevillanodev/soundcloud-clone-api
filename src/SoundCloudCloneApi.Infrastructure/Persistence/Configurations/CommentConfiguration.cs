using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SoundCloudCloneApi.Domain.Entities;

namespace SoundCloudCloneApi.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content).IsRequired().HasMaxLength(500);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Track)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TrackId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}