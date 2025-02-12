using PlanDeck.Domain.Entities.Enums;

namespace PlanDeck.Domain.Entities;

public class Room : BaseEntity
{
    public string Name { get; set; }
    public VotingSystem VotingSystem { get; set; }
    public RoomPermission WhoCanRevealCards { get; set; }
    public RoomPermission WhoCanManageIssues { get; set; }
    public bool AutoRevealCards { get; set; }
    public bool ShowAverage { get; set; }

    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}