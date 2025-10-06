using TeasyBot.Lovense.Dtos;
using TeasyBot.Lovense.Responses;

namespace TeasyBot.Lovense.Services.Abstractions
{
    public interface ILovenseService
    {
        Task<GenerateQrCodeResultDto?> GenerateQrCodeAsync(string userId, string username, string userToken);
        Task<WebCommandResponseV2?> CommandAsync(List<string> userIds, WebCommandDto command);
        Task<WebCommandResponseV2?> CommandPatternAsync(List<string> userIds, WebCommandPatternDto command);
    }
}
