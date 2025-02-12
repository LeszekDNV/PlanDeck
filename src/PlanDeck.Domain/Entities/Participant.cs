namespace PlanDeck.Domain.Entities;

public class Participant : BaseEntity
{
    public string Name { get; set; }

    public bool IsObserver { get; set; }

    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}