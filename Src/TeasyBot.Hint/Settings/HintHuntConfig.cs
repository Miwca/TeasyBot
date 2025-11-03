namespace TeasyBot.Hint.Settings;

public class HintHuntConfig
{
    public int TimerSeconds { get; set; }
    public int Chance { get; set; }
    public int Probability { get; set; }
    public int MaxClaimed { get; set; }
    public int VibeLoopSeconds { get; set; }
    public string ProductUrl { get; set; } = string.Empty;
}