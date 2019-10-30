using System;
using System.Globalization;
using Portable.Xaml.Markup;

namespace RC.Common.Helpers.Markup.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(double))]
    public class DoubleExtension : MarkupExtension
    {
        public DoubleExtension() { }

        public DoubleExtension(string value)
        {
            Value = value;
        }

        public string Value
        {
            get => _value.ToString(CultureInfo.InvariantCulture);
            set
            {
                double.TryParse(value, out var doubleValue);
                _value = doubleValue;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _value;
        }

        private double _value;

    }
}
