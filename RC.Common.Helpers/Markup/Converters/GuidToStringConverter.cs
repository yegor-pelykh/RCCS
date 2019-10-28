using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RC.Common.Helpers.Markup.Converters
{
    public class GuidToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Guid guid))
                return null;

            return guid.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str))
                return null;

            if (!Guid.TryParse(str, out var guid))
                return null;

            return guid;
        }

    }

}
