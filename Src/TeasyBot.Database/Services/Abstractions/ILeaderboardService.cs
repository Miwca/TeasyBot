using TeasyBot.Leaderboard.Dtos;

namespace TeasyBot.Database.Services.Abstractions
{
    public interface ILeaderboardService
    {
        Task<IEnumerable<LeaderboardDto>> GetGuildTopLeaderboardAsync(string guildId);
        Task<LeaderboardDto?> GetLeaderboardEntryByGuildAndUser(string guildId, string userId);
        Task<LeaderboardDto> UpLeaderboardEntryScore(string guildId, string userId);
    }
}
