namespace EventBookSystem.Common.Settings
{
    public class CacheSettings
    {
        public int ResponseCacheDuration { get; set; }
        public int SlidingExpiration { get; set; }
    }
}