using System;
using Portable.Xaml.Markup;

namespace RC.Common.Helpers.Markup.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(int))]
    public class IntExtension : MarkupExtension
    {
        public IntExtension() { }

        public IntExtension(string value)
        {
            Value = value;
        }

        public string Value
        {
            get => _value.ToString();
            set
            {
                int.TryParse(value, out var intValue);
                _value = intValue;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _value;
        }

        private int _value;
    }
}
