using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlanDeck.Domain.Entities;

namespace PlanDeck.Infrastructure.Persistence.EntityConfigurations;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("Issues");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Description)
            .HasMaxLength(1000);

        builder.Property(i => i.IsActive)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(i => i.Room)
            .WithMany(r => r.Issues)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(i => i.Votes)
            .WithOne(v => v.Issue)
            .HasForeignKey(v => v.IssueId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
