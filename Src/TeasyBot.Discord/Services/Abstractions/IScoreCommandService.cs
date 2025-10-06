using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IScoreCommandService
{
    Task HandleLeaderboardCommandAsync(SocketSlashCommand command);
    Task HandleProfileCommandAsync(SocketSlashCommand command);
}