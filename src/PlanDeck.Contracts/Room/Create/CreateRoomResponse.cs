using ProtoBuf;

namespace PlanDeck.Contracts.Room.Create;

[ProtoContract]
public class CreateRoomResponse()
{
    public CreateRoomResponse(string id) : this()
    {
        Id = id;
    }

    [ProtoMember(1)]
    public string Id { get; }
}
