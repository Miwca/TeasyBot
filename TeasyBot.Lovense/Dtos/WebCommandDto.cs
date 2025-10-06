using TeasyBot.Lovense.Enums;

namespace TeasyBot.Lovense.Dtos;

public class WebCommandDto
{
    public LovenseCommandEnum Command { get; set; }
    public int Strength { get; set; }
    public int Seconds { get; set; }
}