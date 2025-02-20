using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.Get;

[ProtoContract]
public class GetRoomSettingsResponse
{
    [ProtoMember(1)] 
    public RoomSettingsDto RoomSettings { get; set; } = null!;
}
