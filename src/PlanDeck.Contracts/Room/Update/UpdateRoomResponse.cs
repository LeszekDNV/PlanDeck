using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.Update;

[ProtoContract]
public class UpdateRoomResponse()
{
    public UpdateRoomResponse(bool success) : this()
    {
        Success = success;
    }

    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public RoomSettingsDto RoomSettings { get; set; }
}
