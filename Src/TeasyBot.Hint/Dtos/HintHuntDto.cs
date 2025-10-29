namespace TeasyBot.Hint.Dtos;

public class HintHuntDto
{
    public bool Enabled { get; set; }
    public List<string> Participants { get; set; } = new();
}