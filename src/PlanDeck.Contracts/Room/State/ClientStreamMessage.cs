using PlanDeck.Contracts.Dtos;
using ProtoBuf;

namespace PlanDeck.Contracts.Room.State;

[ProtoContract]
public class ClientStreamMessage
{
    [ProtoMember(1)]
    public string RoomId { get; set; }

    [ProtoMember(2)]
    public RoomActionTypes ActionType { get; set; }

    // Przykładowe pola do przesyłania danych w zależności od typu akcji
    [ProtoMember(3)]
    public string CardValue { get; set; } = null!;

    [ProtoMember(4)]
    public bool AutoRevealCards { get; set; }

    [ProtoMember(5)]
    public UserDto User { get; set; } = null!;
}
