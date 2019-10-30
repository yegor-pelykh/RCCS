using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RC.Common.Helpers.Markup.Converters
{
    public class IsGreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is double param))
                return false;

            return (value is int vi && vi > param) || (value is double vd && vd > param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
