using Newtonsoft.Json;

namespace TelegramBotBARS_WebAPI.JsonConverters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;

        public DateOnlyConverter() : this(null)
        {
        }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.ReadAsString();
            return DateOnly.Parse(value!);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
           => writer.WriteValue(value.ToString(serializationFormat));
    }
}
