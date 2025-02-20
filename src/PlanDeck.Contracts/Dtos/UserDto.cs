using ProtoBuf;

namespace PlanDeck.Contracts.Dtos;

[ProtoContract]
public class UserDto
{
    public UserDto() { }

    public UserDto(string id, string name)
    {
        Id = id;
        Name = name;
    }

    [ProtoMember(1)]
    public string? Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; } = null!;

    [ProtoMember(3)]
    public string? LastPlanningRoom { get; set; }
}
