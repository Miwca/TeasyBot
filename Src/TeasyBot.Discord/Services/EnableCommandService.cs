using TeasyBot.Discord.Services.Abstractions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using TeasyBot.Hint.Abstractions;

namespace TeasyBot.Discord.Services
{
    public class EnableCommandService : IEnableCommandService
    {
        private readonly ILogger<EnableCommandService> _logger;
        private readonly IHintHunt _hintHunt;

        public EnableCommandService(ILogger<EnableCommandService> logger, IHintHunt hintHunt)
        {
            _logger = logger;
            _hintHunt = hintHunt;
        }

        public async Task HandleEnableCommandAsync(SocketSlashCommand command)
        {
            // Send a message with an attached button with the ID `join` and the label `Join`
            var builder = new ComponentBuilder()
                .WithButton("Join",  "join", ButtonStyle.Primary)
                //.WithButton("Test Hint", "test-hint", ButtonStyle.Secondary) // Test spawn an hint
                .WithButton("Leave", "leave", ButtonStyle.Danger);

            #pragma warning disable CS4014
            Task.Run(() => _hintHunt.StartHintHuntForGuildAsync(command.GuildId.ToString()!));
            #pragma warning restore CS4014

            await command.RespondAsync("Join the hunt for the mysterious hints around the Discord server!", components: builder.Build());
        }

        public async Task HandleDisableCommandAsync(SocketSlashCommand command)
        {
            await _hintHunt.StopHintHuntForGuild(command.GuildId.ToString()!);
            await command.RespondAsync("Hint hunt has been disabled for this server.");
        }
    }
}