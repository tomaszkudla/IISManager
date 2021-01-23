using System;
using System.Globalization;
using System.Windows.Data;

namespace IISManagerUI.Converters
{
    [ValueConversion(typeof(string), typeof(Thresholds))]
    public class ThresholdConverter : IValueConverter
    {
        public string Unit { get; set; }
        public double MediumThreshold { get; set; }
        public double HighThreshold { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataValueString = ((string)value).Replace(Unit, string.Empty);

            if (double.TryParse(dataValueString, out var dataValue))
            {
                if (dataValue > HighThreshold)
                {
                    return Thresholds.High;
                }

                if (dataValue > MediumThreshold)
                {
                    return Thresholds.Medium;
                }
            }

            return Thresholds.Low;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
