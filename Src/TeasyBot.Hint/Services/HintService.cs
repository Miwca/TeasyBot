using Microsoft.Extensions.Options;
using TeasyBot.Cache.Services.Abstractions;
using TeasyBot.Hint.Dtos;
using TeasyBot.Hint.Services.Abstractions;
using TeasyBot.Hint.Settings;

namespace TeasyBot.Hint.Services
{
    public class HintService : IHintService
    {
        private readonly IMemoryCacheService _memoryCache;

        private readonly List<HintDto> _hints;
        private readonly List<DudDto> _duds;

        public HintService(IOptions<HintConfig> config, IMemoryCacheService memoryCache)
        {
            _memoryCache = memoryCache;

            _hints = config.Value.Hints;
            _duds = config.Value.Duds;
        }

        public HintDto GetRandomHint()
        {
            var random = new Random();
            var next = random.Next(_hints.Count);

            return _hints[next];
        }

        public DudDto GetRandomDud()
        {
            var random = new Random();
            var next = random.Next(_duds.Count);

            return _duds[next];
        }

        public HintDto? GetHintByName(string name)
        {
            return _hints.FirstOrDefault(x => x.Name == name);
        }

        public HintHuntDto? GetHintHuntForGuild(string guildId)
        {
            var hintHunt = _memoryCache.Get<HintHuntDto>(guildId);
            return hintHunt;
        }
    }
}
