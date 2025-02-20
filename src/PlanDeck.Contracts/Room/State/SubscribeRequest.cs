using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.State;

[ProtoContract]
public class SubscribeRequest
{
    [ProtoMember(1)] public string RoomId { get; set; } = null!;
    [ProtoMember(2)] public UserDto User { get; set; } = null!;
}
