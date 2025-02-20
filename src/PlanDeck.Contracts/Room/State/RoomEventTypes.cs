using ProtoBuf;

namespace PlanDeck.Contracts.Room.State;

[ProtoContract]
public enum RoomEventTypes
{
    [ProtoEnum] PING,
    [ProtoEnum] USER_JOINED = 1,
    [ProtoEnum] CARD_PLAYED = 2,
    [ProtoEnum] CARDS_REVEALED = 3,
    [ProtoEnum] SETTINGS_CHANGED = 4,
    [ProtoEnum] ACTIVE_ISSUE_CHANGED = 5,
    [ProtoEnum] USER_LEFT = 6
}