using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IHintHuntSlashCommandService
{
    Task JoinSlashCommandHandlerAsync(SocketSlashCommand command);
    Task LeaveSlashCommandHandlerAsync(SocketSlashCommand command);
}