using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.Update;

[ProtoContract]
public record UpdateRoomRequest
{
    public UpdateRoomRequest()
    {
        RoomSettings = new();
    }

    public UpdateRoomRequest(
        string id,
        string name,
        VotingSystemsDto votingSystem,
        RoomPermissionsDto whoCanRevealCards,
        RoomPermissionsDto whoCanManageIssues,
        bool autoRevealCards,
        bool showAverage)
    {
        Id = id;
        RoomSettings = new()
        {
            Name = name,
            VotingSystem = votingSystem,
            WhoCanRevealCards = whoCanRevealCards,
            WhoCanManageIssues = whoCanManageIssues,
            AutoRevealCards = autoRevealCards,
            ShowAverage = showAverage
        };
    }

    [ProtoMember(1)]
    public string Id { get; set; } = null!;

    [ProtoMember(2)]
    public RoomSettingsDto RoomSettings { get; set; }
}
