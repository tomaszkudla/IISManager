using System;
using System.Globalization;
using System.Windows.Data;

namespace IISManagerUI.Converters
{
    [ValueConversion(typeof(double), typeof(Thresholds))]
    public class ThresholdConverter : IValueConverter
    {
        public double MediumThreshold { get; set; }
        public double HighThreshold { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataValue = (double)value;
            if (dataValue > HighThreshold)
            {
                return Thresholds.High;
            }

            if (dataValue > MediumThreshold)
            {
                return Thresholds.Medium;
            }

            return Thresholds.Low;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
