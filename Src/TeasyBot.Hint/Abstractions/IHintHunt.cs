namespace TeasyBot.Hint.Abstractions
{
    public interface IHintHunt
    {
        Task StartHintHuntForGuildAsync(string guildId);
        Task StopHintHuntForGuild(string guildId);
    }
}
