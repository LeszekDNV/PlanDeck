using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlanDeck.Domain.Entities;

namespace PlanDeck.Infrastructure.Persistence.EntityConfigurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("Votes");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Property(v => v.CardValue)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(v => v.Issue)
            .WithMany(i => i.Votes)
            .HasForeignKey(v => v.IssueId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(v => v.Participant)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.ParticipantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
