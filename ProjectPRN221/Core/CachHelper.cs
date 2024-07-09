using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ProjectPRN221.Core
{
    public static class CachHelper
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
                                                   string recordId,
                                                   T data,
                                                   TimeSpan? absoluteExpireTime = null,
                                                   TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = slidingExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache,
                                                       string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

		public static async Task RemoveRecordAsync(this IDistributedCache cache,
												   string recordId,
												   ILogger logger = null)
		{
			try
			{
				await cache.RemoveAsync(recordId);
			}
			catch (Exception ex)
			{
				logger?.LogError(ex, "Error removing cache record for {RecordId}", recordId);
				throw;
			}
		}
	}
}
