using TeasyBot.Hint.Dtos;

namespace TeasyBot.Hint.Settings
{
    public class HintConfig
    {
        public List<HintDto> Hints { get; set; } = new List<HintDto>();
        public List<DudDto> Duds { get; set; } = new List<DudDto>();
    }
}
