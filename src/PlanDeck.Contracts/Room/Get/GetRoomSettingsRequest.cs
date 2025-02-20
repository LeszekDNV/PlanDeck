using ProtoBuf;

namespace PlanDeck.Contracts.Room.Get;

[ProtoContract]
public class GetRoomSettingsRequest
{
    public GetRoomSettingsRequest()
    { }

    public GetRoomSettingsRequest(string id)
    {
        Id = id;
    }

    [ProtoMember(1)]
    public string Id { get; set; } = null!;
}
