namespace PlanDeck.Domain.Entities;

public class Vote : BaseEntity
{
    // e.g. "5", "8", "?", "XS", "M" itp.
    public string CardValue { get; set; }

    public Guid IssueId { get; set; }
    public Issue Issue { get; set; }
    
    public Guid ParticipantId { get; set; }
    public Participant Participant { get; set; }
}