using System.Configuration;

namespace IISManagerUI
{
    public static class Configuration
    {
        public static int RefreshingInterval
        {
            get
            {
                var valueRaw = ConfigurationManager.AppSettings["RefreshingInterval"];
                if (!string.IsNullOrEmpty(valueRaw) && int.TryParse(valueRaw, out var value))
                {
                    return value;
                }

                return 1000;
            }
        }

        public static double CpuUsageMediumThreshold
        {
            get
            {
                var valueRaw = ConfigurationManager.AppSettings["CpuUsageMediumThreshold"];
                if (!string.IsNullOrEmpty(valueRaw) && double.TryParse(valueRaw, out var value))
                {
                    return value;
                }

                return 10.0;
            }
        }

        public static double CpuUsageHighThreshold
        {
            get
            {
                var valueRaw = ConfigurationManager.AppSettings["CpuUsageHighThreshold"];
                if (!string.IsNullOrEmpty(valueRaw) && double.TryParse(valueRaw, out var value))
                {
                    return value;
                }

                return 30.0;
            }
        }

        public static double MemoryUsageMediumThreshold
        {
            get
            {
                var valueRaw = ConfigurationManager.AppSettings["MemoryUsageMediumThreshold"];
                if (!string.IsNullOrEmpty(valueRaw) && double.TryParse(valueRaw, out var value))
                {
                    return value;
                }

                return 10.0;
            }
        }

        public static double MemoryUsageHighThreshold
        {
            get
            {
                var valueRaw = ConfigurationManager.AppSettings["MemoryUsageHighThreshold"];
                if (!string.IsNullOrEmpty(valueRaw) && double.TryParse(valueRaw, out var value))
                {
                    return value;
                }

                return 30.0;
            }
        }
    }
}
