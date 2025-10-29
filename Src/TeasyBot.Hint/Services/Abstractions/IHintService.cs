using TeasyBot.Hint.Dtos;

namespace TeasyBot.Hint.Services.Abstractions
{
    public interface IHintService
    {
        HintDto GetRandomHint();
        DudDto GetRandomDud();
        HintDto? GetHintByName(string name);
    }
}
