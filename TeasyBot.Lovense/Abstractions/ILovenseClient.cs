using TeasyBot.Lovense.Requests;
using TeasyBot.Lovense.Responses;

namespace TeasyBot.Lovense.Abstractions
{
    public interface ILovenseClient
    {
        Task<WebCommandResponseV2?> GetQrCodeAsync(WebGetQrCodeRequest request);
        Task<WebCommandResponseV2?> CommandAsync(WebCommandRequest request);
        Task<WebCommandResponseV2?> CommandPatternAsync(WebCommandPatternRequest request);
    }
}
