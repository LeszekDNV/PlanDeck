namespace PlanDeck.Contracts.Dtos;

public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int VotingSystem { get; set; }
    public bool AutoRevealCards { get; set; }
}
