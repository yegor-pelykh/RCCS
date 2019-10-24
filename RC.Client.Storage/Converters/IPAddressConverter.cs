using System;
using System.Net;
using Newtonsoft.Json;

namespace RC.Client.Storage.Converters
{
    class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPAddress);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            return value != null
                ? IPAddress.Parse((string) value)
                : null;
        }

    }

}
