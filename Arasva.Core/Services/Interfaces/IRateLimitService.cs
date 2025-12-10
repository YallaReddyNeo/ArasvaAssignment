namespace Arasva.Core.Services.Interfaces
{
    public interface IRateLimitService
    {
        Task<GlobalResponse> CheckUserAccessLimit(string UserId);
    }
}
