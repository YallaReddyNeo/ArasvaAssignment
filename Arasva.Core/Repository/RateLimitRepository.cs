using Arasva.Core.Interface;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Arasva.Core.Repository
{
    public class RateLimitRepository : IRateLimitRepository
    {
        //// Configuration: Max requests (X) per Window
        //private const int MaxRequests = 2;
        //private static readonly TimeSpan WindowDuration = TimeSpan.FromMinutes(1);

        //// Thread-safe dictionary to store state per user
        //private readonly ConcurrentDictionary<string, RateLimitRepository> _userStorage = new();

        //private readonly Queue<DateTime> _requestTimestamps = new();
        //private readonly object _lock = new();

        //// Define a delegate (function signature) that returns the limit for a given user ID
        //public delegate int UserLimitProvider(string userId);

        public async Task<GlobalResponse> CheckUserAccessLimit(string UserId)
        {
            GlobalResponse apiResponse = new();

            try
            {
                // Get the storage for this user, or create it if it doesn't exist
                //var userHistory = _userStorage.GetOrAdd(UserId, _ => new RateLimitRepository());

                // Delegate the logic to the user's specific history object
                var res = true; // IsRequestAllowed(UserId, MaxRequests, WindowDuration);

                //return response 
                apiResponse.message = res ? string.Format(AppConstants.ActionSuccess) : null;
                apiResponse.errorMessage = res ? null : "Rate limit exceeded. Please try again later.";
                apiResponse.success = res;
            }
            catch (Exception ex)
            {
                apiResponse.success = false;
                apiResponse.errorMessage = string.Format(AppConstants.ErrorMessage, ex.Message);
            }

            return apiResponse;
        }


        //public bool IsRequestAllowed(string UserId, int maxRequests, TimeSpan windowDuration)
        //{
        //    var now = DateTime.UtcNow;

        //    lock (_lock)
        //    {
        //        // 1. Remove timestamps that are outside the rolling window
        //        while (_requestTimestamps.Count > 0 &&
        //               (now - _requestTimestamps.Peek()) > windowDuration)
        //        {
        //            _requestTimestamps.Dequeue();
        //        }

        //        // 2. Check if the user has reached the limit
        //        if (_requestTimestamps.Count < maxRequests)
        //        {
        //            // Allow request: record the timestamp
        //            _requestTimestamps.Enqueue(now);
        //            return true;
        //        }

        //        // Block request
        //        return false;
        //    }
        //}
    }
}
