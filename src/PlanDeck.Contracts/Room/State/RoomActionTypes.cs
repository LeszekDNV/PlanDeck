using ProtoBuf;

namespace PlanDeck.Contracts.Room.State;

[ProtoContract]
public enum RoomActionTypes
{
    [ProtoEnum] PING = 0,
    [ProtoEnum] JOIN_ROOM = 1,
    [ProtoEnum] PLAY_CARD = 2,
    [ProtoEnum] REVEAL_CARDS = 3,
    [ProtoEnum] CHANGE_SETTINGS = 4,
    [ProtoEnum]  CHANGE_ACTIVE_ISSUE = 5,
}