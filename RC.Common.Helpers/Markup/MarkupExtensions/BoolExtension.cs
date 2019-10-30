using System;
using Portable.Xaml.Markup;

namespace RC.Common.Helpers.Markup.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(bool))]
    public class BoolExtension : MarkupExtension
    {
        public BoolExtension() { }

        public BoolExtension(string value)
        {
            Value = value;
        }
        
        public string Value
        {
            get => _value.ToString();
            set
            {
                bool.TryParse(value, out var boolValue);
                _value = boolValue;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _value;
        }

        private bool _value;

    }
}
