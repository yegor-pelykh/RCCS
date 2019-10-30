using System;
using Portable.Xaml.Markup;

namespace RC.Common.Helpers.Markup.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(uint))]
    public class UIntExtension : MarkupExtension
    {
        public UIntExtension() { }

        public UIntExtension(string value)
        {
            Value = value;
        }

        public string Value
        {
            get => _value.ToString();
            set
            {
                uint.TryParse(value, out var uintValue);
                _value = uintValue;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _value;
        }

        private uint _value;
    }
}
