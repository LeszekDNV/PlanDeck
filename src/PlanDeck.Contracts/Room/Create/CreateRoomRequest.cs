using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.Create;

[ProtoContract]
public record CreateRoomRequest
{
    public CreateRoomRequest()
    {

    }

    public CreateRoomRequest(
        string name,
        VotingSystemsDto votingSystem,
        RoomPermissionsDto whoCanRevealCards,
        RoomPermissionsDto whoCanManageIssues,
        bool autoRevealCards,
        bool showAverage)
    {
        Name = name;
        VotingSystem = votingSystem;
        WhoCanRevealCards = whoCanRevealCards;
        WhoCanManageIssues = whoCanManageIssues;
        AutoRevealCards = autoRevealCards;
        ShowAverage = showAverage;
    }

    [ProtoMember(1)]
    public string Name { get; set; }

    [ProtoMember(2)]
    public VotingSystemsDto VotingSystem { get; set; }

    [ProtoMember(3)]
    public RoomPermissionsDto WhoCanRevealCards { get; set; }

    [ProtoMember(4)]
    public RoomPermissionsDto WhoCanManageIssues { get; set; }

    [ProtoMember(5)]
    public bool AutoRevealCards { get; set; }

    [ProtoMember(6)]
    public bool ShowAverage { get; set; }

    [ProtoMember(7)]
    public UserDto Owner { get; set; }
}