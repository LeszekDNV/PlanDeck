using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.State;

[ProtoContract]
public class ServerStreamMessage
{
    [ProtoMember(1)]
    public RoomEventTypes EventType { get; set; }

    [ProtoMember(2)]
    public string RoomId { get; set; } = null!;

    [ProtoMember(3)]
    public RoomSettingsDto? RoomSettings { get; set; }

    [ProtoMember(4)]
    public UserDto User { get; set; } = null!;

    [ProtoMember(5)]
    public string ErrorMessage { get; set;} = null!;
}