using Discord.WebSocket;

namespace TeasyBot.Discord.Services.Abstractions;

public interface IButtonInteractionService
{
    Task JoinButtonHandler(SocketMessageComponent component);
    Task LeaveButtonHandler(SocketMessageComponent component);
    Task InvalidButtonHandler(SocketMessageComponent component);
    Task FindHintButtonHandler(SocketMessageComponent component);
    Task SendTestHintButtonHandler(SocketMessageComponent component);
}