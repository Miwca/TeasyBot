using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface ISlashCommandService
{
    Task BuildSlashCommandsAsync();
    Task HandleTestCommandAsync(SocketSlashCommand command);
}