using TeasyBot.Lovense.Enums;

namespace TeasyBot.Lovense.Requests
{
    public class WebCommandRequest
    {
        public List<string>? UserIds { get; set; }
        public int Strength { get; set; }
        public int Seconds { get; set; }
    }
}
