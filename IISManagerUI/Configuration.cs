using System.Configuration;

namespace IISManagerUI
{
    public static class Configuration
    {
        private const int refreshingIntervalDefault = 1000;
        public static int RefreshingInterval
        {
            get
            {
                var refreshingIntervalRaw = ConfigurationManager.AppSettings["RefreshingInterval"];
                if (!string.IsNullOrEmpty(refreshingIntervalRaw) && int.TryParse(refreshingIntervalRaw, out var refreshingInterval))
                {
                    return refreshingInterval;
                }

                return refreshingIntervalDefault;
            }
        }
    }
}
