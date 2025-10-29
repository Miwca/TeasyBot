using Discord;
using Discord.WebSocket;
using TeasyBot.Lovense.Exceptions;
using Microsoft.Extensions.Logging;
using TeasyBot.Discord.Sender.Abstractions;
using TeasyBot.Discord.Services.Abstractions;
using TeasyBot.Hint.Services.Abstractions;
using TeasyBot.Lovense.Services.Abstractions;

namespace TeasyBot.Discord.Services;

public class HintHuntSlashCommandService : IHintHuntSlashCommandService
{
    private readonly ILogger<HintHuntSlashCommandService> _logger;
    private readonly ILovenseService _lovenseService;
    private readonly IHintHuntService _hintHuntService;
    private readonly IWebHookMessageSender _webHookMessageSender;

    public HintHuntSlashCommandService(ILogger<HintHuntSlashCommandService>  logger, ILovenseService lovenseService, 
        IHintHuntService hintHuntService, IWebHookMessageSender webHookMessageSender)
    {
        _logger = logger;
        _lovenseService = lovenseService;
        _hintHuntService = hintHuntService;
        _webHookMessageSender = webHookMessageSender;
    }

    public async Task JoinSlashCommandHandlerAsync(SocketSlashCommand command)
    {
        await command.DeferAsync(ephemeral: true);
        var uId = command.User.Id.ToString();

        // uToken isn't being used as we aren't verifying it.
        try
        {
            var qrCodeDetails = await _lovenseService.GenerateQrCodeAsync(uId, uId, uId);
            if (qrCodeDetails == null)
            {
                await command.FollowupAsync("Failed to generate QR code. Please try again later.",
                    ephemeral: true);
                return;
            }

            var qrCode = qrCodeDetails.ImageUrl;
            var qrCodeUniqueCode = qrCodeDetails.UniqueCode;

            var embedBuilder = new EmbedBuilder()
                .WithTitle("Lovense QR Code")
                .WithDescription(@$"
Scan the QR code to connect your toy.

Or Connect via the Code:
**Unique Code:** {qrCodeUniqueCode}"
                )
                .WithImageUrl(qrCode)
                .WithColor(215, 42, 119) // Lovense Pink
            .WithFooter("TeasyBot - Made by @miwca and @kitty_cass");

            _hintHuntService.AddParticipantToHintHunt(command.GuildId.ToString()!, command.User.Id.ToString());
            await command.FollowupAsync(embed: embedBuilder.Build(), ephemeral: true);
        }
        catch (GeneralLovenseException gle)
        {
            _logger.LogError($"Lovense error occurred while generating QR code. {gle.Message}");
            await _webHookMessageSender.SendErrorAsync($"Lovense error occurred while generating QR code. {gle.Message}", gle.StatusCode);

            await command.FollowupAsync("Failed to generate QR code. Please try again later.",
                ephemeral: true);
        }
        catch (Exception e)
        {
            _logger.LogError($"General exception occurred while generating QR code. {e.Message}");
            await _webHookMessageSender.SendErrorAsync($"General exception occurred while generating QR code. {e.Message}");

            await command.FollowupAsync("Failed to generate QR code. Please try again later.",
                ephemeral: true);
        }
    }

    public async Task LeaveSlashCommandHandlerAsync(SocketSlashCommand command)
    {
        await command.DeferAsync();

        _hintHuntService.RemoveParticipantFromHintHunt(command.GuildId.ToString()!, command.User.Id.ToString());

        await command.FollowupAsync("Successfully left, we hope you join us again later.", ephemeral: true);
    }
}