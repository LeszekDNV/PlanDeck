using PlanDeck.Application.Interfaces;
using PlanDeck.Contracts.Dtos;
using PlanDeck.Contracts.Room.Create;
using PlanDeck.Domain.Entities;
using PlanDeck.Domain.Entities.Enums;

namespace PlanDeck.Application.Services;

public class RoomService(IKeyRepository<Room, Guid> roomRepository) : IRoomService
{
    public async Task<RoomDto> CreateRoomAsync(CreateRoomRequest request)
    {
        // Konwersja int -> enum
        var votingSystem = (VotingSystem)request.VotingSystem;
        var whoCanReveal = (RoomPermission)request.WhoCanRevealCards;
        var whoCanManage = (RoomPermission)request.WhoCanManageIssues;

        // Tworzymy nową encję domenową
        var newRoom = new Room
        {
            Name = request.Name,
            VotingSystem = votingSystem,
            WhoCanRevealCards = whoCanReveal,
            WhoCanManageIssues = whoCanManage,
            AutoRevealCards = request.AutoRevealCards,
            ShowAverage = request.ShowAverage,
            // Id i CreatedAt wygenerowane automatycznie dzięki konfiguracji EF + BaseEntity
        };

        // Zapis w repozytorium / bazie
        await roomRepository.AddAsync(newRoom);

        // Mapa zwrotnie na DTO
        var result = new RoomDto
        {
            Id = newRoom.Id,
            Name = newRoom.Name,
            VotingSystem = (int)newRoom.VotingSystem,
            AutoRevealCards = newRoom.AutoRevealCards,
        };

        return result;
    }
}

