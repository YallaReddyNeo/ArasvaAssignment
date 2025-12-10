namespace Arasva.Core.Interface
{
    public interface IRateLimitRepository
    {
        Task<GlobalResponse> CheckUserAccessLimit(string UserId);
    }
}
