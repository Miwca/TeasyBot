using TeasyBot.Egg.Dtos;

namespace TeasyBot.Egg.Services.Abstractions
{
    public interface IEggService
    {
        EggDto GetRandomEgg();
        DudDto GetRandomDud();
        EggDto? GetEggByName(string name);
    }
}
