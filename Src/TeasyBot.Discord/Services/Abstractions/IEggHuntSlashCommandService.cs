using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IEggHuntSlashCommandService
{
    Task JoinSlashCommandHandlerAsync(SocketSlashCommand command);
    Task LeaveSlashCommandHandlerAsync(SocketSlashCommand command);
}