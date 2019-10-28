using System;
using System.Globalization;
using System.Net;
using Avalonia.Data.Converters;

namespace RC.Common.Helpers.Markup.Converters
{
    public class IPAddressToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is IPAddress ip))
                return null;

            return ip.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string str))
                return null;

            if (!IPAddress.TryParse(str, out var address))
                return null;

            return address;
        }

    }

}
