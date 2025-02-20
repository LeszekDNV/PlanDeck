using ProtoBuf;

namespace PlanDeck.Contracts.Dtos;

[ProtoContract]
public class RoomSettingsDto
{
    [ProtoMember(1)]
    public string Name { get; set; } = null!;

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
    public UserDto? Owner { get; set; }
}
