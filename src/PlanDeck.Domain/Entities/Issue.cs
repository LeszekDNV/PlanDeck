namespace PlanDeck.Domain.Entities;

public class Issue : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}