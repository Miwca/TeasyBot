using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IEnableCommandService
{
    Task HandleEnableCommandAsync(SocketSlashCommand command);
    Task HandleDisableCommandAsync(SocketSlashCommand command);
}