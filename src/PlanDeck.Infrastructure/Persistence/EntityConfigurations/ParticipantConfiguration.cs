using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlanDeck.Domain.Entities;

namespace PlanDeck.Infrastructure.Persistence.EntityConfigurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.Room)
            .WithMany(r => r.Participants)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Votes)
            .WithOne(v => v.Participant)
            .HasForeignKey(v => v.ParticipantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
