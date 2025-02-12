using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlanDeck.Domain.Entities;

namespace PlanDeck.Infrastructure.Persistence.EntityConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEWID()");
        // Lub .HasDefaultValueSql("NEWSEQUENTIALID()") 

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.VotingSystem)
            .IsRequired();

        builder.Property(r => r.WhoCanRevealCards)
            .IsRequired();

        builder.Property(r => r.WhoCanManageIssues)
            .IsRequired();

        builder.Property(r => r.AutoRevealCards)
            .IsRequired();

        builder.Property(r => r.ShowAverage)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(r => r.Participants)
            .WithOne(p => p.Room)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.Issues)
            .WithOne(i => i.Room)
            .HasForeignKey(i => i.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}