using TeasyBot.Leaderboard.Dtos;

namespace TeasyBot.Database.Services.Abstractions;

public interface IApprovedChannelsService
{
    Task<ApprovedChannelDto?> GetApprovedChannelAsync(string guildId, string channelId);
    Task<IEnumerable<ApprovedChannelDto>> GetApprovedChannelByGuildAsync(string guildId);
    Task<ApprovedChannelDto> AddApprovedChannelAsync(string guildId, string channelId);
    Task RemoveApprovedChannelAsync(string guildId, string channelId);
    Task RemoveApprovedChannelByGuildAsync(string guildId);
}