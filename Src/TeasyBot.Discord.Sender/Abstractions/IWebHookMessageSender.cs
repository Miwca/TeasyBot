using TeasyBot.Lovense.Enums;

namespace TeasyBot.Discord.Sender.Abstractions;

public interface IWebHookMessageSender
{
    Task SendErrorAsync(string message, int errorCode);
    Task SendErrorAsync(string message);
}