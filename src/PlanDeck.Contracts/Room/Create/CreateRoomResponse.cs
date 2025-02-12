using ProtoBuf;

namespace PlanDeck.Contracts.Room.Create;

[ProtoContract]
public class CreateRoomResponse()
{
    public CreateRoomResponse(string id) : this()
    {
        Id = id;
    }

    public string Id { get; }
}
