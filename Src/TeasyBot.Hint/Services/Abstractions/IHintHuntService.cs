using TeasyBot.Hint.Dtos;

namespace TeasyBot.Hint.Services.Abstractions;

public interface IHintHuntService
{
    HintHuntDto? GetHintHuntForGuild(string guildId);
    HintHuntDto EnableHintHuntForGuild(string guildId);
    HintHuntDto DisableHintHuntForGuild(string guildId);
    bool AddParticipantToHintHunt(string guildId, string userId);
    bool RemoveParticipantFromHintHunt(string guildId, string userId);
    bool IsParticipantInHintHunt(string guildId, string userId);
}