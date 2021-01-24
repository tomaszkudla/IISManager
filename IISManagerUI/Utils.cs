using System;
using System.Management;
using System.Windows;

namespace IISManagerUI
{
    public static class Utils
    {
        static readonly Lazy<double?> totalMemory = new Lazy<double?>(DoGetTotalMemory());
        public static void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static double? GetTotalMemory()
        {
            return totalMemory?.Value;
        }

        static double? DoGetTotalMemory()
        {
            var capacity = 0.0;
            
            try
            {
                var query = new ObjectQuery("SELECT * FROM CIM_OperatingSystem");
                var searcher = new ManagementObjectSearcher(query);

                foreach (var item in searcher.Get())
                {
                    capacity += Convert.ToDouble(item["TotalVisibleMemorySize"]);
                }
            }
            catch
            {
                return null;
            }

            return capacity / 1024.0;
        }
    }
}
