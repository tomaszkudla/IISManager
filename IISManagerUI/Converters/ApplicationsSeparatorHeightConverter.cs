using IISManager.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace IISManagerUI.Converters
{
    [ValueConversion(typeof(ApplicationPool), typeof(int))]
    public class ApplicationsSeparatorHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var applicationPool = (ApplicationPool)value;

            var anyWorkingProcesses = applicationPool.WorkerProcesses.Value.Count > 0;
            var anyApplications = applicationPool.Applications.Value.Count > 0;

            if ((anyWorkingProcesses && !anyApplications))
            {
                return 1;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
