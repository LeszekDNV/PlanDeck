using ProtoBuf;

namespace PlanDeck.Contracts.Dtos;

[ProtoContract]
public class UserDto
{
    [ProtoMember(1)]
    public string Name { get; set; }

    [ProtoMember(2)]
    public string? LastPlanningRoom { get; set; }
}
