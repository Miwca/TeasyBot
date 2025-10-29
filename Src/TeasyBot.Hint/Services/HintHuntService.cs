using TeasyBot.Cache.Services.Abstractions;
using TeasyBot.Hint.Dtos;
using TeasyBot.Hint.Services.Abstractions;

namespace TeasyBot.Hint.Services;

public class HintHuntService : IHintHuntService
{
    private readonly IMemoryCacheService _memoryCache;

    public HintHuntService(IMemoryCacheService memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public HintHuntDto? GetHintHuntForGuild(string guildId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        return hintHunt;
    }

    public HintHuntDto EnableHintHuntForGuild(string guildId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        if (hintHunt is { Enabled: true }) return hintHunt;

        hintHunt = new HintHuntDto()
        {
            Enabled = true
        };

        _memoryCache.Set(guildId, hintHunt);
        return hintHunt;
    }

    public HintHuntDto DisableHintHuntForGuild(string guildId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        switch (hintHunt)
        {
            case null:
                return new HintHuntDto() { Enabled = false };
            case { Enabled: false }:
                return hintHunt;
        }

        hintHunt = new HintHuntDto()
        {
            Enabled = false
        };

        _memoryCache.Set(guildId, hintHunt);
        return hintHunt;
    }

    public bool AddParticipantToHintHunt(string guildId, string userId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        if (hintHunt is not { Enabled: true })
        {
            return false;
        }

        var participant = hintHunt.Participants.FirstOrDefault(p => p == userId);
        if (participant != null)
        {
            return false;
        }

        hintHunt.Participants.Add(userId);
        _memoryCache.Set(guildId, hintHunt);

        return true;
    }

    public bool RemoveParticipantFromHintHunt(string guildId, string userId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        if (hintHunt is not { Enabled: true })
        {
            return false;
        }

        var participant = hintHunt.Participants.FirstOrDefault(p => p == userId);
        if (participant != null)
        {
            return false;
        }

        hintHunt.Participants.Remove(userId);
        _memoryCache.Set(guildId, hintHunt);

        return true;
    }

    public bool IsParticipantInHintHunt(string guildId, string userId)
    {
        var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
        if (hintHunt is not { Enabled: true })
        {
            return false;
        }

        return hintHunt.Participants.Contains(userId);
    }
}