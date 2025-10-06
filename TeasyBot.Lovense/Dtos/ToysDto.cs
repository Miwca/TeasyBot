using Newtonsoft.Json;

namespace TeasyBot.Lovense.Dtos;

public class ToysDto
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("nickName")]
    public string? Nickname { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("status")]
    public int Status { get; set; }
}