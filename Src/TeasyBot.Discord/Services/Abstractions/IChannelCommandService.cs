using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IChannelCommandService
{
    Task HandleAddChannelCommandAsync(SocketSlashCommand command);
    Task HandleRemoveChannelCommandAsync(SocketSlashCommand command);

    Task HandleListChannelsCommandAsync(SocketSlashCommand command);
}