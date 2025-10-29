using TeasyBot.Lovense.Settings;
using Discord.WebSocket;
using Discord;
using TeasyBot.Cache.Services;
using TeasyBot.Cache.Services.Abstractions;
using TeasyBot.Database.Repositories;
using TeasyBot.Database.Repositories.Abstractions;
using TeasyBot.Database.Services;
using TeasyBot.Database.Services.Abstractions;
using DiscordConfig = TeasyBot.Discord.Settings.DiscordConfig;
using TeasyBot.Discord.Handlers;
using TeasyBot.Discord.Services.Abstractions;
using TeasyBot.Discord.Services;
using TeasyBot.Lovense.Services.Abstractions;
using TeasyBot.Lovense.Services;
using TeasyBot.Discord;
using TeasyBot.Discord.Sender;
using TeasyBot.Hint.Settings;
using TeasyBot.Lovense;
using TeasyBot.Lovense.Abstractions;
using TeasyBot.Discord.Sender.Abstractions;
using TeasyBot.Discord.Sender.Settings;
using TeasyBot.Hint;
using TeasyBot.Hint.Abstractions;
using TeasyBot.Hint.Services;
using TeasyBot.Hint.Services.Abstractions;
using TeasyBot.Hint.Settings;

namespace TeasyBot.Service.Extensions
{
    public static class DependencyExtensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Config
            var lovenseSection = configuration.GetSection("Lovense");
            var lovenseConfig = lovenseSection.Get<LovenseConfig>();
            services.AddHttpClient(
                lovenseConfig?.ClientName ?? "LovenseClient",
                client =>
                {
                    // Set the base address of the named client.
                    client.BaseAddress = new Uri(lovenseConfig!.ApiRoot!);

                    // Add a user-agent default request header.
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-docs");
                }
            );

            services.AddMemoryCache();

            services.Configure<LovenseConfig>(lovenseSection);
            services.Configure<DiscordConfig>(configuration.GetSection("Discord"));
            services.Configure<HintConfig>(configuration.GetSection("Hint"));
            services.Configure<HintHuntConfig>(configuration.GetSection("HintHunt"));
            services.Configure<DiscordWebHookConfig>(configuration.GetSection("DiscordWebHook"));
            var discordConfig = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.None
            };

            // Singletons
            services.AddSingleton(_ => discordConfig);
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<SlashCommandHandler>();
            services.AddSingleton<ButtonInteractionHandler>();

            services.AddSingleton<ISlashCommandService, SlashCommandService>();
            services.AddSingleton<IEnableCommandService, EnableCommandService>();
            services.AddSingleton<IScoreCommandService, ScoreCommandService>();
            services.AddSingleton<IChannelCommandService, ChannelCommandService>();
            services.AddSingleton<IHintHuntSlashCommandService, HintHuntSlashCommandService>();
            services.AddSingleton<IButtonInteractionService, ButtonInteractionService>();
            services.AddSingleton<IDiscordMessageSender, DiscordMessageSender>();
            services.AddSingleton<IWebHookMessageSender, WebHookMessageSender>();

            services.AddSingleton<IHintHunt, HintHunt>();
            services.AddSingleton<ILovenseService, LovenseService>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddSingleton<IHintService, HintService>();
            services.AddSingleton<IHintHuntService, HintHuntService>();
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
            services.AddSingleton<IApprovedChannelsService, ApprovedChannelsService>();
            services.AddSingleton<ILeaderboardRepository>(_ =>
                new LeaderboardRepository(configuration.GetValue<string>("ConnectionStrings:Leaderboard") ??
                                          throw new ArgumentNullException("connectionString")));
            services.AddSingleton<IApprovedChannelsRepository>(_ =>
                new ApprovedChannelsRepository(configuration.GetValue<string>("ConnectionStrings:Leaderboard") ??
                                          throw new ArgumentNullException("connectionString")));

            // Transients
            services.AddTransient<ILovenseClient, LovenseClient>();

        // Hosted services
            services.AddHostedService<DiscordClient>();

            return services;
        }
    }
}
