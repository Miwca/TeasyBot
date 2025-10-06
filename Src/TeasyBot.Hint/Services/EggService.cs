using Microsoft.Extensions.Options;
using TeasyBot.Cache.Services.Abstractions;
using TeasyBot.Egg.Dtos;
using TeasyBot.Egg.Services.Abstractions;
using TeasyBot.Egg.Settings;

namespace TeasyBot.Egg.Services
{
    public class EggService : IEggService
    {
        private readonly IMemoryCacheService _memoryCache;

        private readonly List<EggDto> _eggs;
        private readonly List<DudDto> _duds;

        public EggService(IOptions<EggConfig> config, IMemoryCacheService memoryCache)
        {
            _memoryCache = memoryCache;

            _eggs = config.Value.Eggs;
            _duds = config.Value.Duds;
        }

        public EggDto GetRandomEgg()
        {
            var random = new Random();
            var next = random.Next(_eggs.Count);

            return _eggs[next];
        }

        public DudDto GetRandomDud()
        {
            var random = new Random();
            var next = random.Next(_duds.Count);

            return _duds[next];
        }

        public EggDto? GetEggByName(string name)
        {
            return _eggs.FirstOrDefault(x => x.Name == name);
        }

        public EggHuntDto? GetEggHuntForGuild(string guildId)
        {
            var eggHunt = _memoryCache.Get<EggHuntDto>(guildId);
            return eggHunt;
        }
    }
}
