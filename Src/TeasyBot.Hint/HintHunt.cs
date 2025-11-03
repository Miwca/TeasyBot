using Discord;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TeasyBot.Cache.Services.Abstractions;
using TeasyBot.Database.Services.Abstractions;
using TeasyBot.Discord.Sender.Abstractions;
using TeasyBot.Hint.Settings;
using TeasyBot.Hint.Abstractions;
using TeasyBot.Hint.Dtos;
using TeasyBot.Hint.Services.Abstractions;
using TeasyBot.Lovense.Dtos;
using TeasyBot.Lovense.Exceptions;
using TeasyBot.Lovense.Services.Abstractions;

namespace TeasyBot.Hint
{
    public class HintHunt : IHintHunt
    {
        private readonly ILogger<HintHunt> _logger;
        private readonly IHintHuntService _hintHuntService;
        private readonly IHintService _hintService;
        private readonly IApprovedChannelsService _channelsService;
        private readonly IDiscordMessageSender _messageSender;
        private readonly IMemoryCacheService _cacheService;
        private readonly ILovenseService _lovenseService;
        private readonly IWebHookMessageSender _webHookMessageSender;

        private readonly Random _random;
        private readonly HintHuntConfig _config;

        public HintHunt(ILogger<HintHunt> logger, IHintHuntService hintHuntService, 
            IHintService hintService, IApprovedChannelsService channelService,
            IDiscordMessageSender messageSender, IMemoryCacheService cacheService, 
            ILovenseService lovenseService, IWebHookMessageSender webHookMessageSender, 
            IOptions<HintHuntConfig> config)
        {
            _logger = logger;
            _hintHuntService = hintHuntService;
            _hintService = hintService;
            _channelsService = channelService;
            _messageSender = messageSender;
            _cacheService = cacheService;
            _lovenseService = lovenseService;
            _webHookMessageSender = webHookMessageSender;

            _random = new Random();
            _config = config.Value;
        }

        public async Task StartHintHuntForGuildAsync(string guildId)
        {
            var hunt = _hintHuntService.GetHintHuntForGuild(guildId);
            if (hunt is { Enabled: true }) return;

            _cacheService.Set($"{guildId}-hunt-ongoing", false);

            _hintHuntService.EnableHintHuntForGuild(guildId);
            _logger.LogDebug($"Hint hunt started for guild with ID {guildId}");

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(_config.TimerSeconds));
            var messageId = string.Empty;
            while (await timer.WaitForNextTickAsync())
            {
                hunt = _hintHuntService.GetHintHuntForGuild(guildId);
                if (hunt is not { Enabled: true })
                {
                    _logger.LogDebug($"Hint hunt is no longer ongoing for {guildId}. Stopping game loop...");
                    return;
                }

                var ongoing = _cacheService.Get<bool>($"{guildId}-hunt-ongoing");
                if (ongoing)
                {
                    _logger.LogDebug("Hunt is still ongoing. No need to drop a new hint now...");
                    continue;
                }

                if (hunt.Participants.Count == 0)
                {
                    _logger.LogDebug("No participants. Not dropping any hints.");
                    continue;
                }

                if (_random.Next(_config.Probability) > _config.Chance)
                {
                    _logger.LogDebug("Probability to drop hint not hit...");
                    continue;
                }

                var channels = await _channelsService.GetApprovedChannelByGuildAsync(guildId);
                var channelsArr = channels.ToArray();
                var hintChannel = _random.Next(channelsArr.Length);

                for (var i = 0; i < channelsArr.Length; i++)
                {
                    IUserMessage? message;
                    if (i == hintChannel)
                    {
                        message = await BuildHintEmbedAsync(_hintService.GetRandomHint(), channelsArr[i].ChannelId);
                    }
                    else
                    {
                        message = await BuildDudEmbedAsync(_hintService.GetRandomDud(), channelsArr[i].ChannelId);
                    }

                    _cacheService.Set($"{message!.Id}-participant-count", hunt.Participants.Count);
                    _cacheService.Set($"{guildId}-hunt-ongoing", true);
                }

                await StartVibeLoop(guildId, hunt.Participants);
            }
        }

        public Task StopHintHuntForGuild(string guildId)
        {
            _hintHuntService.DisableHintHuntForGuild(guildId);
            _logger.LogDebug($"Hint hunt stopped for guild with ID {guildId}, no more hints will be dropped, any existing hints can still be collected.");

            return Task.CompletedTask;
        }

        private async Task StartVibeLoop(string guildId, List<string> participants)
        {
            var strength = 1;
            await CallLovense(participants, strength);

            for (var i = 0; i < _config.VibeLoopSeconds; i++)
            {
                var ongoing = _cacheService.Get<bool>($"{guildId}-hunt-ongoing");
                if (!ongoing) return;

                if (i % 3 == 0)
                {
                    strength = strength + 1 > 20 ? 20 : strength + 1;

                    await CallLovense(participants, strength);
                }
                
                await Task.Delay(1000);
            }

            _cacheService.Set($"{guildId}-hunt-ongoing", false);
        }

        private async Task CallLovense(List<string> participants, int strength)
        {
            try
            {
                await _lovenseService.CommandAsync(participants, new WebCommandDto()
                {
                    Strength = strength,
                    Seconds = 4
                });
            }
            catch (GeneralLovenseException gle)
            {
                _logger.LogError($"Lovense error occurred while sending incremental timer strength. {gle.Message}");
                await _webHookMessageSender.SendErrorAsync($"Lovense error occurred while sending incremental timer strength: {gle.Message}", gle.StatusCode);
            }
            catch (Exception e)
            {
                _logger.LogError($"General exception occurred while sending incremental timer strength. {e.Message}");
                await _webHookMessageSender.SendErrorAsync($"General exception occurred while sending incremental timer strength. {e.Message}");
            }
        }

        private async Task<IUserMessage?> BuildHintEmbedAsync(HintDto hint, string channelId)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle(hint.Name)
                .WithDescription(BuildDescription(hint.Description))
                .WithImageUrl(hint.ImageUrl)
                .WithColor(Color.Purple)
                .WithFooter("TeasyBot - Made by @miwca and @kitty_cass");

            var componentBuilder = new ComponentBuilder()
                .WithButton("Collect Hint", $"find-{hint.Name}", ButtonStyle.Success);

            return await _messageSender.SendMessageToChannelAsync(channelId, embedBuilder, componentBuilder);
        }

        private async Task<IUserMessage?> BuildDudEmbedAsync(DudDto dud, string channelId)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle(dud.Name)
                .WithDescription(dud.Description)
                .WithImageUrl(dud.ImageUrl)
                .WithColor(Color.Purple)
                .WithFooter("TeasyBot - Made by @miwca and @kitty_cass");

            return await _messageSender.SendMessageToChannelAsync(channelId, embedBuilder);
        }

        private string BuildDescription(string description)
        {
            if (string.IsNullOrEmpty(_config.ProductUrl)) return description;

            return $"{description}\n\nGet yours here: {_config.ProductUrl}";
        }
    }
}
